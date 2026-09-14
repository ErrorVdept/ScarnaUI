namespace ScarnaUI.Components.Forms;

/// <summary>
/// Тип числа в <see cref="ScarnaNumericField"/> — как выбор типа переменной (int/decimal)
/// у MudNumericField. Integer округляет любой ввод и любой шаг до целого значения.
/// </summary>
public enum ScarnaNumberFormat
{
    Integer,
    Decimal,
}
