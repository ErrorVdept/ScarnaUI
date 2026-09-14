namespace ScarnaUI.Services;

/// <summary>
/// Императивный сервис показа тостов (снэкбаров) — аналог ISnackbar в MudBlazor.
/// Инжектируется в любой компонент: <c>ToastManager.Add("Сохранено")</c>.
/// Требует размещённого в layout &lt;ScarnaSnackbarProvider /&gt;, которая и рисует очередь.
/// Реализация — чистый C#, без JS: авто-скрытие сделано на Task.Delay-таймерах.
/// </summary>
public interface IToastManager
{
    /// <summary>Текущая очередь показанных уведомлений.</summary>
    IReadOnlyList<ScarnaToast> Toasts { get; }

    /// <summary>Срабатывает при добавлении/удалении любого тоста.</summary>
    event Action? OnChange;

    /// <summary>Добавляет уведомление в очередь. duration = TimeSpan.Zero — не скрывать автоматически (нужна ручная кнопка закрытия).</summary>
    void Add(string message, ScarnaToastSeverity severity = ScarnaToastSeverity.Info, TimeSpan? duration = null);

    /// <summary>Убирает конкретное уведомление раньше срока (например, по клику на крестик).</summary>
    void Remove(Guid id);
}
