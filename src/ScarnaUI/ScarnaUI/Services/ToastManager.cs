namespace ScarnaUI.Services;

/// <summary>
/// Реализация <see cref="IToastManager"/>. Регистрируется как Scoped.
/// Чистое состояние + событие — вся отрисовка находится в ScarnaSnackbarProvider.
/// </summary>
internal sealed class ToastManager : IToastManager
{
    private static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(4);

    // Add может вызываться из любого потока (например, из фонового таймера приложения), а
    // авто-удаление — из продолжения Task.Delay; ScarnaSnackbarProvider при этом читает
    // очередь с потока рендера. Блокировка + снимок исключают "Collection was modified".
    private readonly object _gate = new();
    private readonly List<ScarnaToast> _toasts = new();

    public event Action? OnChange;

    public IReadOnlyList<ScarnaToast> Toasts
    {
        get
        {
            lock (_gate)
            {
                return _toasts.ToArray();
            }
        }
    }

    public void Add(string message, ScarnaToastSeverity severity = ScarnaToastSeverity.Info, TimeSpan? duration = null)
    {
        var toast = new ScarnaToast(message, severity, duration ?? DefaultDuration);

        lock (_gate)
        {
            _toasts.Add(toast);
        }

        OnChange?.Invoke();

        if (toast.Duration > TimeSpan.Zero)
        {
            _ = AutoRemoveAsync(toast);
        }
    }

    public void Remove(Guid id)
    {
        bool removed;
        lock (_gate)
        {
            removed = _toasts.RemoveAll(t => t.Id == id) > 0;
        }

        if (removed)
        {
            OnChange?.Invoke();
        }
    }

    private async Task AutoRemoveAsync(ScarnaToast toast)
    {
        await Task.Delay(toast.Duration);
        Remove(toast.Id);
    }
}
