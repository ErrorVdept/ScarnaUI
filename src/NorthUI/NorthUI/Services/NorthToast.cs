namespace NorthUI.Services;

/// <summary>Смысловая окраска уведомления.</summary>
public enum NorthToastSeverity
{
    Info,
    Success,
    Warning,
    Error,
}

/// <summary>Одно уведомление в очереди <see cref="IToastManager"/>.</summary>
public sealed class NorthToast
{
    internal NorthToast(string message, NorthToastSeverity severity, TimeSpan duration)
    {
        Message = message;
        Severity = severity;
        Duration = duration;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public string Message { get; }

    public NorthToastSeverity Severity { get; }

    /// <summary>Через сколько уведомление скроется само. TimeSpan.Zero — не скрывать автоматически.</summary>
    public TimeSpan Duration { get; }
}
