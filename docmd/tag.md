# ScarnaTag

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaTag.razor`

## Описание

Маленькая компактная метка — облегчённый [ScarnaChip](chip.md): меньше паддингов,
всегда один компактный размер (без `Size`) и без встроенной кнопки закрытия. Для
кликабельных тегов с крестиком/иконкой и выбором размера используйте `ScarnaChip`.

## Пример использования

```razor
<ScarnaTag>новое</ScarnaTag>
<ScarnaTag Color="ScarnaColor.Success">активен</ScarnaTag>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `ScarnaColor` | `Default` | Цвет заливки |
| `Variant` | `ScarnaVariant` | `Flat` | Вариант заливки (см. [ScarnaColor/ScarnaVariant](getting-started.md)) |
| `ChildContent` | `RenderFragment?` | — | Текст метки |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
