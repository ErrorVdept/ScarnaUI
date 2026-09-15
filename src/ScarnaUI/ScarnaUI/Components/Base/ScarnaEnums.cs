namespace ScarnaUI.Components.Base;

/// <summary>
/// Семантический цвет компонента. Каждое значение соответствует набору CSS-переменных
/// вида <c>--scarna-color-{name}</c> / <c>--scarna-color-{name}-foreground</c>,
/// которые определяются в активной теме (<see cref="ScarnaUI.Theme.ScarnaTheme"/>) и могут
/// быть изменены в рантайме — см. ScarnaThemeProvider.
/// </summary>
public enum Color
{
    /// <summary>Нейтральный серый цвет — используется по умолчанию, если ничего не задано.</summary>
    Default,
    Primary,
    Secondary,
    Success,
    Warning,
    Danger,
}

/// <summary>Размер компонента. Влияет на паддинги, шрифт и высоту контрола.</summary>
public enum Size
{
    Small,
    Medium,
    Large,
}

/// <summary>
/// Радиус скругления углов компонента. По умолчанию — <see cref="Medium"/>,
/// значение берётся из активной темы (--scarna-radius-*) и может отличаться от глобального.
/// </summary>
public enum Radius
{
    None,
    Small,
    Medium,
    Large,
    Full,
}

/// <summary>
/// Плотность поля формы. <see cref="Dense"/> уменьшает высоту и внутренние отступы —
/// для плотных таблиц/панелей фильтров, где обычный размер поля съедает много места.
/// </summary>
public enum ScarnaDensity
{
    Normal,
    Dense,
}

/// <summary>
/// Визуальный вариант заливки — паттерн из HeroUI: один и тот же компонент (кнопка, чип,
/// поле ввода) можно отрисовать по-разному, не меняя его семантику.
/// </summary>
public enum Variant
{
    /// <summary>Заливка сплошным цветом, текст — контрастным (foreground).</summary>
    Solid,

    /// <summary>Полупрозрачная заливка тем же цветом (10-15% opacity).</summary>
    Flat,

    /// <summary>Прозрачный фон, тонкая цветная граница.</summary>
    Bordered,

    /// <summary>Прозрачный фон, цветной текст, без границы — самый лёгкий вариант.</summary>
    Light,

    /// <summary>Как Flat, но с приглушённым (десатурированным) фоном под цвет темы.</summary>
    Faded,

    /// <summary>Как Solid, но с цветной тенью (box-shadow) под цвет компонента.</summary>
    Shadow,
}
