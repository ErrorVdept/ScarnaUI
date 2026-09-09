namespace NorthUI.Services;

/// <summary>
/// Реализация <see cref="IToastManager"/>. Регистрируется как Scoped.
/// Чистое состояние + событие — вся отрисовка находится в NorthSnackbarProvider.
/// </summary>
internal sealed class ToastManager : IToastManager
{
    private static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(4);

    private readonly List<NorthToast> _toasts = new();

    public event Action? OnChange;

    public IReadOnlyList<NorthToast> Toasts => _toasts;

    public void Add(string message, NorthToastSeverity severity = NorthToastSeverity.Info, TimeSpan? duration = null)
    {
        var toast = new NorthToast(message, severity, duration ?? DefaultDuration);
        _toasts.Add(toast);
        OnChange?.Invoke();

        if (toast.Duration > TimeSpan.Zero)
        {
            _ = AutoRemoveAsync(toast);
        }
    }

    public void Remove(Guid id)
    {
        if (_toasts.RemoveAll(t => t.Id == id) > 0)
        {
            OnChange?.Invoke();
        }
    }

    private async Task AutoRemoveAsync(NorthToast toast)
    {
        await Task.Delay(toast.Duration);
        Remove(toast.Id);
    }
}
