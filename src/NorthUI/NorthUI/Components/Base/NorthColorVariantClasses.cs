namespace NorthUI.Components.Base;

/// <summary>
/// Общая таблица "цвет × вариант заливки" (см. <see cref="NorthColor"/>/<see cref="NorthVariant"/>),
/// переиспользуемая всеми компонентами с заливкой: NorthButton, NorthChip, NorthBadge, NorthAlert.
///
/// Классы записаны буквально (не собираются интерполяцией строк), потому что Tailwind
/// находит классы для генерации CSS статическим сканированием исходников — динамически
/// склеенная во время выполнения строка вроде $"bg-{color}" в файле не встречается
/// и в CSS не попадёт.
/// </summary>
public static class NorthColorVariantClasses
{
    /// <summary>Классы фона/текста/рамки для конкретной пары (цвет, вариант). Без размера и радиуса.</summary>
    public static string Get(NorthColor color, NorthVariant variant) => (color, variant) switch
    {
        (NorthColor.Default, NorthVariant.Solid) => "bg-default text-default-foreground hover:bg-default/80",
        (NorthColor.Default, NorthVariant.Flat) => "bg-default/50 text-default-foreground hover:bg-default/70",
        (NorthColor.Default, NorthVariant.Bordered) => "bg-transparent border border-default text-foreground hover:bg-default/20",
        (NorthColor.Default, NorthVariant.Light) => "bg-transparent text-foreground hover:bg-default/20",
        (NorthColor.Default, NorthVariant.Faded) => "bg-default/30 border border-default text-foreground hover:bg-default/50",
        (NorthColor.Default, NorthVariant.Shadow) => "bg-default text-default-foreground shadow-lg shadow-default/50 hover:bg-default/80",

        (NorthColor.Primary, NorthVariant.Solid) => "bg-primary text-primary-foreground hover:bg-primary/90",
        (NorthColor.Primary, NorthVariant.Flat) => "bg-primary/15 text-primary hover:bg-primary/25",
        (NorthColor.Primary, NorthVariant.Bordered) => "bg-transparent border border-primary text-primary hover:bg-primary/10",
        (NorthColor.Primary, NorthVariant.Light) => "bg-transparent text-primary hover:bg-primary/10",
        (NorthColor.Primary, NorthVariant.Faded) => "bg-primary/10 border border-primary/20 text-primary hover:bg-primary/20",
        (NorthColor.Primary, NorthVariant.Shadow) => "bg-primary text-primary-foreground shadow-lg shadow-primary/50 hover:bg-primary/90",

        (NorthColor.Secondary, NorthVariant.Solid) => "bg-secondary text-secondary-foreground hover:bg-secondary/90",
        (NorthColor.Secondary, NorthVariant.Flat) => "bg-secondary/15 text-secondary hover:bg-secondary/25",
        (NorthColor.Secondary, NorthVariant.Bordered) => "bg-transparent border border-secondary text-secondary hover:bg-secondary/10",
        (NorthColor.Secondary, NorthVariant.Light) => "bg-transparent text-secondary hover:bg-secondary/10",
        (NorthColor.Secondary, NorthVariant.Faded) => "bg-secondary/10 border border-secondary/20 text-secondary hover:bg-secondary/20",
        (NorthColor.Secondary, NorthVariant.Shadow) => "bg-secondary text-secondary-foreground shadow-lg shadow-secondary/50 hover:bg-secondary/90",

        (NorthColor.Success, NorthVariant.Solid) => "bg-success text-success-foreground hover:bg-success/90",
        (NorthColor.Success, NorthVariant.Flat) => "bg-success/15 text-success hover:bg-success/25",
        (NorthColor.Success, NorthVariant.Bordered) => "bg-transparent border border-success text-success hover:bg-success/10",
        (NorthColor.Success, NorthVariant.Light) => "bg-transparent text-success hover:bg-success/10",
        (NorthColor.Success, NorthVariant.Faded) => "bg-success/10 border border-success/20 text-success hover:bg-success/20",
        (NorthColor.Success, NorthVariant.Shadow) => "bg-success text-success-foreground shadow-lg shadow-success/50 hover:bg-success/90",

        (NorthColor.Warning, NorthVariant.Solid) => "bg-warning text-warning-foreground hover:bg-warning/90",
        (NorthColor.Warning, NorthVariant.Flat) => "bg-warning/15 text-warning hover:bg-warning/25",
        (NorthColor.Warning, NorthVariant.Bordered) => "bg-transparent border border-warning text-warning hover:bg-warning/10",
        (NorthColor.Warning, NorthVariant.Light) => "bg-transparent text-warning hover:bg-warning/10",
        (NorthColor.Warning, NorthVariant.Faded) => "bg-warning/10 border border-warning/20 text-warning hover:bg-warning/20",
        (NorthColor.Warning, NorthVariant.Shadow) => "bg-warning text-warning-foreground shadow-lg shadow-warning/50 hover:bg-warning/90",

        (NorthColor.Danger, NorthVariant.Solid) => "bg-danger text-danger-foreground hover:bg-danger/90",
        (NorthColor.Danger, NorthVariant.Flat) => "bg-danger/15 text-danger hover:bg-danger/25",
        (NorthColor.Danger, NorthVariant.Bordered) => "bg-transparent border border-danger text-danger hover:bg-danger/10",
        (NorthColor.Danger, NorthVariant.Light) => "bg-transparent text-danger hover:bg-danger/10",
        (NorthColor.Danger, NorthVariant.Faded) => "bg-danger/10 border border-danger/20 text-danger hover:bg-danger/20",
        (NorthColor.Danger, NorthVariant.Shadow) => "bg-danger text-danger-foreground shadow-lg shadow-danger/50 hover:bg-danger/90",

        _ => "bg-default text-default-foreground hover:bg-default/80",
    };

    /// <summary>Радиус скругления (общий для всех компонентов с заливкой).</summary>
    public static string Radius(NorthRadius radius) => radius switch
    {
        NorthRadius.None => "rounded-none",
        NorthRadius.Small => "rounded-north-sm",
        NorthRadius.Large => "rounded-north-lg",
        NorthRadius.Full => "rounded-north-full",
        _ => "rounded-north-md",
    };
}
