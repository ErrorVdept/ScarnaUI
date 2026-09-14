# ScarnaMenu / ScarnaMenuItem

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Navigation/ScarnaMenu.razor`
- `src/ScarnaUI/ScarnaUI/Components/Navigation/ScarnaMenuItem.razor`

## Описание

Выпадающее контекстное меню. Построено на [ScarnaPopover](popover.md) — требует
`ScarnaPopoverProvider` в дереве выше. Клик по пункту (`ScarnaMenuItem`) сам закрывает
меню — отдельно это делать не нужно.

## Пример использования

```razor
<ScarnaMenu>
    <Trigger>
        <ScarnaButton Variant="ScarnaVariant.Bordered">Действия</ScarnaButton>
    </Trigger>
    <MenuContent>
        <ScarnaMenuItem OnClick="Edit">Редактировать</ScarnaMenuItem>
        <ScarnaMenuItem OnClick="Duplicate">Дублировать</ScarnaMenuItem>
        <ScarnaDivider Class="my-1" />
        <ScarnaMenuItem Color="ScarnaColor.Danger" OnClick="Delete">Удалить</ScarnaMenuItem>
    </MenuContent>
</ScarnaMenu>
```

Отдельного компонента-разделителя нет — используйте `<ScarnaDivider Class="my-1" />"`
(см. [divider.md](divider.md)) прямо внутри `MenuContent`.

## Параметры ScarnaMenu

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Trigger` | `RenderFragment?` | — | Элемент, открывающий меню по клику (обычно кнопка) |
| `MenuContent` | `RenderFragment?` | — | Набор `ScarnaMenuItem`/`ScarnaDivider` |
| `Position` | `ScarnaPosition` | `BottomStart` | Позиция меню относительно триггера (см. [popover.md](popover.md#scarnaposition)) |
| `IsParentFullSize` | `bool` | `false` | Растянуть меню на ширину триггера |

## Параметры ScarnaMenuItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `ScarnaColor` | `Default` | `Danger` — для деструктивных действий (удаление и т.п.) |
| `Disabled` | `bool` | `false` | — |
| `StartContent` | `RenderFragment?` | — | Иконка перед текстом |
| `ChildContent` | `RenderFragment?` | — | Текст пункта |
| `OnClick` | `EventCallback` | — | Обработчик выбора пункта (меню закроется автоматически после) |

`ScarnaMenuItem` обязан использоваться внутри `ScarnaMenu` — иначе бросает исключение
при инициализации.
