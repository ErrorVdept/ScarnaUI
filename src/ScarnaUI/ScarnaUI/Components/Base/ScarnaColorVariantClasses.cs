namespace ScarnaUI.Components.Base;

/// <summary>
/// Общая таблица "цвет × вариант заливки" (см. <see cref="Color"/>/<see cref="Variant"/>),
/// переиспользуемая всеми компонентами с заливкой: ScarnaButton, ScarnaChip, ScarnaBadge, ScarnaAlert.
///
/// Классы записаны буквально (не собираются интерполяцией строк), потому что Tailwind
/// находит классы для генерации CSS статическим сканированием исходников — динамически
/// склеенная во время выполнения строка вроде $"bg-{color}" в файле не встречается
/// и в CSS не попадёт.
/// </summary>
public static class ScarnaColorVariantClasses
{
    /// <summary>Классы фона/текста/рамки для конкретной пары (цвет, вариант). Без размера и радиуса.</summary>
    public static string Get(Color color, Variant variant) => (color, variant) switch
    {
        (Color.Default, Variant.Solid) => "bg-default text-default-foreground hover:bg-default/80",
        (Color.Default, Variant.Flat) => "bg-default/50 text-default-foreground hover:bg-default/70",
        (Color.Default, Variant.Bordered) => "bg-transparent border border-default text-foreground hover:bg-default/20",
        (Color.Default, Variant.Light) => "bg-transparent text-foreground hover:bg-default/20",
        (Color.Default, Variant.Faded) => "bg-default/30 border border-default text-foreground hover:bg-default/50",
        (Color.Default, Variant.Shadow) => "bg-default text-default-foreground shadow-lg shadow-default/50 hover:bg-default/80",

        (Color.Primary, Variant.Solid) => "bg-primary text-primary-foreground hover:bg-primary/90",
        (Color.Primary, Variant.Flat) => "bg-primary/15 text-primary hover:bg-primary/25",
        (Color.Primary, Variant.Bordered) => "bg-transparent border border-primary text-primary hover:bg-primary/10",
        (Color.Primary, Variant.Light) => "bg-transparent text-primary hover:bg-primary/10",
        (Color.Primary, Variant.Faded) => "bg-primary/10 border border-primary/20 text-primary hover:bg-primary/20",
        (Color.Primary, Variant.Shadow) => "bg-primary text-primary-foreground shadow-lg shadow-primary/50 hover:bg-primary/90",

        (Color.Secondary, Variant.Solid) => "bg-secondary text-secondary-foreground hover:bg-secondary/90",
        (Color.Secondary, Variant.Flat) => "bg-secondary/15 text-secondary hover:bg-secondary/25",
        (Color.Secondary, Variant.Bordered) => "bg-transparent border border-secondary text-secondary hover:bg-secondary/10",
        (Color.Secondary, Variant.Light) => "bg-transparent text-secondary hover:bg-secondary/10",
        (Color.Secondary, Variant.Faded) => "bg-secondary/10 border border-secondary/20 text-secondary hover:bg-secondary/20",
        (Color.Secondary, Variant.Shadow) => "bg-secondary text-secondary-foreground shadow-lg shadow-secondary/50 hover:bg-secondary/90",

        (Color.Success, Variant.Solid) => "bg-success text-success-foreground hover:bg-success/90",
        (Color.Success, Variant.Flat) => "bg-success/15 text-success hover:bg-success/25",
        (Color.Success, Variant.Bordered) => "bg-transparent border border-success text-success hover:bg-success/10",
        (Color.Success, Variant.Light) => "bg-transparent text-success hover:bg-success/10",
        (Color.Success, Variant.Faded) => "bg-success/10 border border-success/20 text-success hover:bg-success/20",
        (Color.Success, Variant.Shadow) => "bg-success text-success-foreground shadow-lg shadow-success/50 hover:bg-success/90",

        (Color.Warning, Variant.Solid) => "bg-warning text-warning-foreground hover:bg-warning/90",
        (Color.Warning, Variant.Flat) => "bg-warning/15 text-warning hover:bg-warning/25",
        (Color.Warning, Variant.Bordered) => "bg-transparent border border-warning text-warning hover:bg-warning/10",
        (Color.Warning, Variant.Light) => "bg-transparent text-warning hover:bg-warning/10",
        (Color.Warning, Variant.Faded) => "bg-warning/10 border border-warning/20 text-warning hover:bg-warning/20",
        (Color.Warning, Variant.Shadow) => "bg-warning text-warning-foreground shadow-lg shadow-warning/50 hover:bg-warning/90",

        (Color.Danger, Variant.Solid) => "bg-danger text-danger-foreground hover:bg-danger/90",
        (Color.Danger, Variant.Flat) => "bg-danger/15 text-danger hover:bg-danger/25",
        (Color.Danger, Variant.Bordered) => "bg-transparent border border-danger text-danger hover:bg-danger/10",
        (Color.Danger, Variant.Light) => "bg-transparent text-danger hover:bg-danger/10",
        (Color.Danger, Variant.Faded) => "bg-danger/10 border border-danger/20 text-danger hover:bg-danger/20",
        (Color.Danger, Variant.Shadow) => "bg-danger text-danger-foreground shadow-lg shadow-danger/50 hover:bg-danger/90",

        _ => "bg-default text-default-foreground hover:bg-default/80",
    };

    /// <summary>Радиус скругления (общий для всех компонентов с заливкой).</summary>
    public static string Radius(ScarnaUI.Components.Base.Radius radius) => radius switch
    {
        ScarnaUI.Components.Base.Radius.None => "rounded-none",
        ScarnaUI.Components.Base.Radius.Small => "rounded-scarna-sm",
        ScarnaUI.Components.Base.Radius.Large => "rounded-scarna-lg",
        ScarnaUI.Components.Base.Radius.Full => "rounded-scarna-full",
        _ => "rounded-scarna-md",
    };

    /// <summary>
    /// Фон "поверхности" по цвету — для ScarnaList/ScarnaCard/ScarnaPaper/ScarnaBottomMenu/
    /// ScarnaTable/ScarnaExpansionPanel и т.п. (не для Button/Chip — см. Get выше, у них
    /// своя семантика "заливка × вариант"). В отличие от Get, Color.Default здесь — это
    /// НЕЙТРАЛЬНАЯ ПОВЕРХНОСТЬ ТЕМЫ (<paramref name="defaultClass"/>, обычно "bg-content1"),
    /// а не "bg-default" — так сохраняется исходный вид компонентов, у которых Color
    /// добавился уже после того, как их фон был захардкожен как bg-content1.
    /// </summary>
    public static string Surface(Color color, string defaultClass = "bg-content1") => color switch
    {
        Color.Transparent => "bg-transparent",
        Color.Primary => "bg-primary text-primary-foreground",
        Color.Secondary => "bg-secondary text-secondary-foreground",
        Color.Success => "bg-success text-success-foreground",
        Color.Warning => "bg-warning text-warning-foreground",
        Color.Danger => "bg-danger text-danger-foreground",
        _ => defaultClass,
    };
}
