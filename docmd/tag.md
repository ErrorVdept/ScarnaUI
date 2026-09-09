# NorthTag

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthTag.razor`

## Описание

Маленькая компактная метка — облегчённый [NorthChip](chip.md): меньше паддингов,
всегда один компактный размер (без `Size`) и без встроенной кнопки закрытия. Для
кликабельных тегов с крестиком/иконкой и выбором размера используйте `NorthChip`.

## Пример использования

```razor
<NorthTag>новое</NorthTag>
<NorthTag Color="NorthColor.Success">активен</NorthTag>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `NorthColor` | `Default` | Цвет заливки |
| `Variant` | `NorthVariant` | `Flat` | Вариант заливки (см. [NorthColor/NorthVariant](getting-started.md)) |
| `ChildContent` | `RenderFragment?` | — | Текст метки |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
