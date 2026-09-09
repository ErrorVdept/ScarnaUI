# NorthBottomMenu / NorthBottomMenuItem

## Файлы
- `src/NorthUI/NorthUI/Components/Layout/NorthBottomMenu.razor`
- `src/NorthUI/NorthUI/Components/Layout/NorthBottomMenuItem.razor`

## Описание

Нижнее меню — панель вкладок, прибитая к низу экрана (`position: fixed`), как в
мобильных приложениях (iOS tab bar / Android bottom navigation): иконка сверху,
подпись снизу. В отличие от [NorthDrawer](drawer.md) с `Anchor="Bottom"` — не
выезжающая панель поверх контента по требованию, а всегда видимая строка навигации.

Регистрация пунктов и переключение активного устроены так же, как у
[NorthTabs](tabs.md) (`Register`/`Unregister` через `[CascadingParameter]`, индекс
вместо ссылки на объект), но без JS-измерения индикатора — здесь просто подсвечивается
активная иконка/подпись цветом `primary`.

Учитывает safe-area у устройств с "чёлкой"/жестовой полосой снизу
(`padding-bottom: env(safe-area-inset-bottom)`).

## Пример использования

```razor
<NorthBottomMenu @bind-SelectedIndex="_tab">
    <NorthBottomMenuItem Text="Главная"><HomeIcon /></NorthBottomMenuItem>
    <NorthBottomMenuItem Text="Поиск"><SearchIcon /></NorthBottomMenuItem>
    <NorthBottomMenuItem Text="Профиль"><ProfileIcon /></NorthBottomMenuItem>
</NorthBottomMenu>
```

Каждый `NorthBottomMenuItem` также поддерживает свой `OnClick` в дополнение к выбору
(удобно для навигации через [Navigator](navigator.md), например
`OnClick='() => Navigator.NavigateTo("/search")'`).

## Параметры NorthBottomMenu

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `SelectedIndex` / `SelectedIndexChanged` | `int` | `0` | Механика `@bind-SelectedIndex` |
| `ChildContent` | `RenderFragment?` | — | Набор `NorthBottomMenuItem` |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |

## Параметры NorthBottomMenuItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Text` | `string?` | — | Подпись под иконкой |
| `ChildContent` | `RenderFragment?` | — | Иконка (svg, обычно 20-22px) |
| `OnClick` | `EventCallback` | — | Доп. коллбэк клика (в дополнение к выбору пункта) |
