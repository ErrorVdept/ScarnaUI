namespace ScarnaUI.Components.Layout;

/// <summary>С какой стороны экрана выезжает <see cref="ScarnaDrawer"/>.</summary>
public enum ScarnaDrawerAnchor
{
    Left,
    Right,
    Top,
    Bottom,
}

/// <summary>
/// Как <see cref="ScarnaDrawer"/> ведёт себя относительно остального содержимого страницы.
/// </summary>
public enum ScarnaDrawerVariant
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
    /// узкой полосой шириной/высотой <see cref="ScarnaDrawer.MiniWidth"/> (обычно под
    /// одни иконки) — аналог MudBlazor DrawerVariant.Mini.
    /// </summary>
    Mini,
}
