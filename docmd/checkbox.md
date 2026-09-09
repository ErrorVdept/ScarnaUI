# NorthCheckbox

## Файлы
- `src/NorthUI/NorthUI/Components/Forms/NorthCheckbox.razor`

## Описание

Чекбокс на основе `NorthFormComponentBase<bool>` — поддерживает `@bind-Value`.
Подпись задаётся через `ChildContent`.

## Пример использования

```razor
<NorthCheckbox @bind-Value="model.AcceptTerms" Color="NorthColor.Success">
    Согласен с условиями использования
</NorthCheckbox>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `bool` / ... | — | Механика `@bind-Value` |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | Запрещает переключение (клик отменяется через `@onclick:preventDefault`), но, в отличие от `Disabled`, чекбокс визуально не тускнеет |
| `Color` | `NorthColor` | `Primary` | Цвет заливки в отмеченном состоянии |
| `Density` | `NorthDensity` | `Normal` | `Dense` — уменьшенный квадрат для плотных списков |
| `ChildContent` | `RenderFragment?` | — | Подпись рядом с чекбоксом |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
