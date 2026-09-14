namespace ScarnaUI.Components.Theme;

/// <summary>
/// Режим темы: явный светлый/тёмный или "следовать системным настройкам ОС"
/// (<c>prefers-color-scheme</c>).
/// </summary>
public enum ScarnaThemeMode
{
    Light,
    Dark,
    System,
}

/// <summary>
/// C#-модель дизайн-токенов ScarnaUI: цветовая палитра, радиусы, шрифт.
/// Это единственный источник правды для темы — <see cref="ScarnaThemeProvider"/> превращает
/// значения этого класса в CSS custom properties (<c>--scarna-*</c>) на &lt;html&gt;,
/// а Tailwind-утилиты (bg-primary, rounded-md, ...) в компонентах уже ссылаются на эти
/// переменные. Поэтому изменение темы в рантайме (например, смена Primary-цвета из кода
/// приложения) не требует пересборки CSS — оно применяется мгновенно через JS.
/// </summary>
public sealed class ScarnaTheme
{
    /// <summary>Палитра для светлой темы.</summary>
    public ScarnaPalette Light { get; set; } = ScarnaPalette.LightDefault();

    /// <summary>Палитра для тёмной темы.</summary>
    public ScarnaPalette Dark { get; set; } = ScarnaPalette.DarkDefault();

    /// <summary>Радиусы скругления, общие для обеих тем.</summary>
    public ScarnaRadiusScale Radius { get; set; } = new();

    /// <summary>Шрифт по умолчанию для всей библиотеки (Inter — как в HeroUI).</summary>
    public string FontFamily { get; set; } =
        "Inter, ui-sans-serif, system-ui, -apple-system, \"Segoe UI\", Roboto, sans-serif";

    /// <summary>Возвращает палитру, соответствующую заданному режиму (System трактуется как Light — фактический выбор делает браузер/JS).</summary>
    public ScarnaPalette GetPalette(bool isDark) => isDark ? Dark : Light;
}

/// <summary>
/// Радиусы скругления компонентов. Значения — валидные CSS-размеры (px/rem).
/// Именно эти три ступени используются во всех компонентах через классы
/// <c>rounded-scarna-sm|md|lg</c> плюс отдельная константа "full" для пилюль/аватаров.
/// </summary>
public sealed class ScarnaRadiusScale
{
    public string Small { get; set; } = "8px";
    public string Medium { get; set; } = "12px";
    public string Large { get; set; } = "14px";
    public string Full { get; set; } = "9999px";
}

/// <summary>
/// Набор семантических цветов одной темы (светлой или тёмной).
/// Каждый цвет хранится как HSL-триплет без функции ("H S% L%", например "212 100% 47%") —
/// это позволяет Tailwind собирать <c>hsl(var(--scarna-primary) / &lt;alpha-value&gt;)</c>
/// и поддерживать модификаторы прозрачности вида <c>bg-primary/50</c>.
/// </summary>
public sealed class ScarnaPalette
{
    // Значения ниже — палитра HeroUI Theme Builder (lightness=0.6204, chroma=0.195,
    // hue=253.83: https://heroui.com/en/themes?lightness=0.6204&chroma=0.195&hue=253.83),
    // сконвертированная из OKLCH в HSL (см. tools/oklch-convert.mjs). HeroUI больше не
    // определяет Secondary в новой токен-системе — она подобрана отдельно по той же
    // методологии (тот же приглушённый нейтральный оттенок фона, один яркий акцентный тон).
    public string Background { get; set; } = "210 10% 96.1%";
    public string Foreground { get; set; } = "240 2% 9.6%";

    /// <summary>Поверхности карточек/попапов, приподнятые над фоном (аналог HeroUI surface/surface-secondary/-tertiary).</summary>
    public string Content1 { get; set; } = "0 0% 100%";
    public string Content2 { get; set; } = "240 3.2% 93.9%";
    public string Content3 { get; set; } = "240 2.4% 92%";
    public string Content4 { get; set; } = "210 3.6% 89%";

    public string Default { get; set; } = "210 5% 92.2%";
    public string DefaultForeground { get; set; } = "210 8% 9.8%";

    public string Primary { get; set; } = "208.15 96.8% 49.2%";
    public string PrimaryForeground { get; set; } = "0 0% 98.8%";

    public string Secondary { get; set; } = "276.77 58.5% 58.4%";
    public string SecondaryForeground { get; set; } = "0 0% 98.8%";

    public string Success { get; set; } = "146.33 81.1% 43.5%";
    public string SuccessForeground { get; set; } = "140 6.4% 9.2%";

    public string Warning { get; set; } = "37.14 91.3% 54.9%";
    public string WarningForeground { get; set; } = "36 10.6% 9.2%";

    public string Danger { get; set; } = "358.5 100% 60.8%";
    public string DangerForeground { get; set; } = "0 0% 98.8%";

    /// <summary>Цвет кольца фокуса (обводка при навигации с клавиатуры).</summary>
    public string Focus { get; set; } = "208.15 96.8% 49.2%";

    /// <summary>Разделительные линии (Divider, границы карточек по умолчанию).</summary>
    public string Divider { get; set; } = "240 1.9% 89.6%";

    /// <summary>Цвет подложки под модалками/дровером (используется с прозрачностью) — нейтральный чёрный, одинаковый в обеих темах: это цвет затемняющего скрима, а не "поверхности", в отличие от одноимённого токена HeroUI.</summary>
    public string Overlay { get; set; } = "0 0% 0%";

    public static ScarnaPalette LightDefault() => new();

    public static ScarnaPalette DarkDefault() => new()
    {
        Background = "180 9.1% 2.2%",
        Foreground = "210 33.3% 98.8%",

        Content1 = "220 6.1% 9.6%",
        Content2 = "210 2.9% 13.7%",
        Content3 = "210 2.6% 15.3%",
        Content4 = "210 2.1% 18.4%",

        Default = "240 1.3% 15.5%",
        DefaultForeground = "0 0% 98.8%",

        Primary = "208.15 96.8% 49.2%",
        PrimaryForeground = "0 0% 98.8%",

        Secondary = "276 69.8% 64.9%",
        SecondaryForeground = "0 0% 98.8%",

        Success = "146.33 81.1% 43.5%",
        SuccessForeground = "140 6.4% 9.2%",

        Warning = "37.01 91.3% 64.1%",
        WarningForeground = "36 10.6% 9.2%",

        Danger = "358.88 69% 54.5%",
        DangerForeground = "0 0% 98.8%",

        Focus = "208.15 96.8% 49.2%",
        Divider = "180 1.5% 13.1%",
        Overlay = "0 0% 0%",
    };
}
