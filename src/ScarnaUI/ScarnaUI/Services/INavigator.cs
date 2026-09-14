using Microsoft.AspNetCore.Components.Routing;

namespace ScarnaUI.Services;

/// <summary>
/// Обёртка над встроенным <c>NavigationManager</c> — аналог того, как в MudBlazor
/// компоненты навигации (NavLink, NavMenu) сверяются с текущим адресом и умеют перейти
/// по клику. Даёт готовые хелперы (IsActive для подсветки текущего пункта меню,
/// Refresh) поверх голого NavigateTo/Uri, которые иначе пришлось бы писать в каждом
/// компоненте навигации отдельно (ScarnaBottomMenu, ScarnaNavBar, будущий ScarnaNavMenu).
///
/// Инжектируется как обычный сервис: <c>@inject INavigator Navigator</c>.
/// </summary>
public interface INavigator
{
    /// <summary>Текущий абсолютный URI.</summary>
    string Uri { get; }

    /// <summary>Базовый URI приложения (с завершающим "/").</summary>
    string BaseUri { get; }

    /// <summary>Срабатывает при каждой смене адреса (в т.ч. по кнопкам браузера "назад"/"вперёд").</summary>
    event EventHandler<LocationChangedEventArgs>? LocationChanged;

    /// <summary>Переход на другой адрес — относительный ("/orders/5") или абсолютный.</summary>
    void NavigateTo(string uri, bool forceLoad = false, bool replace = false);

    /// <summary>Полная перезагрузка текущей страницы (аналог F5, всегда forceLoad).</summary>
    void Refresh(bool replace = false);

    /// <summary>Тот же адрес, но относительно BaseUri (без домена и ведущего "/") — удобно для сравнения в IsActive.</summary>
    string ToBaseRelativePath(string? uri = null);

    /// <summary>
    /// True, если переданный адрес совпадает с текущим — подсветка активного пункта
    /// меню/вкладки. exact=false (по умолчанию) также считает активным любой дочерний
    /// путь (как NavLinkMatch.Prefix); exact=true требует точного совпадения.
    /// </summary>
    bool IsActive(string uri, bool exact = false);
}
