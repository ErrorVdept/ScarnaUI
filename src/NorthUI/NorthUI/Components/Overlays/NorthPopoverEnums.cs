namespace NorthUI.Components.Overlays;

/// <summary>
/// Единая позиция поповера/оверлея относительно якоря — сторона + выравнивание вдоль
/// неё одним значением ("сверху слева", "сверху", "сверху справа" и т.д., 12 позиций
/// по аналогии с floating-ui/Radix). Используется в NorthPopover, NorthTooltip,
/// NorthMenu, NorthSelect — везде, где раньше были отдельные Side/Align.
/// </summary>
public enum NorthPosition
{
    TopStart,
    Top,
    TopEnd,

    RightStart,
    Right,
    RightEnd,

    BottomStart,
    Bottom,
    BottomEnd,

    LeftStart,
    Left,
    LeftEnd,
}

/// <summary>Сторона якоря, у которой должен появиться поповер — внутреннее представление <see cref="NorthPosition"/> для JS.</summary>
public enum NorthPopoverSide
{
    Top,
    Bottom,
    Left,
    Right,
}

/// <summary>Выравнивание поповера вдоль второстепенной оси — внутреннее представление <see cref="NorthPosition"/> для JS.</summary>
public enum NorthPopoverAlign
{
    Start,
    Center,
    End,
}

/// <summary>Раскладывает <see cref="NorthPosition"/> на (сторона, выравнивание) для popover.js.</summary>
public static class NorthPositionExtensions
{
    public static (NorthPopoverSide Side, NorthPopoverAlign Align) ToSideAlign(this NorthPosition position) => position switch
    {
        NorthPosition.TopStart => (NorthPopoverSide.Top, NorthPopoverAlign.Start),
        NorthPosition.Top => (NorthPopoverSide.Top, NorthPopoverAlign.Center),
        NorthPosition.TopEnd => (NorthPopoverSide.Top, NorthPopoverAlign.End),

        NorthPosition.RightStart => (NorthPopoverSide.Right, NorthPopoverAlign.Start),
        NorthPosition.Right => (NorthPopoverSide.Right, NorthPopoverAlign.Center),
        NorthPosition.RightEnd => (NorthPopoverSide.Right, NorthPopoverAlign.End),

        NorthPosition.BottomStart => (NorthPopoverSide.Bottom, NorthPopoverAlign.Start),
        NorthPosition.Bottom => (NorthPopoverSide.Bottom, NorthPopoverAlign.Center),
        NorthPosition.BottomEnd => (NorthPopoverSide.Bottom, NorthPopoverAlign.End),

        NorthPosition.LeftStart => (NorthPopoverSide.Left, NorthPopoverAlign.Start),
        NorthPosition.Left => (NorthPopoverSide.Left, NorthPopoverAlign.Center),
        NorthPosition.LeftEnd => (NorthPopoverSide.Left, NorthPopoverAlign.End),

        _ => (NorthPopoverSide.Bottom, NorthPopoverAlign.Start),
    };
}
