namespace ScarnaUI.Components.Layout;

/// <summary>
/// Выравнивание по главной оси (justify-content) — как Justify в MudBlazor.
/// Используется в ScarnaStack (flex) и ScarnaGrid (распределение свободного места в сетке).
/// </summary>
public enum Justify
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
/// Используется в ScarnaStack (flex) и ScarnaGrid (align-items ячеек).
/// </summary>
public enum Align
{
    Start,
    End,
    Center,
    Stretch,
    Baseline,
}

/// <summary>Максимальная ширина <see cref="ScarnaContainer"/>.</summary>
public enum ScarnaContainerMaxWidth
{
    Small,
    Medium,
    Large,
    ExtraLarge,
}
