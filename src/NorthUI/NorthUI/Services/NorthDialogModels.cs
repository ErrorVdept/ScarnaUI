namespace NorthUI.Services;

/// <summary>Ширина панели диалога.</summary>
public enum NorthDialogSize
{
    Small,
    Medium,
    Large,
    FullScreen,
}

/// <summary>Настройки конкретного вызова <see cref="INorthDialogService.ShowAsync"/>.</summary>
public sealed class NorthDialogOptions
{
    /// <summary>Заголовок, если консьюмер не рисует свой через NorthDialog.Title.</summary>
    public string? Title { get; set; }

    public NorthDialogSize Size { get; set; } = NorthDialogSize.Medium;

    /// <summary>Закрывать ли диалог по нажатию Escape (результат — NorthDialogResult.Cancel()).</summary>
    public bool CloseOnEscapeKey { get; set; } = true;

    /// <summary>Закрывать ли диалог по клику на затемнённую подложку.</summary>
    public bool CloseOnBackdropClick { get; set; } = true;
}

/// <summary>
/// Результат закрытия диалога — аналог DialogResult в MudBlazor.
/// <see cref="Cancelled"/> = true означает закрытие без подтверждения (Escape, подложка,
/// явный Cancel-вызов), а не факт наличия данных.
/// </summary>
public sealed class NorthDialogResult
{
    private NorthDialogResult(bool cancelled, object? data)
    {
        Cancelled = cancelled;
        Data = data;
    }

    public bool Cancelled { get; }

    public object? Data { get; }

    public static NorthDialogResult Ok(object? data = null) => new(cancelled: false, data);

    public static NorthDialogResult Cancel() => new(cancelled: true, data: null);

    /// <summary>Строго типизированный доступ к Data.</summary>
    public T? DataAs<T>() => Data is T typed ? typed : default;
}

/// <summary>
/// Живая ссылка на открытый диалог. Провайдер создаёт её при вызове ShowAsync и кладёт
/// в каскад — компонент содержимого диалога получает её через
/// <c>[CascadingParameter] NorthDialogReference DialogReference</c> и вызывает
/// <see cref="Close"/>, когда пользователь подтвердил/отменил действие.
/// </summary>
public sealed class NorthDialogReference
{
    private readonly TaskCompletionSource<NorthDialogResult> _completion =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    internal NorthDialogReference(Type componentType, IReadOnlyDictionary<string, object?> parameters, NorthDialogOptions options)
    {
        ComponentType = componentType;
        Parameters = parameters;
        Options = options;
    }

    public Guid Id { get; } = Guid.NewGuid();

    internal Type ComponentType { get; }

    internal IReadOnlyDictionary<string, object?> Parameters { get; }

    public NorthDialogOptions Options { get; }

    /// <summary>Завершается, когда диалог закрыт — то, что возвращает ShowAsync вызывающему коду.</summary>
    public Task<NorthDialogResult> Result => _completion.Task;

    /// <summary>Закрывает диалог с указанным результатом (по умолчанию — успешное закрытие без данных).</summary>
    public void Close(NorthDialogResult? result = null) => _completion.TrySetResult(result ?? NorthDialogResult.Ok());
}
