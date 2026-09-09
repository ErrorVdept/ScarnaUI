# NorthNumericField

## Файлы
- `src/NorthUI/NorthUI/Components/Forms/NorthNumericField.razor`
- `src/NorthUI/NorthUI/Components/Forms/NorthNumberFormat.cs`

## Описание

Числовое поле со ступенчатыми кнопками +/− по краям (клавиатурный ввод тоже
работает — под капотом нативный `input[type=number]`, но с **скрытыми родными
стрелками** — `[&::-webkit-inner-spin-button]:appearance-none` и
`[-moz-appearance:textfield]`, чтобы не дублировать свои кнопки родными). Основан на
`NorthFormComponentBase<double>` — поддерживает `@bind-Value`.

## Пример использования

```razor
<NorthNumericField Label="Количество" @bind-Value="_qty" Min="0" Max="99" Step="1" Format="NorthNumberFormat.Integer" />
<NorthNumericField Label="Цена" @bind-Value="_price" Step="0.5" Format="NorthNumberFormat.Decimal" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `double` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над полем |
| `HelperText` / `ErrorText` / `Error` | — | — | См. [getting-started.md](getting-started.md) |
| `Min` / `Max` | `double` | `double.MinValue` / `MaxValue` | Диапазон (кнопки +/− блокируются на границах) |
| `Step` | `double` | `1` | Шаг изменения |
| `Format` | `NorthNumberFormat` | `Decimal` | `Integer` — округляет любой ввод/шаг до целого (аналог выбора типа переменной у MudNumericField); `Decimal` — дробные значения разрешены |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | Поле и кнопки +/− не реагируют на ввод, но, в отличие от `Disabled`, не тускнеют |
| `Variant` | `NorthVariant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля |
| `Size` | `NorthSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Density` | `NorthDensity` | `Normal` | `Dense` — компактное поле |
| `Radius` | `NorthRadius` | `Medium` | Радиус поля |
