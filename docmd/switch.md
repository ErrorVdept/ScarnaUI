# ScarnaSwitch

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaSwitch.razor`

## Описание

Переключатель (toggle) — семантически то же самое, что чекбокс (`bool`), но с другой
визуальной метафорой "вкл/выкл", как в HeroUI/iOS. Тоже основан на
`ScarnaFormComponentBase<bool>` и поддерживает `@bind-Value`.

## Пример использования

```razor
<ScarnaSwitch @bind-Value="model.NotificationsEnabled" Color="ScarnaColor.Success">
    Уведомления по email
</ScarnaSwitch>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `bool` / ... | — | Механика `@bind-Value` |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | Запрещает переключение (клик отменяется через `@onclick:preventDefault`), но, в отличие от `Disabled`, переключатель визуально не тускнеет |
| `Color` | `ScarnaColor` | `Primary` | Цвет дорожки во включённом состоянии |
| `Density` | `ScarnaDensity` | `Normal` | `Dense` — уменьшенная дорожка для плотных списков |
| `ChildContent` | `RenderFragment?` | — | Подпись рядом с переключателем |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
