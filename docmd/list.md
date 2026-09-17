# ScarnaList / ScarnaListItem

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaList.razor`
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaListItem.razor`
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaListEnums.cs` (`ScarnaListSelectionMode`)

## Описание

Список с опциональным выбором — как `MudList`: `SelectionMode.None` (просто
кликабельные пункты через `ScarnaListItem.OnClick` — например, навигационный список в
[ScarnaDrawer](drawer.md)), `.Single` (`@bind-Value`, один выбранный пункт) или
`.Multiple` (`@bind-SelectedValues`, несколько). Дженерик — `TValue` выводится
автоматически из значений `Value` у вложенных `ScarnaListItem`; если ни `Value`, ни
`SelectedValues` не заданы (типичный случай для `SelectionMode.None`), укажите
`TValue` явно.

## Пример использования

```razor
@* Одиночный выбор *@
<ScarnaList SelectionMode="ScarnaListSelectionMode.Single" @bind-Value="_selected">
    <ScarnaListItem Value="@("inbox")">Входящие</ScarnaListItem>
    <ScarnaListItem Value="@("sent")">Отправленные</ScarnaListItem>
</ScarnaList>

@* Множественный выбор *@
<ScarnaList SelectionMode="ScarnaListSelectionMode.Multiple" @bind-SelectedValues="_selectedIds">
    <ScarnaListItem Value="1">Пункт 1</ScarnaListItem>
    <ScarnaListItem Value="2">Пункт 2</ScarnaListItem>
</ScarnaList>

@* Просто кликабельный список — TValue нужно указать явно *@
<ScarnaList TValue="string" SelectionMode="ScarnaListSelectionMode.None">
    <ScarnaListItem Value="@("home")" OnClick="GoHome">Главная</ScarnaListItem>
</ScarnaList>
```

## Параметры ScarnaList

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `SelectionMode` | `ScarnaListSelectionMode` | `None` | `None`/`Single`/`Multiple` |
| `Value` / `ValueChanged` | `TValue?` / ... | — | Выбранное значение при `Single` |
| `SelectedValues` / `SelectedValuesChanged` | `IReadOnlyCollection<TValue>?` / ... | — | Выбранные значения при `Multiple` |
| `Bordered` | `bool` | `false` | Обводка списка |
| `Color` | `Color` | `Default` | Фон списка. `Default` — нейтральная поверхность темы (`bg-content1`); `Transparent` — без заливки; остальные — заливка цветом темы |
| `Density` | `ScarnaDensity` | `Normal` | Зарезервировано для компактного режима |
| `ChildContent` | `RenderFragment?` | — | Набор `ScarnaListItem` |

## Параметры ScarnaListItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` | `TValue` | — | **Обязательный.** Значение пункта |
| `StartContent` | `RenderFragment?` | — | Иконка/аватар перед текстом |
| `ChildContent` | `RenderFragment?` | — | Содержимое пункта |
| `Disabled` | `bool` | `false` | — |
| `OnClick` | `EventCallback` | — | Доп. обработчик клика — вызывается при любом `SelectionMode`, в т.ч. `None` |
