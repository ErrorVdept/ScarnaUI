# NorthToggleButton

## Файлы
- `src/NorthUI/NorthUI/Components/Actions/NorthToggleButton.razor`

## Описание

Кнопка-переключатель — семантически чекбокс (`bool`, основан на
`NorthFormComponentBase<bool>`, поддерживает `@bind-Value`), но выглядит как обычная
кнопка, которая визуально "нажата"/"не нажата" (аналог `MudToggleIconButton`, только
с произвольным содержимым, не только иконкой) — удобно для панели инструментов
(Ж/К/Ч в редакторе текста и т.п.).

## Пример использования

```razor
<NorthStack Row="true" Spacing="2">
    <NorthToggleButton @bind-Value="_bold">Ж</NorthToggleButton>
    <NorthToggleButton @bind-Value="_italic">К</NorthToggleButton>
</NorthStack>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `bool` / ... | — | Механика `@bind-Value` |
| `Color` | `NorthColor` | `Primary` | Цвет заливки во включённом состоянии |
| `Size` | `NorthSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Radius` | `NorthRadius` | `Medium` | Радиус углов |
| `Disabled` | `bool` | `false` | — |
| `Bordered` | `bool` | `true` | Обводка в невключённом состоянии |
| `ChildContent` | `RenderFragment?` | — | Содержимое кнопки |
