using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ScarnaUI.Services;

/// <summary>
/// Реализация <see cref="INavigator"/> поверх фреймворкового <c>NavigationManager</c>
/// (регистрируется как Scoped, см. ScarnaUiServiceCollectionExtensions).
/// </summary>
internal sealed class Navigator : INavigator, IDisposable
{
    private readonly NavigationManager _navigationManager;

    public Navigator(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += OnLocationChanged;
    }

    public string Uri => _navigationManager.Uri;

    public string BaseUri => _navigationManager.BaseUri;

    public event EventHandler<LocationChangedEventArgs>? LocationChanged;

    public void NavigateTo(string uri, bool forceLoad = false, bool replace = false)
        => _navigationManager.NavigateTo(uri, new NavigationOptions { ForceLoad = forceLoad, ReplaceHistoryEntry = replace });

    public void Refresh(bool replace = false)
        => _navigationManager.NavigateTo(Uri, new NavigationOptions { ForceLoad = true, ReplaceHistoryEntry = replace });

    public string ToBaseRelativePath(string? uri = null)
        => _navigationManager.ToBaseRelativePath(uri ?? Uri);

    public bool IsActive(string uri, bool exact = false)
    {
        var absoluteTarget = _navigationManager.ToAbsoluteUri(uri).ToString();

        // Внешняя ссылка (другой хост) не принадлежит приложению — ToBaseRelativePath на ней
        // бросил бы ArgumentException прямо из рендера ScarnaNavLink и уронил бы страницу.
        if (!absoluteTarget.StartsWith(_navigationManager.BaseUri, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(absoluteTarget + "/", _navigationManager.BaseUri, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var current = StripQueryAndFragment(ToBaseRelativePath()).TrimEnd('/');
        var target = StripQueryAndFragment(ToBaseRelativePath(absoluteTarget)).TrimEnd('/');

        if (exact)
        {
            return string.Equals(current, target, StringComparison.OrdinalIgnoreCase);
        }

        return current.Equals(target, StringComparison.OrdinalIgnoreCase)
            || current.StartsWith(target + "/", StringComparison.OrdinalIgnoreCase);
    }

    // Без этого "/orders?tab=2" и "/orders#section" переставали считаться активными для
    // Href="/orders" — строка запроса/якорь попадали в сравнение пути.
    private static string StripQueryAndFragment(string relativePath)
    {
        var end = relativePath.IndexOfAny(['?', '#']);
        return end < 0 ? relativePath : relativePath[..end];
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e) => LocationChanged?.Invoke(this, e);

    public void Dispose() => _navigationManager.LocationChanged -= OnLocationChanged;
}
