using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using ScarnaUI.Components.Base;

namespace ScarnaUI.Components.DataDisplay;

/// <summary>
/// Типографический текст — аналог MudText. Чистый C#-компонент (не .razor): чтобы
/// менять HTML-тег в зависимости от <see cref="Typo"/> (h1 для Typo.H1, p для Body1
/// и т.д.) или явного <see cref="As"/>, нужен рендер с тегом, вычисляемым в рантайме —
/// в .razor-разметке имя тега всегда статическое, поэтому здесь используется
/// <see cref="RenderTreeBuilder"/> напрямую.
///
/// &lt;ScarnaText Typo="ScarnaTypo.H2"&gt;Заголовок раздела&lt;/ScarnaText&gt;
/// &lt;ScarnaText Typo="ScarnaTypo.Body2" Color="ScarnaColor.Danger"&gt;Ошибка сохранения&lt;/ScarnaText&gt;
/// </summary>
public sealed class ScarnaText : ScarnaComponentBase
{
    /// <summary>Типографический стиль — определяет и HTML-тег по умолчанию, и размер/начертание шрифта.</summary>
    [Parameter] public ScarnaTypo Typo { get; set; } = ScarnaTypo.Body1;

    /// <summary>Цвет текста. Не задан (null) — обычный `text-foreground`.</summary>
    [Parameter] public ScarnaColor? Color { get; set; }

    [Parameter] public ScarnaTextAlign Align { get; set; } = ScarnaTextAlign.Inherit;

    /// <summary>Добавляет отступ снизу (для абзацев подряд) — как GutterBottom в MudBlazor.</summary>
    [Parameter] public bool GutterBottom { get; set; }

    /// <summary>Обрезать в одну строку с многоточием при переполнении.</summary>
    [Parameter] public bool Truncate { get; set; }

    /// <summary>Переопределить HTML-тег по умолчанию для выбранного Typo (например, "span" вместо "h2").</summary>
    [Parameter] public string? As { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, As ?? DefaultTag(Typo));
        builder.AddAttribute(1, "class", Classes);

        if (!string.IsNullOrEmpty(Style))
        {
            builder.AddAttribute(2, "style", Style);
        }

        if (UserAttributes is not null)
        {
            builder.AddMultipleAttributes(3, UserAttributes);
        }

        builder.AddContent(4, ChildContent);
        builder.CloseElement();
    }

    private static string DefaultTag(ScarnaTypo typo) => typo switch
    {
        ScarnaTypo.H1 => "h1",
        ScarnaTypo.H2 => "h2",
        ScarnaTypo.H3 => "h3",
        ScarnaTypo.H4 => "h4",
        ScarnaTypo.H5 => "h5",
        ScarnaTypo.H6 => "h6",
        ScarnaTypo.Subtitle1 or ScarnaTypo.Subtitle2 => "h6",
        ScarnaTypo.Caption or ScarnaTypo.Overline or ScarnaTypo.Button => "span",
        _ => "p",
    };

    private string Classes => CssBuilder.Default("scarna-text")
        .AddClass(TypoClasses)
        .AddClass(ColorClass)
        .AddClass(AlignClass)
        .AddClass("mb-3", GutterBottom)
        .AddClass("truncate", Truncate)
        .AddClass(Class)
        .ToString();

    private string TypoClasses => Typo switch
    {
        ScarnaTypo.H1 => "text-4xl font-bold tracking-tight",
        ScarnaTypo.H2 => "text-3xl font-bold tracking-tight",
        ScarnaTypo.H3 => "text-2xl font-semibold tracking-tight",
        ScarnaTypo.H4 => "text-xl font-semibold",
        ScarnaTypo.H5 => "text-lg font-semibold",
        ScarnaTypo.H6 => "text-base font-semibold",
        ScarnaTypo.Subtitle1 => "text-base font-medium",
        ScarnaTypo.Subtitle2 => "text-sm font-medium",
        ScarnaTypo.Body2 => "text-sm",
        ScarnaTypo.Caption => "text-xs",
        ScarnaTypo.Overline => "text-xs uppercase tracking-wide",
        ScarnaTypo.Button => "text-sm font-medium uppercase tracking-wide",
        _ => "text-base",
    };

    // Цвет всегда даёт ровно один класс text-*, независимо от Typo — если бы приглушённый
    // цвет Caption/Overline лежал прямо в TypoClasses, то при явном Color="..." в разметке
    // оказалось бы два конкурирующих класса text-* сразу, и какой из них победит, зависело
    // бы от порядка правил в сгенерированном Tailwind-CSS, а не от того, что явно задал вызывающий.
    private string ColorClass => Color switch
    {
        null when Typo is ScarnaTypo.Caption or ScarnaTypo.Overline => "text-foreground/60",
        null => "text-foreground",
        ScarnaColor.Primary => "text-primary",
        ScarnaColor.Secondary => "text-secondary",
        ScarnaColor.Success => "text-success",
        ScarnaColor.Warning => "text-warning",
        ScarnaColor.Danger => "text-danger",
        ScarnaColor.Default => "text-foreground/70",
        _ => "text-foreground",
    };

    private string AlignClass => Align switch
    {
        ScarnaTextAlign.Start => "text-left",
        ScarnaTextAlign.Center => "text-center",
        ScarnaTextAlign.End => "text-right",
        ScarnaTextAlign.Justify => "text-justify",
        _ => string.Empty,
    };
}
