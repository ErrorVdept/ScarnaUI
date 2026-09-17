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
/// &lt;ScarnaText Typo="Typo.H2"&gt;Заголовок раздела&lt;/ScarnaText&gt;
/// &lt;ScarnaText Typo="Typo.Body2" Color="Color.Danger"&gt;Ошибка сохранения&lt;/ScarnaText&gt;
/// </summary>
public sealed class ScarnaText : ScarnaComponentBase
{
    /// <summary>Типографический стиль — определяет и HTML-тег по умолчанию, и размер/начертание шрифта.</summary>
    [Parameter] public Typo Typo { get; set; } = Typo.Body1;

    /// <summary>Цвет текста. Не задан (null) — обычный `text-foreground`.</summary>
    [Parameter] public Color? Color { get; set; }

    [Parameter] public TextAlign TextAlign { get; set; } = TextAlign.Inherit;

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

        var style = RootStyle;
        if (!string.IsNullOrEmpty(style))
        {
            builder.AddAttribute(2, "style", style);
        }

        if (UserAttributes is not null)
        {
            builder.AddMultipleAttributes(3, UserAttributes);
        }

        builder.AddContent(4, ChildContent);
        builder.CloseElement();
    }

    private static string DefaultTag(Typo typo) => typo switch
    {
        Typo.H1 => "h1",
        Typo.H2 => "h2",
        Typo.H3 => "h3",
        Typo.H4 => "h4",
        Typo.H5 => "h5",
        Typo.H6 => "h6",
        Typo.Subtitle1 or Typo.Subtitle2 => "h6",
        Typo.Caption or Typo.Overline or Typo.Button => "span",
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
        Typo.H1 => "text-4xl font-bold tracking-tight",
        Typo.H2 => "text-3xl font-bold tracking-tight",
        Typo.H3 => "text-2xl font-semibold tracking-tight",
        Typo.H4 => "text-xl font-semibold",
        Typo.H5 => "text-lg font-semibold",
        Typo.H6 => "text-base font-semibold",
        Typo.Subtitle1 => "text-base font-medium",
        Typo.Subtitle2 => "text-sm font-medium",
        Typo.Body2 => "text-sm",
        Typo.Caption => "text-xs",
        Typo.Overline => "text-xs uppercase tracking-wide",
        Typo.Button => "text-sm font-medium uppercase tracking-wide",
        _ => "text-base",
    };

    // Цвет всегда даёт ровно один класс text-*, независимо от Typo — если бы приглушённый
    // цвет Caption/Overline лежал прямо в TypoClasses, то при явном Color="..." в разметке
    // оказалось бы два конкурирующих класса text-* сразу, и какой из них победит, зависело
    // бы от порядка правил в сгенерированном Tailwind-CSS, а не от того, что явно задал вызывающий.
    private string ColorClass => Color switch
    {
        null when Typo is Typo.Caption or Typo.Overline => "text-foreground/60",
        null => "text-foreground",
        ScarnaUI.Components.Base.Color.Primary => "text-primary",
        ScarnaUI.Components.Base.Color.Secondary => "text-secondary",
        ScarnaUI.Components.Base.Color.Success => "text-success",
        ScarnaUI.Components.Base.Color.Warning => "text-warning",
        ScarnaUI.Components.Base.Color.Danger => "text-danger",
        ScarnaUI.Components.Base.Color.Default => "text-foreground/70",
        _ => "text-foreground",
    };

    private string AlignClass => TextAlign switch
    {
        TextAlign.Start => "text-left",
        TextAlign.Center => "text-center",
        TextAlign.End => "text-right",
        TextAlign.Justify => "text-justify",
        _ => string.Empty,
    };
}
