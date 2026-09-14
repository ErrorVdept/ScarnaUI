# ScarnaTimeField

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaTimeField.razor`

## Описание

Поле выбора времени. Под капотом — нативный `input[type=time]` (браузерный
time-picker бесплатно, без своей JS-хореографии). Основан на
`ScarnaFormComponentBase<TimeOnly?>` — наружу компонент отдаёт готовый `TimeOnly?`,
а не строку: формат `"HH:mm"`, который ожидает атрибут `value` у `input[type=time]`,
собирается/парсится внутри компонента.

## Пример использования

```razor
<ScarnaTimeField Label="Время начала" @bind-Value="model.StartTime" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `TimeOnly?` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над полем |
| `HelperText` / `ErrorText` / `Error` | — | — | См. [getting-started.md](getting-started.md) |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | — |
| `Variant` | `ScarnaVariant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля |
| `Size` | `ScarnaSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Density` | `ScarnaDensity` | `Normal` | `Dense` — компактное поле |
| `Radius` | `ScarnaRadius` | `Medium` | Радиус поля |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
