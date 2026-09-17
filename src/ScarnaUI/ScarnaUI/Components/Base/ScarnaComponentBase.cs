using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace ScarnaUI.Components.Base;

/// <summary>
/// Базовый класс для всех компонентов ScarnaUI.
/// Даёт единый способ прокинуть <c>class</c>, <c>style</c> (в т.ч. Width/Height/Min*/Max*
/// как отдельные типизированные параметры) и любые произвольные HTML-атрибуты
/// (data-*, aria-*, onclick и т.д.) на корневой элемент компонента — ровно так же,
/// как это устроено в MudBlazor.
///
/// Как использовать в наследнике:
/// <code>
/// &lt;div class="@Classes" style="@RootStyle" @attributes="UserAttributes"&gt;...&lt;/div&gt;
///
/// @code {
///     protected string Classes =&gt; CssBuilder.Default("Scarna-card")
///         .AddClass("Scarna-card--bordered", Bordered)
///         .AddClass(Class)
///         .ToString();
/// }
/// </code>
///
/// Если у наследника уже есть собственный вычисляемый inline style (градиент заливки,
/// анимируемая ширина и т.п.), не собирайте его вручную вместе со Style — передайте
/// его в <see cref="BuildStyle"/>, чтобы Width/Height/Min*/Max*/Style тоже попали
/// в итоговую строку: <c>style="@BuildStyle(myOwnStyle)"</c>.
///
/// Отступы (padding/margin) сюда намеренно не входят — регулируются CSS-классами
/// (Tailwind px-*/py-*/mx-*/my-* "из коробки", плюс фирменные pa-*/ma-*/ma-n*/mx-n*/my-n*
/// в стиле MudBlazor, см. docmd/custom-classes.md), а не отдельными typed-параметрами:
/// одно и то же значение как класс, а не inline style, можно переопределить в media-query/
/// варианте (sm:pa-6, hover:ma-2) и т.п. — то, чего inline style не умеет вовсе.
/// </summary>
public abstract class ScarnaComponentBase : ComponentBase
{
    /// <summary>Дополнительные CSS-классы, добавляемые к классам компонента (не заменяют их).</summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>Инлайн-стили, добавляемые к корневому элементу компонента.</summary>
    [Parameter]
    public string? Style { get; set; }

    // Все шесть — virtual: Blazor не поддерживает скрытие параметров (new) — компонент,
    // где нужен параметр с тем же именем, но другим типом/умолчанием (ScarnaDrawer.Width,
    // ScarnaLayout.MinHeight), обязан использовать override, а не new, иначе рантайм
    // выбрасывает "declares more than one parameter matching the name" (в отличие от
    // обычного C#, где new просто прошло бы компиляцию молча).

    /// <summary>CSS width корневого элемента (например, "240px", "50%", "20rem").</summary>
    [Parameter]
    public virtual string? Width { get; set; }

    /// <summary>CSS height корневого элемента.</summary>
    [Parameter]
    public virtual string? Height { get; set; }

    /// <summary>CSS min-width корневого элемента.</summary>
    [Parameter]
    public virtual string? MinWidth { get; set; }

    /// <summary>CSS min-height корневого элемента.</summary>
    [Parameter]
    public virtual string? MinHeight { get; set; }

    /// <summary>CSS max-width корневого элемента.</summary>
    [Parameter]
    public virtual string? MaxWidth { get; set; }

    /// <summary>CSS max-height корневого элемента.</summary>
    [Parameter]
    public virtual string? MaxHeight { get; set; }

    /// <summary>
    /// Любые атрибуты, не описанные как параметры компонента явно (data-*, aria-*, id,
    /// обработчики событий и т.п.), прозрачно прокидываются на корневой HTML-элемент.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? UserAttributes { get; set; }

    /// <summary>
    /// Итоговый inline style корневого элемента: Width/Height/MinWidth/MinHeight/
    /// MaxWidth/MaxHeight, затем Style — он идёт последним, поэтому может
    /// переопределить любое из них.
    /// </summary>
    protected string? RootStyle => BuildStyle();

    /// <summary>
    /// Как <see cref="RootStyle"/>, но с возможностью подмешать собственный вычисляемый
    /// CSS наследника (идёт после размерных параметров, но до пользовательского Style).
    ///
    /// includeWidthHeight="false" — для наследников, которые сами override'ят Width/Height
    /// под собственную логику (например, ScarnaDrawer: Width/Height — это размер раскрытой
    /// панели, а не "просто CSS width/height корня", и применяются то к width, то к height
    /// в зависимости от Anchor). Иначе они и так уже посчитаны в ownStyle, но раз
    /// override — не new (см. выше), Width/Height остаются виртуально доступны и здесь,
    /// и без этого флага попали бы в итоговый style ДВАЖДЫ, а для "невыбранной" оси —
    /// ещё и лишним, ничем не обусловленным ограничением (100% высота панели схлопнулась
    /// бы до значения Height, хотя Anchor=Left использует только Width).
    /// </summary>
    protected string? BuildStyle(string? ownStyle = null, bool includeWidthHeight = true)
    {
        var hasAny = (includeWidthHeight && (!string.IsNullOrWhiteSpace(Width) || !string.IsNullOrWhiteSpace(Height)))
            || !string.IsNullOrWhiteSpace(MinWidth) || !string.IsNullOrWhiteSpace(MinHeight)
            || !string.IsNullOrWhiteSpace(MaxWidth) || !string.IsNullOrWhiteSpace(MaxHeight)
            || !string.IsNullOrWhiteSpace(ownStyle) || !string.IsNullOrWhiteSpace(Style);

        if (!hasAny)
        {
            return null;
        }

        var parts = new List<string>(6);
        if (includeWidthHeight)
        {
            AddIfSet(parts, "width", Width);
            AddIfSet(parts, "height", Height);
        }

        AddIfSet(parts, "min-width", MinWidth);
        AddIfSet(parts, "min-height", MinHeight);
        AddIfSet(parts, "max-width", MaxWidth);
        AddIfSet(parts, "max-height", MaxHeight);

        if (!string.IsNullOrWhiteSpace(ownStyle))
        {
            parts.Add(ownStyle.Trim().TrimEnd(';'));
        }

        if (!string.IsNullOrWhiteSpace(Style))
        {
            parts.Add(Style.Trim().TrimEnd(';'));
        }

        return string.Join("; ", parts) + ";";
    }

    private static readonly Regex BareNumber = new(@"^-?\d+(\.\d+)?$", RegexOptions.Compiled);

    private static void AddIfSet(List<string> parts, string property, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        parts.Add($"{property}: {NormalizeLength(value)}");
    }

    /// <summary>
    /// Голое число без единицы измерения ("300") — невалидный CSS для width/height/
    /// padding/margin и т.п.: браузер тихо отбрасывает ВСЮ декларацию целиком (в отличие
    /// от, скажем, line-height/z-index/opacity, где unitless-число как раз обязательно).
    /// Для DX (как у MudBlazor, где Width/Height принимают просто число) достраиваем "px" —
    /// значения уже с единицей ("50%", "20rem") или CSS-функцией (calc(...)) не трогаем.
    /// "0" через "px" не портится — 0px CSS-эквивалентен голому 0.
    /// </summary>
    protected static string NormalizeLength(string value)
    {
        var trimmed = value.Trim();
        return BareNumber.IsMatch(trimmed) ? $"{trimmed}px" : trimmed;
    }
}
