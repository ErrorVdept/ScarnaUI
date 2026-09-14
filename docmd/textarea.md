# ScarnaTextArea

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaTextArea.razor`

## Описание

Многострочное текстовое поле — то же самое, что [ScarnaInput](input.md), но `<textarea>`
вместо `<input>`. Основан на `ScarnaFormComponentBase<string>` — поддерживает `@bind-Value`.

`AutoResize` использует CSS `field-sizing: content` (современные Chromium/Edge) —
поле растёт по высоте вслед за содержимым без JS. В браузерах без поддержки просто
остаётся высота, заданная `Rows`.

## Пример использования

```razor
<ScarnaTextArea Label="Описание" @bind-Value="model.Description" Rows="4" />
<ScarnaTextArea Label="Комментарий" @bind-Value="model.Comment" AutoResize="true" />
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
| `Variant` | `ScarnaVariant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля |
| `Radius` | `ScarnaRadius` | `Medium` | Радиус поля |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
