using Microsoft.AspNetCore.Components;

namespace NorthUI.Services;

/// <summary>
/// Реализация <see cref="INorthDialogService"/>. Хранит список открытых диалогов и
/// уведомляет подписчиков (на практике — единственного: <c>NorthDialogProvider</c>)
/// об изменениях через <see cref="OnChange"/>. Сама по себе не рендерит ничего —
/// это чистое состояние + событие, вся разметка находится в NorthDialogProvider.
/// Регистрируется как Scoped (см. NorthUiServiceCollectionExtensions).
/// </summary>
internal sealed class NorthDialogService : INorthDialogService
{
    private readonly List<NorthDialogReference> _dialogs = new();

    /// <summary>Срабатывает при открытии/закрытии любого диалога.</summary>
    public event Action? OnChange;

    /// <summary>Текущие открытые диалоги, в порядке открытия (последний — самый верхний).</summary>
    public IReadOnlyList<NorthDialogReference> Dialogs => _dialogs;

    public Task<NorthDialogResult> ShowAsync<TComponent>(NorthDialogOptions? options = null)
        where TComponent : IComponent
        => ShowAsync(typeof(TComponent), null, options);

    public Task<NorthDialogResult> ShowAsync<TComponent>(IReadOnlyDictionary<string, object?> parameters, NorthDialogOptions? options = null)
        where TComponent : IComponent
        => ShowAsync(typeof(TComponent), parameters, options);

    public Task<NorthDialogResult> ShowAsync(Type componentType, IReadOnlyDictionary<string, object?>? parameters = null, NorthDialogOptions? options = null)
    {
        var reference = new NorthDialogReference(componentType, parameters ?? new Dictionary<string, object?>(), options ?? new NorthDialogOptions());
        _dialogs.Add(reference);
        OnChange?.Invoke();

        // Как только диалог закрылся (Reference.Close вызван из содержимого или из провайдера) —
        // убираем его из списка и снова уведомляем подписчиков, чтобы панель исчезла из DOM.
        _ = reference.Result.ContinueWith(_ =>
        {
            _dialogs.Remove(reference);
            OnChange?.Invoke();
        }, TaskScheduler.Default);

        return reference.Result;
    }

    public void Close(NorthDialogReference reference, NorthDialogResult? result = null) => reference.Close(result);
}
