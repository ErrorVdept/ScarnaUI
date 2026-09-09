namespace NorthUI.Components.DataDisplay;

/// <summary>Режим выбора в <see cref="NorthList{TValue}"/> — как SelectionMode у MudList.</summary>
public enum NorthListSelectionMode
{
    /// <summary>Список без выбора — просто кликабельные пункты (см. NorthListItem.OnClick).</summary>
    None,

    /// <summary>Можно выбрать один пункт — привязывается через Value/ValueChanged.</summary>
    Single,

    /// <summary>Можно выбрать несколько пунктов — привязывается через SelectedValues/SelectedValuesChanged.</summary>
    Multiple,
}
