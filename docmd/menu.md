# NorthMenu / NorthMenuItem

## Файлы
- `src/NorthUI/NorthUI/Components/Navigation/NorthMenu.razor`
- `src/NorthUI/NorthUI/Components/Navigation/NorthMenuItem.razor`

## Описание

Выпадающее контекстное меню. Построено на [NorthPopover](popover.md) — требует
`NorthPopoverProvider` в дереве выше. Клик по пункту (`NorthMenuItem`) сам закрывает
меню — отдельно это делать не нужно.

## Пример использования

```razor
<NorthMenu>
    <Trigger>
        <NorthButton Variant="NorthVariant.Bordered">Действия</NorthButton>
    </Trigger>
    <MenuContent>
        <NorthMenuItem OnClick="Edit">Редактировать</NorthMenuItem>
        <NorthMenuItem OnClick="Duplicate">Дублировать</NorthMenuItem>
        <NorthDivider Class="my-1" />
        <NorthMenuItem Color="NorthColor.Danger" OnClick="Delete">Удалить</NorthMenuItem>
    </MenuContent>
</NorthMenu>
```

Отдельного компонента-разделителя нет — используйте `<NorthDivider Class="my-1" />"`
(см. [divider.md](divider.md)) прямо внутри `MenuContent`.

## Параметры NorthMenu

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Trigger` | `RenderFragment?` | — | Элемент, открывающий меню по клику (обычно кнопка) |
| `MenuContent` | `RenderFragment?` | — | Набор `NorthMenuItem`/`NorthDivider` |
| `Position` | `NorthPosition` | `BottomStart` | Позиция меню относительно триггера (см. [popover.md](popover.md#northposition)) |
| `IsParentFullSize` | `bool` | `false` | Растянуть меню на ширину триггера |

## Параметры NorthMenuItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `NorthColor` | `Default` | `Danger` — для деструктивных действий (удаление и т.п.) |
| `Disabled` | `bool` | `false` | — |
| `StartContent` | `RenderFragment?` | — | Иконка перед текстом |
| `ChildContent` | `RenderFragment?` | — | Текст пункта |
| `OnClick` | `EventCallback` | — | Обработчик выбора пункта (меню закроется автоматически после) |

`NorthMenuItem` обязан использоваться внутри `NorthMenu` — иначе бросает исключение
при инициализации.
