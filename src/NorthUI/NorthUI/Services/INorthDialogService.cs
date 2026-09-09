using Microsoft.AspNetCore.Components;

namespace NorthUI.Services;

/// <summary>
/// Императивный сервис показа модальных диалогов — аналог IDialogService в MudBlazor.
/// Инжектируется в любой компонент; открывает модалку поверх всего приложения без
/// ручного управления bool-флагом видимости в каждом месте использования.
///
/// Требует, чтобы где-то в дереве (обычно в MainLayout) был размещён
/// &lt;NorthDialogProvider /&gt; — именно он физически рендерит открытые диалоги.
/// </summary>
public interface INorthDialogService
{
    /// <summary>Показывает диалог с компонентом <typeparamref name="TComponent"/> без параметров.</summary>
    Task<NorthDialogResult> ShowAsync<TComponent>(NorthDialogOptions? options = null)
        where TComponent : IComponent;

    /// <summary>Показывает диалог с компонентом <typeparamref name="TComponent"/>, передавая ему параметры (имя параметра компонента → значение).</summary>
    Task<NorthDialogResult> ShowAsync<TComponent>(IReadOnlyDictionary<string, object?> parameters, NorthDialogOptions? options = null)
        where TComponent : IComponent;

    /// <summary>Нетипизированная версия — на случай, если тип компонента известен только в рантайме.</summary>
    Task<NorthDialogResult> ShowAsync(Type componentType, IReadOnlyDictionary<string, object?>? parameters = null, NorthDialogOptions? options = null);

    /// <summary>Принудительно закрыть конкретный диалог (обычно диалог закрывает себя сам через NorthDialogReference.Close).</summary>
    void Close(NorthDialogReference reference, NorthDialogResult? result = null);
}
