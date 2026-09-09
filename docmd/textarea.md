# NorthTextArea

## Файлы
- `src/NorthUI/NorthUI/Components/Forms/NorthTextArea.razor`

## Описание

Многострочное текстовое поле — то же самое, что [NorthInput](input.md), но `<textarea>`
вместо `<input>`. Основан на `NorthFormComponentBase<string>` — поддерживает `@bind-Value`.

`AutoResize` использует CSS `field-sizing: content` (современные Chromium/Edge) —
поле растёт по высоте вслед за содержимым без JS. В браузерах без поддержки просто
остаётся высота, заданная `Rows`.

## Пример использования

```razor
<NorthTextArea Label="Описание" @bind-Value="model.Description" Rows="4" />
<NorthTextArea Label="Комментарий" @bind-Value="model.Comment" AutoResize="true" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `string` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над полем |
| `Placeholder` | `string?` | — | — |
| `HelperText` / `ErrorText` / `Error` | — | — | См. [getting-started.md](getting-started.md) |
| `Rows` | `int` | `3` | Количество видимых строк (влияет на высоту, только если `AutoResize` выключен) |
| `AutoResize` | `bool` | `false` | Расти по высоте вслед за содержимым (`field-sizing: content`) |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | — |
| `Variant` | `NorthVariant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля |
| `Radius` | `NorthRadius` | `Medium` | Радиус поля |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
