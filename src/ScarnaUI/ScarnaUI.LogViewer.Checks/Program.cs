using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ScarnaUI.Components.DataDisplay.LogViewer;

await CheckLines("", [""]);
await CheckLines("\uFEFFпервый\r\nвторой\rтретий\n\n", ["первый", "второй", "третий", "", ""]);
await CheckLines(new string('x', 65535) + "\r\nпосле", [new string('x', 65535), "после"]);
await CheckLines("a\rb\n", ["a", "b", ""]);
await CheckMultiline("начало\r\nошибка: один\nпродолжение\rконец\n", ["ошибка:.*?продолжение", "(?<=ошибка: )один", "^конец$", "(?=о)", "(один).*?\\1", "\\z"]);
await CheckMultiline("", ["^$", "\\z"]);
await CheckMultiline(new string('x', 65535) + "😀\r\nконец", ["😀\\r\\nконец", "(?<=😀)\\r\\n"]);

using (var utf16 = new MemoryStream([0xff, 0xfe, 0x61, 0x00]))
{
    try { await using var bad = await LogSnapshot.CreateAsync(utf16, default); throw new Exception("UTF-16 was accepted"); }
    catch (InvalidDataException) { }
}

using (var cancelled = new CancellationTokenSource())
using (var source = new MemoryStream(Encoding.UTF8.GetBytes("test")))
{
    cancelled.Cancel();
    try { await using var bad = await LogSnapshot.CreateAsync(source, cancelled.Token); throw new Exception("Cancellation was ignored"); }
    catch (OperationCanceledException) { }
}

Console.WriteLine("Snapshot checks passed.");

if (args.Contains("--benchmark"))
{
    const int sizeMiB = 200;
    using var source = new RepeatingStream((long)sizeMiB * 1024 * 1024);
    var stopwatch = Stopwatch.StartNew();
    await using var snapshot = await LogSnapshot.CreateAsync(source, default);
    var indexSeconds = stopwatch.Elapsed.TotalSeconds;
    var checksum = 0L;
    var lines = 0;
    stopwatch.Restart();
    await foreach (var line in snapshot.ReadLinesAsync(default)) { checksum += line.Length; lines++; }
    Console.WriteLine($"{sizeMiB} MiB: index {indexSeconds:F2}s; sequential scan {stopwatch.Elapsed.TotalSeconds:F2}s; lines {lines}; checksum {checksum}; managed memory {GC.GetTotalMemory(true) / 1024 / 1024} MiB");
    stopwatch.Restart();
    await using var map = await LogTextMap.CreateAsync(snapshot, default);
    var found = map.Search(new Regex("x{10}\\n", RegexOptions.Multiline | RegexOptions.Singleline, TimeSpan.FromSeconds(30)), default);
    using var process = Process.GetCurrentProcess(); process.Refresh();
    Console.WriteLine($"{sizeMiB} MiB: multiline conversion + regex {stopwatch.Elapsed.TotalSeconds:F2}s; matches {found.Count}; managed memory {GC.GetTotalMemory(true) / 1024 / 1024} MiB; peak working set {process.PeakWorkingSet64 / 1024 / 1024} MiB");
}

static async Task CheckMultiline(string text, string[] patterns)
{
    using var source = new MemoryStream(Encoding.UTF8.GetBytes(text));
    await using var snapshot = await LogSnapshot.CreateAsync(source, default);
    await using var map = await LogTextMap.CreateAsync(snapshot, default);
    var starts = new List<int> { 0 };
    for (var i = 0; i < text.Length; i++)
        if (text[i] == '\r') { if (i + 1 < text.Length && text[i + 1] == '\n') i++; starts.Add(i + 1); }
        else if (text[i] == '\n') starts.Add(i + 1);
    int LineAt(int offset) { var i = starts.BinarySearch(offset); return i >= 0 ? i : ~i - 1; }
    foreach (var pattern in patterns)
    {
        var regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.Singleline, TimeSpan.FromSeconds(2));
        var matches = regex.Matches(text).Cast<Match>().ToArray();
        var expected = matches.Select(m => LineAt(m.Index) + 1).ToArray();
        if (!expected.SequenceEqual(map.Search(regex, default))) throw new Exception($"Search differs: {pattern}");
        var expectedLines = new bool[snapshot.LineCount];
        foreach (var match in matches)
            for (var i = LineAt(match.Index); i <= LineAt(match.Index + Math.Max(0, match.Length - 1)); i++) expectedLines[i] = true;
        var filtered = Enumerable.Range(1, snapshot.LineCount).Where(i => expectedLines[i - 1]).ToArray();
        if (!filtered.SequenceEqual(map.Filter(snapshot.LineCount, regex, null, default))) throw new Exception($"Filter differs: {pattern}");
        var excluded = Enumerable.Range(1, snapshot.LineCount).Where(i => !expectedLines[i - 1]).ToArray();
        if (!excluded.SequenceEqual(map.Filter(snapshot.LineCount, null, regex, default))) throw new Exception($"Exclude filter differs: {pattern}");
    }
}

static async Task CheckLines(string input, string[] expected)
{
    using var source = new ChunkedStream(Encoding.UTF8.GetBytes(input));
    await using var snapshot = await LogSnapshot.CreateAsync(source, default);
    if (!source.CanRead || snapshot.LineCount != expected.Length) throw new Exception("Incorrect line count or source closed");
    var sequential = new List<string>();
    await foreach (var line in snapshot.ReadLinesAsync(default)) sequential.Add(line);
    if (!expected.SequenceEqual(sequential)) throw new Exception("Sequential lines differ");
    for (var i = 0; i < expected.Length; i++)
        if (await snapshot.ReadLineAsync(i + 1) != expected[i]) throw new Exception($"Random line {i + 1} differs");
    if (await snapshot.ReadAllAsync(default) != input.TrimStart('\uFEFF')) throw new Exception("Full text differs");
}

sealed class ChunkedStream(byte[] data) : MemoryStream(data)
{
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        => base.ReadAsync(buffer[..Math.Min(buffer.Length, 7)], cancellationToken);
}

sealed class RepeatingStream(long length) : Stream
{
    private static readonly byte[] Row = Encoding.UTF8.GetBytes(new string('x', 199) + "\n");
    private long _position;
    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => length;
    public override long Position { get => _position; set => throw new NotSupportedException(); }
    public override int Read(byte[] buffer, int offset, int count)
    {
        count = (int)Math.Min(count, length - _position);
        for (var i = 0; i < count; i++) buffer[offset + i] = Row[(int)((_position + i) % Row.Length)];
        _position += count;
        return count;
    }
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var count = (int)Math.Min(buffer.Length, length - _position);
        for (var i = 0; i < count; i++) buffer.Span[i] = Row[(int)((_position + i) % Row.Length)];
        _position += count;
        return ValueTask.FromResult(count);
    }
    public override void Flush() { }
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
}
