namespace ScarnaUI.Components.DataDisplay;

/// <summary>Режим выбора в <see cref="ScarnaList{TValue}"/> — как SelectionMode у MudList.</summary>
public enum ScarnaListSelectionMode
{
    /// <summary>Список без выбора — просто кликабельные пункты (см. ScarnaListItem.OnClick).</summary>
    None,

    /// <summary>Можно выбрать один пункт — привязывается через Value/ValueChanged.</summary>
    Single,

    /// <summary>Можно выбрать несколько пунктов — привязывается через SelectedValues/SelectedValuesChanged.</summary>
    Multiple,
}
