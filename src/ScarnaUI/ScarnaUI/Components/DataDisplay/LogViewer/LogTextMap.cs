using System.IO.MemoryMappedFiles;
using System.Text;
using System.Text.RegularExpressions;

namespace ScarnaUI.Components.DataDisplay.LogViewer;

/// <summary>Временное UTF-16 представление для полного поиска .NET Regex без string размером с файл.</summary>
internal sealed class LogTextMap : IAsyncDisposable
{
    private readonly string _path;
    private readonly FileStream _file;
    private readonly int _length;

    private LogTextMap(string path, FileStream file, int length) => (_path, _file, _length) = (path, file, length);

    public static async Task<LogTextMap> CreateAsync(LogSnapshot snapshot, CancellationToken token)
    {
        var path = Path.Combine(Path.GetTempPath(), $"scarna-log-regex-{Guid.NewGuid():N}.tmp");
        FileStream? file = null;
        try
        {
            file = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read,
                65536, FileOptions.Asynchronous | FileOptions.SequentialScan);
            using var reader = snapshot.OpenReader();
            await using (var writer = new StreamWriter(file, new UnicodeEncoding(false, false), 65536, leaveOpen: true))
            {
                var buffer = new char[65536];
                int count;
                while ((count = await reader.ReadAsync(buffer.AsMemory(), token)) != 0)
                    await writer.WriteAsync(buffer.AsMemory(0, count), token);
                await writer.FlushAsync(token);
            }
            if (file.Length / 2 > int.MaxValue) throw new InvalidDataException("Многострочный поиск поддерживает не более 2 147 483 647 символов.");
            return new LogTextMap(path, file, checked((int)(file.Length / 2)));
        }
        catch
        {
            if (file is not null) await file.DisposeAsync();
            File.Delete(path);
            throw;
        }
    }

    public unsafe List<int> Search(Regex expression, CancellationToken token)
    {
        if (_length == 0) return SearchSpan(ReadOnlySpan<char>.Empty, expression, token);
        using var map = MemoryMappedFile.CreateFromFile(_file, null, 0, MemoryMappedFileAccess.Read, HandleInheritability.None, leaveOpen: true);
        using var view = map.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
        byte* pointer = null;
        try
        {
            view.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);
            return SearchSpan(new ReadOnlySpan<char>(pointer + view.PointerOffset, _length), expression, token);
        }
        finally { if (pointer != null) view.SafeMemoryMappedViewHandle.ReleasePointer(); }
    }

    public unsafe List<int> Filter(int lineCount, Regex? include, Regex? exclude, CancellationToken token)
    {
        if (_length == 0) return FilterSpan(ReadOnlySpan<char>.Empty, lineCount, include, exclude, token);
        using var map = MemoryMappedFile.CreateFromFile(_file, null, 0, MemoryMappedFileAccess.Read, HandleInheritability.None, leaveOpen: true);
        using var view = map.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
        byte* pointer = null;
        try
        {
            view.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);
            return FilterSpan(new ReadOnlySpan<char>(pointer + view.PointerOffset, _length), lineCount, include, exclude, token);
        }
        finally { if (pointer != null) view.SafeMemoryMappedViewHandle.ReleasePointer(); }
    }

    private static List<int> Starts(ReadOnlySpan<char> text, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var starts = new List<int> { 0 };
        for (var i = 0; i < text.Length; i++)
        {
            if ((i & 65535) == 0) token.ThrowIfCancellationRequested();
            if (text[i] == '\r') { if (i + 1 < text.Length && text[i + 1] == '\n') i++; starts.Add(i + 1); }
            else if (text[i] == '\n') starts.Add(i + 1);
        }
        return starts;
    }

    private static int LineAt(List<int> starts, int offset)
    {
        var index = starts.BinarySearch(offset);
        return index >= 0 ? index : ~index - 1;
    }

    private static List<int> SearchSpan(ReadOnlySpan<char> text, Regex expression, CancellationToken token)
    {
        var starts = Starts(text, token);
        var result = new List<int>();
        foreach (var match in expression.EnumerateMatches(text))
        {
            token.ThrowIfCancellationRequested();
            result.Add(LineAt(starts, match.Index) + 1);
        }
        return result;
    }

    private static List<int> FilterSpan(ReadOnlySpan<char> text, int lineCount, Regex? include, Regex? exclude, CancellationToken token)
    {
        var starts = Starts(text, token);
        var included = new bool[lineCount]; var excluded = new bool[lineCount];
        if (include is null) Array.Fill(included, true); else Mark(text, starts, include, included, token);
        if (exclude is not null) Mark(text, starts, exclude, excluded, token);
        var result = new List<int>();
        for (var i = 0; i < lineCount; i++) { if ((i & 65535) == 0) token.ThrowIfCancellationRequested(); if (included[i] && !excluded[i]) result.Add(i + 1); }
        return result;
    }

    private static void Mark(ReadOnlySpan<char> text, List<int> starts, Regex expression, bool[] marked, CancellationToken token)
    {
        foreach (var match in expression.EnumerateMatches(text))
        {
            token.ThrowIfCancellationRequested();
            var first = LineAt(starts, match.Index);
            var last = LineAt(starts, match.Index + Math.Max(0, match.Length - 1));
            for (var i = first; i <= last && i < marked.Length; i++) marked[i] = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _file.DisposeAsync();
        File.Delete(_path);
    }
}
