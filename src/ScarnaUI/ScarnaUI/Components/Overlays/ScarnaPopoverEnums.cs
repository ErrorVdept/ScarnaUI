namespace ScarnaUI.Components.Overlays;

/// <summary>
/// Единая позиция поповера/оверлея относительно якоря — сторона + выравнивание вдоль
/// неё одним значением ("сверху слева", "сверху", "сверху справа" и т.д., 12 позиций
/// по аналогии с floating-ui/Radix). Используется в ScarnaPopover, ScarnaTooltip,
/// ScarnaMenu, ScarnaSelect — везде, где раньше были отдельные Side/Align.
/// </summary>
public enum ScarnaPosition
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

/// <summary>Сторона якоря, у которой должен появиться поповер — внутреннее представление <see cref="ScarnaPosition"/> для JS.</summary>
public enum ScarnaPopoverSide
{
    Top,
    Bottom,
    Left,
    Right,
}

/// <summary>Выравнивание поповера вдоль второстепенной оси — внутреннее представление <see cref="ScarnaPosition"/> для JS.</summary>
public enum ScarnaPopoverAlign
{
    Start,
    Center,
    End,
}

/// <summary>Раскладывает <see cref="ScarnaPosition"/> на (сторона, выравнивание) для popover.js.</summary>
public static class ScarnaPositionExtensions
{
    public static (ScarnaPopoverSide Side, ScarnaPopoverAlign Align) ToSideAlign(this ScarnaPosition position) => position switch
    {
        ScarnaPosition.TopStart => (ScarnaPopoverSide.Top, ScarnaPopoverAlign.Start),
        ScarnaPosition.Top => (ScarnaPopoverSide.Top, ScarnaPopoverAlign.Center),
        ScarnaPosition.TopEnd => (ScarnaPopoverSide.Top, ScarnaPopoverAlign.End),

        ScarnaPosition.RightStart => (ScarnaPopoverSide.Right, ScarnaPopoverAlign.Start),
        ScarnaPosition.Right => (ScarnaPopoverSide.Right, ScarnaPopoverAlign.Center),
        ScarnaPosition.RightEnd => (ScarnaPopoverSide.Right, ScarnaPopoverAlign.End),

        ScarnaPosition.BottomStart => (ScarnaPopoverSide.Bottom, ScarnaPopoverAlign.Start),
        ScarnaPosition.Bottom => (ScarnaPopoverSide.Bottom, ScarnaPopoverAlign.Center),
        ScarnaPosition.BottomEnd => (ScarnaPopoverSide.Bottom, ScarnaPopoverAlign.End),

        ScarnaPosition.LeftStart => (ScarnaPopoverSide.Left, ScarnaPopoverAlign.Start),
        ScarnaPosition.Left => (ScarnaPopoverSide.Left, ScarnaPopoverAlign.Center),
        ScarnaPosition.LeftEnd => (ScarnaPopoverSide.Left, ScarnaPopoverAlign.End),

        _ => (ScarnaPopoverSide.Bottom, ScarnaPopoverAlign.Start),
    };
}
