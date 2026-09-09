using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace NorthUI.Components.Base;

/// <summary>
/// Базовый класс для полей ввода (Input, Select, Checkbox, DatePicker и т.д.).
/// Даёт "из коробки":
///  - двустороннее связывание <c>@bind-Value</c> через Value/ValueChanged/ValueExpression;
///  - интеграцию с EditForm/EditContext (регистрация поля, подсветка ошибок валидации,
///    пересчёт при вызове EditContext.Validate()).
///
/// Наследник должен:
///  1. вызвать <c>await SetValueAsync(newValue)</c> при изменении значения пользователем
///     (это обновит Value, вызовет ValueChanged и уведомит EditContext);
///  2. при необходимости использовать <see cref="FieldCssClass"/> для подсветки
///     valid/invalid/modified состояния поля (как North-input--invalid).
/// </summary>
/// <typeparam name="T">Тип значения поля.</typeparam>
public abstract class NorthFormComponentBase<T> : NorthComponentBase, IDisposable
{
    private bool _hasInitializedField;
    private FieldIdentifier _fieldIdentifier;

    /// <summary>Текущее значение поля.</summary>
    [Parameter]
    public T? Value { get; set; }

    /// <summary>Коллбэк, вызываемый при изменении значения (часть механики <c>@bind-Value</c>).</summary>
    [Parameter]
    public EventCallback<T?> ValueChanged { get; set; }

    /// <summary>
    /// Выражение, указывающее на связанное свойство модели — вторая часть механики <c>@bind-Value</c>.
    /// Blazor подставляет его автоматически при использовании <c>@bind-Value="model.Prop"</c>.
    /// </summary>
    [Parameter]
    public Expression<Func<T>>? ValueExpression { get; set; }

    /// <summary>Текст ошибки/подсказки под полем, задаваемый вручную (в дополнение к валидации EditContext).</summary>
    [Parameter]
    public string? ErrorText { get; set; }

    /// <summary>Принудительно помечает поле как невалидное, независимо от EditContext.</summary>
    [Parameter]
    public bool Error { get; set; }

    /// <summary>
    /// Каскадный EditContext от родительской <c>EditForm</c>. Может отсутствовать —
    /// компонент обязан работать и вне формы (standalone-режим).
    /// </summary>
    [CascadingParameter]
    private EditContext? CascadedEditContext { get; set; }

    /// <summary>EditContext, к которому подключено поле (если компонент используется внутри EditForm).</summary>
    protected EditContext? EditContext => CascadedEditContext;

    /// <summary>True, если EditContext считает значение поля невалидным.</summary>
    protected bool IsInvalid => Error || (EditContext is not null && _hasInitializedField && EditContext.GetValidationMessages(_fieldIdentifier).Any());

    /// <summary>Готовая CSS-модификаторная строка ("north-invalid", "north-modified") для подсветки состояния поля из EditContext.</summary>
    protected string FieldCssClass => EditContext is not null && _hasInitializedField
        ? EditContext.FieldCssClass(_fieldIdentifier)
        : string.Empty;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (ValueExpression is not null)
        {
            _fieldIdentifier = FieldIdentifier.Create(ValueExpression);
            _hasInitializedField = true;
        }
    }

    /// <summary>
    /// Вызывается наследником при вводе пользователем нового значения.
    /// Обновляет Value, поднимает ValueChanged и уведомляет EditContext о том,
    /// что поле изменилось (для повторной валидации и подсветки состояния).
    /// </summary>
    protected async Task SetValueAsync(T? value)
    {
        if (EqualityComparer<T?>.Default.Equals(Value, value))
        {
            return;
        }

        Value = value;

        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(value);
        }

        if (EditContext is not null && _hasInitializedField)
        {
            EditContext.NotifyFieldChanged(_fieldIdentifier);
        }
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
