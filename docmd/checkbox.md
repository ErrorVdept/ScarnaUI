# ScarnaCheckbox

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaCheckbox.razor`

## Описание

Чекбокс на основе `ScarnaFormComponentBase<bool>` — поддерживает `@bind-Value`.
Подпись задаётся через `ChildContent`.

## Пример использования

```razor
<ScarnaCheckbox @bind-Value="model.AcceptTerms" Color="Color.Success">
    Согласен с условиями использования
</ScarnaCheckbox>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `bool` / ... | — | Механика `@bind-Value` |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | Запрещает переключение (клик отменяется через `@onclick:preventDefault`), но, в отличие от `Disabled`, чекбокс визуально не тускнеет |
| `Color` | `Color` | `Primary` | Цвет заливки в отмеченном состоянии |
| `Density` | `ScarnaDensity` | `Normal` | `Dense` — уменьшенный квадрат для плотных списков |
| `ChildContent` | `RenderFragment?` | — | Подпись рядом с чекбоксом |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
