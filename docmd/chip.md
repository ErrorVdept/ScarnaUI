# ScarnaChip

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaChip.razor`

## Описание

Небольшая цветная "пилюля" с текстом — тег, статус, значение фильтра. Не путать с
[ScarnaBadge](badge.md) — тот не самостоятельный, а накладывается поверх другого элемента.

## Пример использования

```razor
<ScarnaChip Color="Color.Success" Variant="Variant.Flat">Активен</ScarnaChip>

<ScarnaChip Color="Color.Danger" Variant="Variant.Faded" OnClose="() => Remove(tag)">
    @tag.Name
</ScarnaChip>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `Color` | `Default` | См. [theme-provider.md](theme-provider.md) |
| `Variant` | `Variant` | `Flat` | `Solid`/`Flat`/`Bordered`/`Light`/`Faded`/`Shadow` |
| `Size` | `Size` | `Medium` | `Small`/`Medium`/`Large` |
| `Bordered` | `bool` | `false` | Доп. обводка поверх Variant (для Variant.Bordered ничего не меняет) |
| `StartContent` | `RenderFragment?` | — | Иконка/точка перед текстом |
| `ChildContent` | `RenderFragment?` | — | Текст метки |
| `OnClose` | `EventCallback` | — | Если задан — справа появляется крестик, вызывающий этот коллбэк |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
