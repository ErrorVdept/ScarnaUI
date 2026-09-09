# NorthSwitch

## Файлы
- `src/NorthUI/NorthUI/Components/Forms/NorthSwitch.razor`

## Описание

Переключатель (toggle) — семантически то же самое, что чекбокс (`bool`), но с другой
визуальной метафорой "вкл/выкл", как в HeroUI/iOS. Тоже основан на
`NorthFormComponentBase<bool>` и поддерживает `@bind-Value`.

## Пример использования

```razor
<NorthSwitch @bind-Value="model.NotificationsEnabled" Color="NorthColor.Success">
    Уведомления по email
</NorthSwitch>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `bool` / ... | — | Механика `@bind-Value` |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | Запрещает переключение (клик отменяется через `@onclick:preventDefault`), но, в отличие от `Disabled`, переключатель визуально не тускнеет |
| `Color` | `NorthColor` | `Primary` | Цвет дорожки во включённом состоянии |
| `Density` | `NorthDensity` | `Normal` | `Dense` — уменьшенная дорожка для плотных списков |
| `ChildContent` | `RenderFragment?` | — | Подпись рядом с переключателем |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
