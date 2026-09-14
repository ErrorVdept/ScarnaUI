using Microsoft.AspNetCore.Components;

namespace ScarnaUI.Services;

/// <summary>
/// Императивный сервис показа модальных диалогов — аналог IDialogService в MudBlazor.
/// Инжектируется в любой компонент; открывает модалку поверх всего приложения без
/// ручного управления bool-флагом видимости в каждом месте использования.
///
/// Требует, чтобы где-то в дереве (обычно в MainLayout) был размещён
/// &lt;ScarnaDialogProvider /&gt; — именно он физически рендерит открытые диалоги.
/// </summary>
public interface IScarnaDialogService
{
    /// <summary>Показывает диалог с компонентом <typeparamref name="TComponent"/> без параметров.</summary>
    Task<ScarnaDialogResult> ShowAsync<TComponent>(ScarnaDialogOptions? options = null)
        where TComponent : IComponent;

    /// <summary>Показывает диалог с компонентом <typeparamref name="TComponent"/>, передавая ему параметры (имя параметра компонента → значение).</summary>
    Task<ScarnaDialogResult> ShowAsync<TComponent>(IReadOnlyDictionary<string, object?> parameters, ScarnaDialogOptions? options = null)
        where TComponent : IComponent;

    /// <summary>Нетипизированная версия — на случай, если тип компонента известен только в рантайме.</summary>
    Task<ScarnaDialogResult> ShowAsync(Type componentType, IReadOnlyDictionary<string, object?>? parameters = null, ScarnaDialogOptions? options = null);

    /// <summary>Принудительно закрыть конкретный диалог (обычно диалог закрывает себя сам через ScarnaDialogReference.Close).</summary>
    void Close(ScarnaDialogReference reference, ScarnaDialogResult? result = null);
}
