# NorthTooltip

## Файлы
- `src/NorthUI/NorthUI/Components/Overlays/NorthTooltip.razor`

## Описание

Всплывающая подсказка по наведению курсора/фокусу клавиатурой. Построена поверх
[NorthPopover](popover.md) — живой пример того, как декларативные компоненты
используют примитив поповера. Требует `NorthPopoverProvider` в дереве выше (см.
[getting-started.md](getting-started.md)).

## Пример использования

```razor
<NorthTooltip Text="Сохранить изменения">
    <NorthButton>💾</NorthButton>
</NorthTooltip>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Text` | `string` | — | **Обязательный.** Текст подсказки |
| `Position` | `NorthPosition` | `Top` | Позиция подсказки относительно якоря (см. [popover.md](popover.md#northposition)) |
| `ChildContent` | `RenderFragment?` | — | Элемент-триггер (то, над чем наводят курсор) |
