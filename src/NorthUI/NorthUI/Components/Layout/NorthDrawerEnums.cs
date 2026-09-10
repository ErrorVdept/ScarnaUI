namespace NorthUI.Components.Layout;

/// <summary>С какой стороны экрана выезжает <see cref="NorthDrawer"/>.</summary>
public enum NorthDrawerAnchor
{
    Left,
    Right,
    Top,
    Bottom,
}

/// <summary>
/// Как <see cref="NorthDrawer"/> ведёт себя относительно остального содержимого страницы.
/// </summary>
public enum NorthDrawerVariant
{
    /// <summary>
    /// Классическое поведение (по умолчанию) — position:fixed поверх содержимого,
    /// с полупрозрачной подложкой, закрывается по клику вне себя. Не занимает места
    /// в потоке документа — сама панель ничего не "раздвигает".
    /// </summary>
    Temporary,

    /// <summary>
    /// Без подложки — панель занимает реальное место в потоке (flex-элемент), поэтому
    /// соседние элементы (например, AppBar/основной контент) сами раздвигаются вместе
    /// с анимацией её ширины/высоты, без ручной синхронизации отступов в коде страницы.
    /// В закрытом состоянии схлопывается до нуля.
    /// </summary>
    Persistent,

    /// <summary>
    /// Как Persistent, но в закрытом состоянии не схлопывается до нуля, а остаётся
    /// узкой полосой шириной/высотой <see cref="NorthDrawer.MiniWidth"/> (обычно под
    /// одни иконки) — аналог MudBlazor DrawerVariant.Mini.
    /// </summary>
    Mini,
}
