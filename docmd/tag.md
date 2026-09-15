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
<ScarnaTag Color="Color.Success">активен</ScarnaTag>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `Color` | `Default` | Цвет заливки |
| `Variant` | `Variant` | `Flat` | Вариант заливки (см. [Color/Variant](getting-started.md)) |
| `ChildContent` | `RenderFragment?` | — | Текст метки |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
