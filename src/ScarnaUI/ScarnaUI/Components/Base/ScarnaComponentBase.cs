using Microsoft.AspNetCore.Components;

namespace ScarnaUI.Components.Base;

/// <summary>
/// Базовый класс для всех компонентов ScarnaUI.
/// Даёт единый способ прокинуть <c>class</c>, <c>style</c> и любые произвольные
/// HTML-атрибуты (data-*, aria-*, onclick и т.д.) на корневой элемент компонента —
/// ровно так же, как это устроено в MudBlazor.
///
/// Как использовать в наследнике:
/// <code>
/// &lt;div class="@Classes" style="@Style" @attributes="UserAttributes"&gt;...&lt;/div&gt;
///
/// @code {
///     protected string Classes =&gt; CssBuilder.Default("Scarna-card")
///         .AddClass("Scarna-card--bordered", Bordered)
///         .AddClass(Class)
///         .ToString();
/// }
/// </code>
/// </summary>
public abstract class ScarnaComponentBase : ComponentBase
{
    /// <summary>Дополнительные CSS-классы, добавляемые к классам компонента (не заменяют их).</summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>Инлайн-стили, добавляемые к корневому элементу компонента.</summary>
    [Parameter]
    public string? Style { get; set; }

    /// <summary>
    /// Любые атрибуты, не описанные как параметры компонента явно (data-*, aria-*, id,
    /// обработчики событий и т.п.), прозрачно прокидываются на корневой HTML-элемент.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? UserAttributes { get; set; }
}
