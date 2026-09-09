namespace NorthUI.Components.Theme;

/// <summary>
/// Каскадный контекст темы — то, что реально получают компоненты через
/// <c>[CascadingParameter] NorthThemeContext? ThemeContext</c>.
/// Создаётся и обновляется единственным экземпляром <see cref="NorthThemeProvider"/>.
///
/// Даёт компонентам (например, переключателю темы <c>NorthThemeSwitch</c>) доступ
/// к текущим токенам и способ переключить светлую/тёмную тему без прямой зависимости
/// от JS-интеропа — вся работа с DOM инкапсулирована в провайдере.
/// </summary>
public sealed class NorthThemeContext
{
    private readonly Func<NorthThemeMode, Task> _setModeAsync;

    internal NorthThemeContext(NorthTheme theme, bool isDark, NorthThemeMode mode, Func<NorthThemeMode, Task> setModeAsync)
    {
        Theme = theme;
        IsDarkMode = isDark;
        Mode = mode;
        _setModeAsync = setModeAsync;
    }

    /// <summary>Активные токены темы (палитры, радиусы, шрифт).</summary>
    public NorthTheme Theme { get; }

    /// <summary>Палитра, применённая прямо сейчас (Light или Dark — в зависимости от IsDarkMode).</summary>
    public NorthPalette ActivePalette => Theme.GetPalette(IsDarkMode);

    /// <summary>True, если сейчас отображается тёмная тема (уже разрешённая, если Mode == System).</summary>
    public bool IsDarkMode { get; }

    /// <summary>Выбранный пользователем режим (может быть System — тогда IsDarkMode берётся из ОС).</summary>
    public NorthThemeMode Mode { get; }

    /// <summary>Переключить между Light и Dark (System трактуется как переход в противоположность текущему разрешённому значению).</summary>
    public Task ToggleDarkModeAsync() => _setModeAsync(IsDarkMode ? NorthThemeMode.Light : NorthThemeMode.Dark);

    /// <summary>Явно установить режим темы.</summary>
    public Task SetModeAsync(NorthThemeMode mode) => _setModeAsync(mode);
}
