using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace NorthUI.Services;

/// <summary>
/// Реализация <see cref="INavigator"/> поверх фреймворкового <c>NavigationManager</c>
/// (регистрируется как Scoped, см. NorthUiServiceCollectionExtensions).
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
        var current = ToBaseRelativePath().TrimEnd('/');
        var target = ToBaseRelativePath(_navigationManager.ToAbsoluteUri(uri).ToString()).TrimEnd('/');

        if (exact)
        {
            return string.Equals(current, target, StringComparison.OrdinalIgnoreCase);
        }

        return current.Equals(target, StringComparison.OrdinalIgnoreCase)
            || current.StartsWith(target + "/", StringComparison.OrdinalIgnoreCase);
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e) => LocationChanged?.Invoke(this, e);

    public void Dispose() => _navigationManager.LocationChanged -= OnLocationChanged;
}
