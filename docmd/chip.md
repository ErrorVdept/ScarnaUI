# ScarnaChip

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaChip.razor`

## Описание

Небольшая цветная "пилюля" с текстом — тег, статус, значение фильтра. Не путать с
[ScarnaBadge](badge.md) — тот не самостоятельный, а накладывается поверх другого элемента.

## Пример использования

```razor
<ScarnaChip Color="ScarnaColor.Success" Variant="ScarnaVariant.Flat">Активен</ScarnaChip>

<ScarnaChip Color="ScarnaColor.Danger" Variant="ScarnaVariant.Faded" OnClose="() => Remove(tag)">
    @tag.Name
</ScarnaChip>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `ScarnaColor` | `Default` | См. [theme-provider.md](theme-provider.md) |
| `Variant` | `ScarnaVariant` | `Flat` | `Solid`/`Flat`/`Bordered`/`Light`/`Faded`/`Shadow` |
| `Size` | `ScarnaSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Bordered` | `bool` | `false` | Доп. обводка поверх Variant (для Variant.Bordered ничего не меняет) |
| `StartContent` | `RenderFragment?` | — | Иконка/точка перед текстом |
| `ChildContent` | `RenderFragment?` | — | Текст метки |
| `OnClose` | `EventCallback` | — | Если задан — справа появляется крестик, вызывающий этот коллбэк |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
