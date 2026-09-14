namespace ScarnaUI.Services;

/// <summary>Смысловая окраска уведомления.</summary>
public enum ScarnaToastSeverity
{
    Info,
    Success,
    Warning,
    Error,
}

/// <summary>Одно уведомление в очереди <see cref="IToastManager"/>.</summary>
public sealed class ScarnaToast
{
    internal ScarnaToast(string message, ScarnaToastSeverity severity, TimeSpan duration)
    {
        Message = message;
        Severity = severity;
        Duration = duration;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public string Message { get; }

    public ScarnaToastSeverity Severity { get; }

    /// <summary>Через сколько уведомление скроется само. TimeSpan.Zero — не скрывать автоматически.</summary>
    public TimeSpan Duration { get; }
}
