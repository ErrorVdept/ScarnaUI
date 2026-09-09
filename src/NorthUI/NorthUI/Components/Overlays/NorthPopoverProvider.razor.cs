using Microsoft.AspNetCore.Components;

namespace NorthUI.Components.Overlays;

public partial class NorthPopoverProvider
{
    private readonly Dictionary<Guid, RenderFragment> _surfaces = new();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>Регистрирует (или обновляет) содержимое поповера — вызывается из <see cref="NorthPopover"/>.</summary>
    internal void Register(Guid id, RenderFragment fragment)
    {
        _surfaces[id] = fragment;
        StateHasChanged();
    }

    /// <summary>Убирает поповер из портала — вызывается при закрытии/уничтожении <see cref="NorthPopover"/>.</summary>
    internal void Unregister(Guid id)
    {
        if (_surfaces.Remove(id))
        {
            StateHasChanged();
        }
    }
}
