namespace ScarnaUI.Components.Base;

/// <summary>
/// Общая таблица "цвет × вариант заливки" (см. <see cref="ScarnaColor"/>/<see cref="ScarnaVariant"/>),
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
    public static string Get(ScarnaColor color, ScarnaVariant variant) => (color, variant) switch
    {
        (ScarnaColor.Default, ScarnaVariant.Solid) => "bg-default text-default-foreground hover:bg-default/80",
        (ScarnaColor.Default, ScarnaVariant.Flat) => "bg-default/50 text-default-foreground hover:bg-default/70",
        (ScarnaColor.Default, ScarnaVariant.Bordered) => "bg-transparent border border-default text-foreground hover:bg-default/20",
        (ScarnaColor.Default, ScarnaVariant.Light) => "bg-transparent text-foreground hover:bg-default/20",
        (ScarnaColor.Default, ScarnaVariant.Faded) => "bg-default/30 border border-default text-foreground hover:bg-default/50",
        (ScarnaColor.Default, ScarnaVariant.Shadow) => "bg-default text-default-foreground shadow-lg shadow-default/50 hover:bg-default/80",

        (ScarnaColor.Primary, ScarnaVariant.Solid) => "bg-primary text-primary-foreground hover:bg-primary/90",
        (ScarnaColor.Primary, ScarnaVariant.Flat) => "bg-primary/15 text-primary hover:bg-primary/25",
        (ScarnaColor.Primary, ScarnaVariant.Bordered) => "bg-transparent border border-primary text-primary hover:bg-primary/10",
        (ScarnaColor.Primary, ScarnaVariant.Light) => "bg-transparent text-primary hover:bg-primary/10",
        (ScarnaColor.Primary, ScarnaVariant.Faded) => "bg-primary/10 border border-primary/20 text-primary hover:bg-primary/20",
        (ScarnaColor.Primary, ScarnaVariant.Shadow) => "bg-primary text-primary-foreground shadow-lg shadow-primary/50 hover:bg-primary/90",

        (ScarnaColor.Secondary, ScarnaVariant.Solid) => "bg-secondary text-secondary-foreground hover:bg-secondary/90",
        (ScarnaColor.Secondary, ScarnaVariant.Flat) => "bg-secondary/15 text-secondary hover:bg-secondary/25",
        (ScarnaColor.Secondary, ScarnaVariant.Bordered) => "bg-transparent border border-secondary text-secondary hover:bg-secondary/10",
        (ScarnaColor.Secondary, ScarnaVariant.Light) => "bg-transparent text-secondary hover:bg-secondary/10",
        (ScarnaColor.Secondary, ScarnaVariant.Faded) => "bg-secondary/10 border border-secondary/20 text-secondary hover:bg-secondary/20",
        (ScarnaColor.Secondary, ScarnaVariant.Shadow) => "bg-secondary text-secondary-foreground shadow-lg shadow-secondary/50 hover:bg-secondary/90",

        (ScarnaColor.Success, ScarnaVariant.Solid) => "bg-success text-success-foreground hover:bg-success/90",
        (ScarnaColor.Success, ScarnaVariant.Flat) => "bg-success/15 text-success hover:bg-success/25",
        (ScarnaColor.Success, ScarnaVariant.Bordered) => "bg-transparent border border-success text-success hover:bg-success/10",
        (ScarnaColor.Success, ScarnaVariant.Light) => "bg-transparent text-success hover:bg-success/10",
        (ScarnaColor.Success, ScarnaVariant.Faded) => "bg-success/10 border border-success/20 text-success hover:bg-success/20",
        (ScarnaColor.Success, ScarnaVariant.Shadow) => "bg-success text-success-foreground shadow-lg shadow-success/50 hover:bg-success/90",

        (ScarnaColor.Warning, ScarnaVariant.Solid) => "bg-warning text-warning-foreground hover:bg-warning/90",
        (ScarnaColor.Warning, ScarnaVariant.Flat) => "bg-warning/15 text-warning hover:bg-warning/25",
        (ScarnaColor.Warning, ScarnaVariant.Bordered) => "bg-transparent border border-warning text-warning hover:bg-warning/10",
        (ScarnaColor.Warning, ScarnaVariant.Light) => "bg-transparent text-warning hover:bg-warning/10",
        (ScarnaColor.Warning, ScarnaVariant.Faded) => "bg-warning/10 border border-warning/20 text-warning hover:bg-warning/20",
        (ScarnaColor.Warning, ScarnaVariant.Shadow) => "bg-warning text-warning-foreground shadow-lg shadow-warning/50 hover:bg-warning/90",

        (ScarnaColor.Danger, ScarnaVariant.Solid) => "bg-danger text-danger-foreground hover:bg-danger/90",
        (ScarnaColor.Danger, ScarnaVariant.Flat) => "bg-danger/15 text-danger hover:bg-danger/25",
        (ScarnaColor.Danger, ScarnaVariant.Bordered) => "bg-transparent border border-danger text-danger hover:bg-danger/10",
        (ScarnaColor.Danger, ScarnaVariant.Light) => "bg-transparent text-danger hover:bg-danger/10",
        (ScarnaColor.Danger, ScarnaVariant.Faded) => "bg-danger/10 border border-danger/20 text-danger hover:bg-danger/20",
        (ScarnaColor.Danger, ScarnaVariant.Shadow) => "bg-danger text-danger-foreground shadow-lg shadow-danger/50 hover:bg-danger/90",

        _ => "bg-default text-default-foreground hover:bg-default/80",
    };

    /// <summary>Радиус скругления (общий для всех компонентов с заливкой).</summary>
    public static string Radius(ScarnaRadius radius) => radius switch
    {
        ScarnaRadius.None => "rounded-none",
        ScarnaRadius.Small => "rounded-scarna-sm",
        ScarnaRadius.Large => "rounded-scarna-lg",
        ScarnaRadius.Full => "rounded-scarna-full",
        _ => "rounded-scarna-md",
    };
}
