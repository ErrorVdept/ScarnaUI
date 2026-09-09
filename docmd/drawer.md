# NorthDrawer

## Файлы
- `src/NorthUI/NorthUI/Components/Layout/NorthDrawer.razor`
- `src/NorthUI/NorthUI/Components/Layout/NorthDrawerEnums.cs` (`NorthDrawerAnchor`)

## Описание

Выезжающая панель (боковое меню, панель фильтров). В отличие от
[диалогов](dialog-service.md) — не императивный сервис, а обычный декларативный
компонент: кладётся в разметку (обычно в layout) и управляется через `@bind-Open`,
как `MudDrawer`.

## Пример использования

```razor
<NorthDrawer @bind-Open="_menuOpen" Anchor="NorthDrawerAnchor.Left">
    <NorthList TValue="string" SelectionMode="NorthListSelectionMode.None" Class="p-3">
        <NorthListItem Value="@("home")">Главная</NorthListItem>
    </NorthList>
</NorthDrawer>

<NorthButton OnClick="() => _menuOpen = true">Меню</NorthButton>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Open` / `OpenChanged` | `bool` / ... | — | Открыта ли панель. Поддерживает `@bind-Open` |
| `Anchor` | `NorthDrawerAnchor` | `Left` | `Left`/`Right`/`Top`/`Bottom` |
| `Width` | `string` | `"280px"` | Ширина панели (для `Left`/`Right`) — любое CSS-значение |
| `Height` | `string` | `"280px"` | Высота панели (для `Top`/`Bottom`) |
| `CloseOnBackdropClick` | `bool` | `true` | Закрывать по клику на подложку |
| `ChildContent` | `RenderFragment?` | — | Содержимое панели |
