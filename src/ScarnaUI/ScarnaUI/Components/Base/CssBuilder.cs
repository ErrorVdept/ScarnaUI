using System.Text;

namespace ScarnaUI.Components.Base;

/// <summary>
/// Небольшой fluent-хелпер для сборки строки CSS-классов.
/// Аналог внутреннего CssBuilder в MudBlazor: позволяет компонентам собирать
/// "Scarna-btn Scarna-btn--primary Scarna-btn--disabled" из набора условий,
/// не городя тройные тернарники прямо в разметке .razor.
/// </summary>
public sealed class CssBuilder
{
    private readonly StringBuilder _builder = new();

    private CssBuilder(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            _builder.Append(value);
        }
    }

    /// <summary>Создаёт билдер, опционально начиная с базового класса.</summary>
    public static CssBuilder Default(string? value = null) => new(value);

    /// <summary>Добавляет класс, если <paramref name="when"/> истинно (по умолчанию — всегда).</summary>
    public CssBuilder AddClass(string? value, bool when = true)
    {
        if (!when || string.IsNullOrWhiteSpace(value))
        {
            return this;
        }

        if (_builder.Length > 0)
        {
            _builder.Append(' ');
        }

        _builder.Append(value);
        return this;
    }

    /// <summary>Добавляет класс, вычисляемый лениво (полезно, когда построение строки не бесплатно), только если <paramref name="when"/> истинно.</summary>
    public CssBuilder AddClass(Func<string?> valueFactory, bool when = true) => when ? AddClass(valueFactory()) : this;

    /// <summary>
    /// Добавляет произвольные пользовательские классы последними — обычно сюда передают
    /// параметр компонента <c>Class</c>, чтобы пользователь мог переопределить стили компонента.
    /// </summary>
    public CssBuilder AddClassFromAttributes(string? userClass) => AddClass(userClass);

    public override string ToString() => _builder.ToString();

    public static implicit operator string(CssBuilder builder) => builder.ToString();
}
