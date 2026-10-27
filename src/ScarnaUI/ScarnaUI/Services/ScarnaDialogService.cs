using Microsoft.AspNetCore.Components;

namespace ScarnaUI.Services;

/// <summary>
/// Реализация <see cref="IScarnaDialogService"/>. Хранит список открытых диалогов и
/// уведомляет подписчиков (на практике — единственного: <c>ScarnaDialogProvider</c>)
/// об изменениях через <see cref="OnChange"/>. Сама по себе не рендерит ничего —
/// это чистое состояние + событие, вся разметка находится в ScarnaDialogProvider.
/// Регистрируется как Scoped (см. ScarnaUiServiceCollectionExtensions).
/// </summary>
internal sealed class ScarnaDialogService : IScarnaDialogService
{
    // Закрытие диалога обрабатывается в ContinueWith на потоке из пула (TaskScheduler.Default),
    // а Add/чтение списка идут с потока рендера — без блокировки одновременное закрытие и
    // открытие/перерисовка давали "Collection was modified" при enumerate в ScarnaDialogProvider.
    private readonly object _gate = new();
    private readonly List<ScarnaDialogReference> _dialogs = new();

    /// <summary>Срабатывает при открытии/закрытии любого диалога.</summary>
    public event Action? OnChange;

    /// <summary>Текущие открытые диалоги, в порядке открытия (последний — самый верхний). Возвращает снимок.</summary>
    public IReadOnlyList<ScarnaDialogReference> Dialogs
    {
        get
        {
            lock (_gate)
            {
                return _dialogs.ToArray();
            }
        }
    }

    public Task<ScarnaDialogResult> ShowAsync<TComponent>(ScarnaDialogOptions? options = null)
        where TComponent : IComponent
        => ShowAsync(typeof(TComponent), null, options);

    public Task<ScarnaDialogResult> ShowAsync<TComponent>(IReadOnlyDictionary<string, object?> parameters, ScarnaDialogOptions? options = null)
        where TComponent : IComponent
        => ShowAsync(typeof(TComponent), parameters, options);

    public Task<ScarnaDialogResult> ShowAsync(Type componentType, IReadOnlyDictionary<string, object?>? parameters = null, ScarnaDialogOptions? options = null)
    {
        var reference = new ScarnaDialogReference(componentType, parameters ?? new Dictionary<string, object?>(), options ?? new ScarnaDialogOptions());
        lock (_gate)
        {
            _dialogs.Add(reference);
        }

        OnChange?.Invoke();

        // Как только диалог закрылся (Reference.Close вызван из содержимого или из провайдера) —
        // убираем его из списка и снова уведомляем подписчиков, чтобы панель исчезла из DOM.
        _ = reference.Result.ContinueWith(_ =>
        {
            lock (_gate)
            {
                _dialogs.Remove(reference);
            }

            OnChange?.Invoke();
        }, TaskScheduler.Default);

        return reference.Result;
    }

    public void Close(ScarnaDialogReference reference, ScarnaDialogResult? result = null) => reference.Close(result);
}
