# ScarnaTooltip

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Overlays/ScarnaTooltip.razor`

## Описание

Всплывающая подсказка по наведению курсора/фокусу клавиатурой. Построена поверх
[ScarnaPopover](popover.md) — живой пример того, как декларативные компоненты
используют примитив поповера. Требует `ScarnaPopoverProvider` в дереве выше (см.
[getting-started.md](getting-started.md)).

## Пример использования

```razor
<ScarnaTooltip Text="Сохранить изменения">
    <ScarnaButton>💾</ScarnaButton>
</ScarnaTooltip>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Text` | `string` | — | **Обязательный.** Текст подсказки |
| `Position` | `ScarnaPosition` | `Top` | Позиция подсказки относительно якоря (см. [popover.md](popover.md#scarnaposition)) |
| `ChildContent` | `RenderFragment?` | — | Элемент-триггер (то, над чем наводят курсор) |
