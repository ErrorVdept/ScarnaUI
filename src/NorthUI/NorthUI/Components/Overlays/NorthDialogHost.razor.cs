using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NorthUI.Services;

namespace NorthUI.Components.Overlays;

public partial class NorthDialogHost : IAsyncDisposable
{
    private const string ModulePath = "./_content/NorthUI/js/north/dialog.js";

    [Parameter, EditorRequired]
    public NorthDialogReference Reference { get; set; } = default!;

    private ElementReference _panelElement;
    private DotNetObjectReference<NorthDialogHost>? _selfRef;
    private IJSObjectReference? _module;

    private Dictionary<string, object> ComponentParameters => Reference.Parameters
        .Where(p => p.Value is not null)
        .ToDictionary(p => p.Key, p => p.Value!);

    private string SizeClass => Reference.Options.Size switch
    {
        NorthDialogSize.Small => "max-w-sm",
        NorthDialogSize.Large => "max-w-3xl",
        NorthDialogSize.FullScreen => "w-full h-full max-w-none rounded-none",
        _ => "max-w-lg",
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", ModulePath);
        _selfRef = DotNetObjectReference.Create(this);
        await _module.InvokeVoidAsync("open", Reference.Id.ToString(), _panelElement, _selfRef);
    }

    /// <summary>Вызывается из dialog.js по нажатию Escape.</summary>
    [JSInvokable]
    public Task OnEscapeAsync()
    {
        if (Reference.Options.CloseOnEscapeKey)
        {
            Reference.Close(NorthDialogResult.Cancel());
        }

        return Task.CompletedTask;
    }

    private void OnBackdropClick()
    {
        if (Reference.Options.CloseOnBackdropClick)
        {
            Reference.Close(NorthDialogResult.Cancel());
        }
    }

    public async ValueTask DisposeAsync()
    {
        _selfRef?.Dispose();

        if (_module is not null)
        {
            try
            {
                await _module.InvokeVoidAsync("close", Reference.Id.ToString());
                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // circuit уже закрыт — очищать нечего
            }
        }
    }
}
