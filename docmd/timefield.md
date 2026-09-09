# NorthTimeField

## Файлы
- `src/NorthUI/NorthUI/Components/Forms/NorthTimeField.razor`

## Описание

Поле выбора времени. Под капотом — нативный `input[type=time]` (браузерный
time-picker бесплатно, без своей JS-хореографии). Основан на
`NorthFormComponentBase<TimeOnly?>` — наружу компонент отдаёт готовый `TimeOnly?`,
а не строку: формат `"HH:mm"`, который ожидает атрибут `value` у `input[type=time]`,
собирается/парсится внутри компонента.

## Пример использования

```razor
<NorthTimeField Label="Время начала" @bind-Value="model.StartTime" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `TimeOnly?` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над полем |
| `HelperText` / `ErrorText` / `Error` | — | — | См. [getting-started.md](getting-started.md) |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | — |
| `Variant` | `NorthVariant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля |
| `Size` | `NorthSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Density` | `NorthDensity` | `Normal` | `Dense` — компактное поле |
| `Radius` | `NorthRadius` | `Medium` | Радиус поля |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
