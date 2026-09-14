# ScarnaToggleButton

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Actions/ScarnaToggleButton.razor`

## Описание

Кнопка-переключатель — семантически чекбокс (`bool`, основан на
`ScarnaFormComponentBase<bool>`, поддерживает `@bind-Value`), но выглядит как обычная
кнопка, которая визуально "нажата"/"не нажата" (аналог `MudToggleIconButton`, только
с произвольным содержимым, не только иконкой) — удобно для панели инструментов
(Ж/К/Ч в редакторе текста и т.п.).

## Пример использования

```razor
<ScarnaStack Row="true" Spacing="2">
    <ScarnaToggleButton @bind-Value="_bold">Ж</ScarnaToggleButton>
    <ScarnaToggleButton @bind-Value="_italic">К</ScarnaToggleButton>
</ScarnaStack>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `bool` / ... | — | Механика `@bind-Value` |
| `Color` | `ScarnaColor` | `Primary` | Цвет заливки во включённом состоянии |
| `Size` | `ScarnaSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Radius` | `ScarnaRadius` | `Medium` | Радиус углов |
| `Disabled` | `bool` | `false` | — |
| `Bordered` | `bool` | `true` | Обводка в невключённом состоянии |
| `ChildContent` | `RenderFragment?` | — | Содержимое кнопки |
