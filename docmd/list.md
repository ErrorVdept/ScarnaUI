# NorthList / NorthListItem

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthList.razor`
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthListItem.razor`
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthListEnums.cs` (`NorthListSelectionMode`)

## Описание

Список с опциональным выбором — как `MudList`: `SelectionMode.None` (просто
кликабельные пункты через `NorthListItem.OnClick` — например, навигационный список в
[NorthDrawer](drawer.md)), `.Single` (`@bind-Value`, один выбранный пункт) или
`.Multiple` (`@bind-SelectedValues`, несколько). Дженерик — `TValue` выводится
автоматически из значений `Value` у вложенных `NorthListItem`; если ни `Value`, ни
`SelectedValues` не заданы (типичный случай для `SelectionMode.None`), укажите
`TValue` явно.

## Пример использования

```razor
@* Одиночный выбор *@
<NorthList SelectionMode="NorthListSelectionMode.Single" @bind-Value="_selected">
    <NorthListItem Value="@("inbox")">Входящие</NorthListItem>
    <NorthListItem Value="@("sent")">Отправленные</NorthListItem>
</NorthList>

@* Множественный выбор *@
<NorthList SelectionMode="NorthListSelectionMode.Multiple" @bind-SelectedValues="_selectedIds">
    <NorthListItem Value="1">Пункт 1</NorthListItem>
    <NorthListItem Value="2">Пункт 2</NorthListItem>
</NorthList>

@* Просто кликабельный список — TValue нужно указать явно *@
<NorthList TValue="string" SelectionMode="NorthListSelectionMode.None">
    <NorthListItem Value="@("home")" OnClick="GoHome">Главная</NorthListItem>
</NorthList>
```

## Параметры NorthList

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `SelectionMode` | `NorthListSelectionMode` | `None` | `None`/`Single`/`Multiple` |
| `Value` / `ValueChanged` | `TValue?` / ... | — | Выбранное значение при `Single` |
| `SelectedValues` / `SelectedValuesChanged` | `IReadOnlyCollection<TValue>?` / ... | — | Выбранные значения при `Multiple` |
| `Bordered` | `bool` | `false` | Обводка списка |
| `Density` | `NorthDensity` | `Normal` | Зарезервировано для компактного режима |
| `ChildContent` | `RenderFragment?` | — | Набор `NorthListItem` |

## Параметры NorthListItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` | `TValue` | — | **Обязательный.** Значение пункта |
| `StartContent` | `RenderFragment?` | — | Иконка/аватар перед текстом |
| `ChildContent` | `RenderFragment?` | — | Содержимое пункта |
| `Disabled` | `bool` | `false` | — |
| `OnClick` | `EventCallback` | — | Доп. обработчик клика — вызывается при любом `SelectionMode`, в т.ч. `None` |
