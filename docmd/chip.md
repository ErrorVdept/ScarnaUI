# NorthChip

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthChip.razor`

## Описание

Небольшая цветная "пилюля" с текстом — тег, статус, значение фильтра. Не путать с
[NorthBadge](badge.md) — тот не самостоятельный, а накладывается поверх другого элемента.

## Пример использования

```razor
<NorthChip Color="NorthColor.Success" Variant="NorthVariant.Flat">Активен</NorthChip>

<NorthChip Color="NorthColor.Danger" Variant="NorthVariant.Faded" OnClose="() => Remove(tag)">
    @tag.Name
</NorthChip>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `NorthColor` | `Default` | См. [theme-provider.md](theme-provider.md) |
| `Variant` | `NorthVariant` | `Flat` | `Solid`/`Flat`/`Bordered`/`Light`/`Faded`/`Shadow` |
| `Size` | `NorthSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Bordered` | `bool` | `false` | Доп. обводка поверх Variant (для Variant.Bordered ничего не меняет) |
| `StartContent` | `RenderFragment?` | — | Иконка/точка перед текстом |
| `ChildContent` | `RenderFragment?` | — | Текст метки |
| `OnClose` | `EventCallback` | — | Если задан — справа появляется крестик, вызывающий этот коллбэк |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
