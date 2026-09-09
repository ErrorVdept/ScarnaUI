namespace NorthUI.Components.Forms;

/// <summary>
/// Тип числа в <see cref="NorthNumericField"/> — как выбор типа переменной (int/decimal)
/// у MudNumericField. Integer округляет любой ввод и любой шаг до целого значения.
/// </summary>
public enum NorthNumberFormat
{
    Integer,
    Decimal,
}
