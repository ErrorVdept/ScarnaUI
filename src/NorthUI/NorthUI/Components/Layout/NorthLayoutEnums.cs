namespace NorthUI.Components.Layout;

/// <summary>
/// Выравнивание по главной оси (justify-content) — как Justify в MudBlazor.
/// Используется в NorthStack (flex) и NorthGrid (распределение свободного места в сетке).
/// </summary>
public enum NorthJustify
{
    Start,
    End,
    Center,
    SpaceBetween,
    SpaceAround,
    SpaceEvenly,
}

/// <summary>
/// Выравнивание по поперечной оси (align-items) — как AlignItems в MudBlazor.
/// Используется в NorthStack (flex) и NorthGrid (align-items ячеек).
/// </summary>
public enum NorthAlign
{
    Start,
    End,
    Center,
    Stretch,
    Baseline,
}

/// <summary>Максимальная ширина <see cref="NorthContainer"/>.</summary>
public enum NorthContainerMaxWidth
{
    Small,
    Medium,
    Large,
    ExtraLarge,
}
