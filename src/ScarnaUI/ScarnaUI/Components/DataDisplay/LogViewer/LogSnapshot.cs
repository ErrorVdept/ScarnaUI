using System.Text;
using System.Runtime.CompilerServices;

namespace ScarnaUI.Components.DataDisplay.LogViewer;

/// <summary>
/// Временный снимок UTF-8 журнала с индексом начала строк. Содержимое хранится на
/// диске, а в памяти находятся только смещения; исходный поток остаётся открытым.
/// </summary>
internal sealed class LogSnapshot : IAsyncDisposable
{
    private readonly string _path;
    private readonly FileStream _file;
    private readonly long[] _starts;
    private readonly long _length;

    private LogSnapshot(string path, FileStream file, long[] starts, long length)
        => (_path, _file, _starts, _length) = (path, file, starts, length);

    public int LineCount => _starts.Length;

    public StreamReader OpenReader() => new(new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete,
        65536, FileOptions.Asynchronous | FileOptions.SequentialScan), Encoding.UTF8, true, 65536);

    public async Task<string> ReadAllAsync(CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete,
            65536, FileOptions.Asynchronous | FileOptions.SequentialScan);
        using var reader = new StreamReader(stream, Encoding.UTF8, true);
        return await reader.ReadToEndAsync(cancellationToken);
    }

    public async IAsyncEnumerable<string> ReadLinesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete,
            65536, FileOptions.Asynchronous | FileOptions.SequentialScan);
        using var reader = new StreamReader(stream, Encoding.UTF8, true, 65536);
        for (var i = 0; i < LineCount; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return await reader.ReadLineAsync(cancellationToken) ?? string.Empty;
        }
    }

    public static async Task<LogSnapshot> CreateAsync(Stream source, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(source);
        var path = Path.Combine(Path.GetTempPath(), $"scarna-log-{Guid.NewGuid():N}.tmp");
        FileStream? file = null;
        try
        {
            file = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read | FileShare.Delete,
                65536, FileOptions.Asynchronous | FileOptions.RandomAccess);
            var starts = new List<long> { 0 };
            var buffer = new byte[65536];
            long length = 0;
            bool pendingCr = false;
            int count;
            while ((count = await source.ReadAsync(buffer, cancellationToken)) != 0)
            {
                await file.WriteAsync(buffer.AsMemory(0, count), cancellationToken);
                for (var i = 0; i < count; i++)
                {
                    var position = length + i;
                    var value = buffer[i];
                    if (pendingCr)
                    {
                        if (value == (byte)'\n')
                        {
                            starts.Add(position + 1);
                            pendingCr = false;
                            continue;
                        }
                        starts.Add(position);
                        pendingCr = false;
                    }
                    if (value == (byte)'\r') pendingCr = true;
                    else if (value == (byte)'\n') starts.Add(position + 1);
                }
                length += count;
            }
            if (pendingCr) starts.Add(length);
            await file.FlushAsync(cancellationToken);
            if (length >= 2)
            {
                var bom = new byte[4];
                var bomLength = await RandomAccess.ReadAsync(file.SafeFileHandle, bom, 0, cancellationToken);
                if ((bom[0] == 0xff && bom[1] == 0xfe) || (bom[0] == 0xfe && bom[1] == 0xff) ||
                    (bomLength >= 4 && bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff))
                    throw new InvalidDataException("Поддерживаются только журналы в кодировке UTF-8.");
            }
            return new LogSnapshot(path, file, starts.ToArray(), length);
        }
        catch
        {
            if (file is not null) await file.DisposeAsync();
            File.Delete(path);
            throw;
        }
    }

    public async ValueTask<string> ReadLineAsync(int lineNumber, CancellationToken cancellationToken = default)
    {
        if (lineNumber < 1 || lineNumber > LineCount) throw new ArgumentOutOfRangeException(nameof(lineNumber));
        var index = lineNumber - 1;
        var start = _starts[index];
        var end = index + 1 < _starts.Length ? _starts[index + 1] : _length;
        var byteCount = checked((int)(end - start));
        var bytes = new byte[byteCount];
        var read = 0;
        while (read < byteCount)
        {
            var n = await RandomAccess.ReadAsync(_file.SafeFileHandle, bytes.AsMemory(read), start + read, cancellationToken);
            if (n == 0) throw new EndOfStreamException();
            read += n;
        }
        var text = Encoding.UTF8.GetString(bytes);
        if (index == 0 && text.StartsWith('\uFEFF')) text = text[1..];
        return text.TrimEnd('\r', '\n');
    }

    public async ValueTask DisposeAsync()
    {
        await _file.DisposeAsync();
        File.Delete(_path);
    }
}
