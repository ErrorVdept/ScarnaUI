using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ScarnaUI.Components.Theme;

public partial class ScarnaThemeProvider : IAsyncDisposable
{
    private const string ModulePath = "./_content/ScarnaUI/js/scarna/theme.js";

    private IJSObjectReference? _module;
    private DotNetObjectReference<ScarnaThemeProvider>? _selfReference;
    private ScarnaThemeContext _context = null!;
    private bool _isDark;
    private ScarnaThemeMode _mode = ScarnaThemeMode.System;

    /// <summary>Дерево компонентов приложения, получающее доступ к теме через каскад.</summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>Токены темы (палитры, радиусы, шрифт). По умолчанию — палитра ScarnaUI, вдохновлённая HeroUI.</summary>
    [Parameter]
    public ScarnaTheme Theme { get; set; } = new();

    /// <summary>Режим темы при первом запуске, если у пользователя ещё нет сохранённого выбора в localStorage.</summary>
    [Parameter]
    public ScarnaThemeMode DefaultMode { get; set; } = ScarnaThemeMode.System;

    protected override void OnInitialized()
    {
        _mode = DefaultMode;
        _isDark = DefaultMode == ScarnaThemeMode.Dark;
        RebuildContext();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", ModulePath);
        _selfReference = DotNetObjectReference.Create(this);

        var storedMode = await _module.InvokeAsync<string?>("getStoredMode");
        _mode = storedMode switch
        {
            "light" => ScarnaThemeMode.Light,
            "dark" => ScarnaThemeMode.Dark,
            "system" => ScarnaThemeMode.System,
            _ => DefaultMode,
        };

        await ResolveAndApplyAsync();
        await _module.InvokeVoidAsync("watchSystemTheme", _selfReference);
    }

    /// <summary>Вызывается из JS (theme.js) при смене системной темы ОС, пока активен режим System.</summary>
    [JSInvokable]
    public async Task OnSystemPreferenceChangedAsync(bool isDark)
    {
        if (_mode != ScarnaThemeMode.System)
        {
            return;
        }

        _isDark = isDark;
        RebuildContext();
        StateHasChanged();
        await ApplyCssVariablesAsync();
    }

    private async Task SetModeAsync(ScarnaThemeMode mode)
    {
        _mode = mode;
        await ResolveAndApplyAsync();

        if (_module is not null)
        {
            var stored = mode switch
            {
                ScarnaThemeMode.Light => "light",
                ScarnaThemeMode.Dark => "dark",
                _ => "system",
            };
            await _module.InvokeVoidAsync("setStoredMode", stored);
        }
    }

    private async Task ResolveAndApplyAsync()
    {
        _isDark = _mode switch
        {
            ScarnaThemeMode.Dark => true,
            ScarnaThemeMode.Light => false,
            _ => _module is not null && await _module.InvokeAsync<bool>("systemPrefersDark"),
        };

        RebuildContext();
        StateHasChanged();
        await ApplyCssVariablesAsync();
    }

    private async Task ApplyCssVariablesAsync()
    {
        if (_module is null)
        {
            return;
        }

        var palette = Theme.GetPalette(_isDark);
        var vars = new Dictionary<string, string>
        {
            ["--scarna-background"] = palette.Background,
            ["--scarna-foreground"] = palette.Foreground,
            ["--scarna-content1"] = palette.Content1,
            ["--scarna-content2"] = palette.Content2,
            ["--scarna-content3"] = palette.Content3,
            ["--scarna-content4"] = palette.Content4,
            ["--scarna-default"] = palette.Default,
            ["--scarna-default-foreground"] = palette.DefaultForeground,
            ["--scarna-primary"] = palette.Primary,
            ["--scarna-primary-foreground"] = palette.PrimaryForeground,
            ["--scarna-secondary"] = palette.Secondary,
            ["--scarna-secondary-foreground"] = palette.SecondaryForeground,
            ["--scarna-success"] = palette.Success,
            ["--scarna-success-foreground"] = palette.SuccessForeground,
            ["--scarna-warning"] = palette.Warning,
            ["--scarna-warning-foreground"] = palette.WarningForeground,
            ["--scarna-danger"] = palette.Danger,
            ["--scarna-danger-foreground"] = palette.DangerForeground,
            ["--scarna-focus"] = palette.Focus,
            ["--scarna-divider"] = palette.Divider,
            ["--scarna-overlay"] = palette.Overlay,
            ["--scarna-radius-sm"] = Theme.Radius.Small,
            ["--scarna-radius-md"] = Theme.Radius.Medium,
            ["--scarna-radius-lg"] = Theme.Radius.Large,
            ["--scarna-radius-full"] = Theme.Radius.Full,
            ["--scarna-font-sans"] = Theme.FontFamily,
        };

        await _module.InvokeVoidAsync("applyTheme", vars, _isDark);
    }

    private void RebuildContext()
    {
        _context = new ScarnaThemeContext(Theme, _isDark, _mode, SetModeAsync);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try
            {
                await _module.InvokeVoidAsync("unwatchSystemTheme");
                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // circuit уже закрыт (например, обновление страницы) — освобождать нечего
            }
        }

        // Ссылку освобождаем только после отписки в JS — иначе системная смена темы,
        // пришедшая между Dispose() и отпиской, дёрнула бы уже освобождённый объект.
        _selfReference?.Dispose();
    }
}
