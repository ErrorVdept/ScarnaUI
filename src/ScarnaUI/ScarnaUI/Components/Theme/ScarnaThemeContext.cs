namespace ScarnaUI.Components.Theme;

/// <summary>
/// Каскадный контекст темы — то, что реально получают компоненты через
/// <c>[CascadingParameter] ScarnaThemeContext? ThemeContext</c>.
/// Создаётся и обновляется единственным экземпляром <see cref="ScarnaThemeProvider"/>.
///
/// Даёт компонентам (например, переключателю темы <c>ScarnaThemeSwitch</c>) доступ
/// к текущим токенам и способ переключить светлую/тёмную тему без прямой зависимости
/// от JS-интеропа — вся работа с DOM инкапсулирована в провайдере.
/// </summary>
public sealed class ScarnaThemeContext
{
    private readonly Func<ScarnaThemeMode, Task> _setModeAsync;

    internal ScarnaThemeContext(ScarnaTheme theme, bool isDark, ScarnaThemeMode mode, Func<ScarnaThemeMode, Task> setModeAsync)
    {
        Theme = theme;
        IsDarkMode = isDark;
        Mode = mode;
        _setModeAsync = setModeAsync;
    }

    /// <summary>Активные токены темы (палитры, радиусы, шрифт).</summary>
    public ScarnaTheme Theme { get; }

    /// <summary>Палитра, применённая прямо сейчас (Light или Dark — в зависимости от IsDarkMode).</summary>
    public ScarnaPalette ActivePalette => Theme.GetPalette(IsDarkMode);

    /// <summary>True, если сейчас отображается тёмная тема (уже разрешённая, если Mode == System).</summary>
    public bool IsDarkMode { get; }

    /// <summary>Выбранный пользователем режим (может быть System — тогда IsDarkMode берётся из ОС).</summary>
    public ScarnaThemeMode Mode { get; }

    /// <summary>Переключить между Light и Dark (System трактуется как переход в противоположность текущему разрешённому значению).</summary>
    public Task ToggleDarkModeAsync() => _setModeAsync(IsDarkMode ? ScarnaThemeMode.Light : ScarnaThemeMode.Dark);

    /// <summary>Явно установить режим темы.</summary>
    public Task SetModeAsync(ScarnaThemeMode mode) => _setModeAsync(mode);
}
