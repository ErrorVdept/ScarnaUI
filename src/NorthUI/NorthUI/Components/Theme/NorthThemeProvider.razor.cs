using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace NorthUI.Components.Theme;

public partial class NorthThemeProvider : IAsyncDisposable
{
    private const string ModulePath = "./_content/NorthUI/js/north/theme.js";

    private IJSObjectReference? _module;
    private DotNetObjectReference<NorthThemeProvider>? _selfReference;
    private NorthThemeContext _context = null!;
    private bool _isDark;
    private NorthThemeMode _mode = NorthThemeMode.System;

    /// <summary>Дерево компонентов приложения, получающее доступ к теме через каскад.</summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>Токены темы (палитры, радиусы, шрифт). По умолчанию — палитра NorthUI, вдохновлённая HeroUI.</summary>
    [Parameter]
    public NorthTheme Theme { get; set; } = new();

    /// <summary>Режим темы при первом запуске, если у пользователя ещё нет сохранённого выбора в localStorage.</summary>
    [Parameter]
    public NorthThemeMode DefaultMode { get; set; } = NorthThemeMode.System;

    protected override void OnInitialized()
    {
        _mode = DefaultMode;
        _isDark = DefaultMode == NorthThemeMode.Dark;
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
            "light" => NorthThemeMode.Light,
            "dark" => NorthThemeMode.Dark,
            "system" => NorthThemeMode.System,
            _ => DefaultMode,
        };

        await ResolveAndApplyAsync();
        await _module.InvokeVoidAsync("watchSystemTheme", _selfReference);
    }

    /// <summary>Вызывается из JS (theme.js) при смене системной темы ОС, пока активен режим System.</summary>
    [JSInvokable]
    public async Task OnSystemPreferenceChangedAsync(bool isDark)
    {
        if (_mode != NorthThemeMode.System)
        {
            return;
        }

        _isDark = isDark;
        RebuildContext();
        StateHasChanged();
        await ApplyCssVariablesAsync();
    }

    private async Task SetModeAsync(NorthThemeMode mode)
    {
        _mode = mode;
        await ResolveAndApplyAsync();

        if (_module is not null)
        {
            var stored = mode switch
            {
                NorthThemeMode.Light => "light",
                NorthThemeMode.Dark => "dark",
                _ => "system",
            };
            await _module.InvokeVoidAsync("setStoredMode", stored);
        }
    }

    private async Task ResolveAndApplyAsync()
    {
        _isDark = _mode switch
        {
            NorthThemeMode.Dark => true,
            NorthThemeMode.Light => false,
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
            ["--north-background"] = palette.Background,
            ["--north-foreground"] = palette.Foreground,
            ["--north-content1"] = palette.Content1,
            ["--north-content2"] = palette.Content2,
            ["--north-content3"] = palette.Content3,
            ["--north-content4"] = palette.Content4,
            ["--north-default"] = palette.Default,
            ["--north-default-foreground"] = palette.DefaultForeground,
            ["--north-primary"] = palette.Primary,
            ["--north-primary-foreground"] = palette.PrimaryForeground,
            ["--north-secondary"] = palette.Secondary,
            ["--north-secondary-foreground"] = palette.SecondaryForeground,
            ["--north-success"] = palette.Success,
            ["--north-success-foreground"] = palette.SuccessForeground,
            ["--north-warning"] = palette.Warning,
            ["--north-warning-foreground"] = palette.WarningForeground,
            ["--north-danger"] = palette.Danger,
            ["--north-danger-foreground"] = palette.DangerForeground,
            ["--north-focus"] = palette.Focus,
            ["--north-divider"] = palette.Divider,
            ["--north-overlay"] = palette.Overlay,
            ["--north-radius-sm"] = Theme.Radius.Small,
            ["--north-radius-md"] = Theme.Radius.Medium,
            ["--north-radius-lg"] = Theme.Radius.Large,
            ["--north-radius-full"] = Theme.Radius.Full,
            ["--north-font-sans"] = Theme.FontFamily,
        };

        await _module.InvokeVoidAsync("applyTheme", vars, _isDark);
    }

    private void RebuildContext()
    {
        _context = new NorthThemeContext(Theme, _isDark, _mode, SetModeAsync);
    }

    public async ValueTask DisposeAsync()
    {
        _selfReference?.Dispose();

        if (_module is not null)
        {
            try
            {
                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // circuit уже закрыт (например, обновление страницы) — освобождать нечего
            }
        }
    }
}
