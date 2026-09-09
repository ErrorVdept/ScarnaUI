using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using NorthUI.Components.Base;

namespace NorthUI.Components.DataDisplay;

/// <summary>
/// Типографический текст — аналог MudText. Чистый C#-компонент (не .razor): чтобы
/// менять HTML-тег в зависимости от <see cref="Typo"/> (h1 для Typo.H1, p для Body1
/// и т.д.) или явного <see cref="As"/>, нужен рендер с тегом, вычисляемым в рантайме —
/// в .razor-разметке имя тега всегда статическое, поэтому здесь используется
/// <see cref="RenderTreeBuilder"/> напрямую.
///
/// &lt;NorthText Typo="NorthTypo.H2"&gt;Заголовок раздела&lt;/NorthText&gt;
/// &lt;NorthText Typo="NorthTypo.Body2" Color="NorthColor.Danger"&gt;Ошибка сохранения&lt;/NorthText&gt;
/// </summary>
public sealed class NorthText : NorthComponentBase
{
    /// <summary>Типографический стиль — определяет и HTML-тег по умолчанию, и размер/начертание шрифта.</summary>
    [Parameter] public NorthTypo Typo { get; set; } = NorthTypo.Body1;

    /// <summary>Цвет текста. Не задан (null) — обычный `text-foreground`.</summary>
    [Parameter] public NorthColor? Color { get; set; }

    [Parameter] public NorthTextAlign Align { get; set; } = NorthTextAlign.Inherit;

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

    private static string DefaultTag(NorthTypo typo) => typo switch
    {
        NorthTypo.H1 => "h1",
        NorthTypo.H2 => "h2",
        NorthTypo.H3 => "h3",
        NorthTypo.H4 => "h4",
        NorthTypo.H5 => "h5",
        NorthTypo.H6 => "h6",
        NorthTypo.Subtitle1 or NorthTypo.Subtitle2 => "h6",
        NorthTypo.Caption or NorthTypo.Overline or NorthTypo.Button => "span",
        _ => "p",
    };

    private string Classes => CssBuilder.Default("north-text")
        .AddClass(TypoClasses)
        .AddClass(ColorClass)
        .AddClass(AlignClass)
        .AddClass("mb-3", GutterBottom)
        .AddClass("truncate", Truncate)
        .AddClass(Class)
        .ToString();

    private string TypoClasses => Typo switch
    {
        NorthTypo.H1 => "text-4xl font-bold tracking-tight",
        NorthTypo.H2 => "text-3xl font-bold tracking-tight",
        NorthTypo.H3 => "text-2xl font-semibold tracking-tight",
        NorthTypo.H4 => "text-xl font-semibold",
        NorthTypo.H5 => "text-lg font-semibold",
        NorthTypo.H6 => "text-base font-semibold",
        NorthTypo.Subtitle1 => "text-base font-medium",
        NorthTypo.Subtitle2 => "text-sm font-medium",
        NorthTypo.Body2 => "text-sm",
        NorthTypo.Caption => "text-xs",
        NorthTypo.Overline => "text-xs uppercase tracking-wide",
        NorthTypo.Button => "text-sm font-medium uppercase tracking-wide",
        _ => "text-base",
    };

    // Цвет всегда даёт ровно один класс text-*, независимо от Typo — если бы приглушённый
    // цвет Caption/Overline лежал прямо в TypoClasses, то при явном Color="..." в разметке
    // оказалось бы два конкурирующих класса text-* сразу, и какой из них победит, зависело
    // бы от порядка правил в сгенерированном Tailwind-CSS, а не от того, что явно задал вызывающий.
    private string ColorClass => Color switch
    {
        null when Typo is NorthTypo.Caption or NorthTypo.Overline => "text-foreground/60",
        null => "text-foreground",
        NorthColor.Primary => "text-primary",
        NorthColor.Secondary => "text-secondary",
        NorthColor.Success => "text-success",
        NorthColor.Warning => "text-warning",
        NorthColor.Danger => "text-danger",
        NorthColor.Default => "text-foreground/70",
        _ => "text-foreground",
    };

    private string AlignClass => Align switch
    {
        NorthTextAlign.Start => "text-left",
        NorthTextAlign.Center => "text-center",
        NorthTextAlign.End => "text-right",
        NorthTextAlign.Justify => "text-justify",
        _ => string.Empty,
    };
}
