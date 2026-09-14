# ScarnaBottomMenu / ScarnaBottomMenuItem

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaBottomMenu.razor`
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaBottomMenuItem.razor`

## Описание

Нижнее меню — панель вкладок, прибитая к низу экрана (`position: fixed`), как в
мобильных приложениях (iOS tab bar / Android bottom navigation): иконка сверху,
подпись снизу. В отличие от [ScarnaDrawer](drawer.md) с `Anchor="Bottom"` — не
выезжающая панель поверх контента по требованию, а всегда видимая строка навигации.

Регистрация пунктов и переключение активного устроены так же, как у
[ScarnaTabs](tabs.md) (`Register`/`Unregister` через `[CascadingParameter]`, индекс
вместо ссылки на объект), но без JS-измерения индикатора — здесь просто подсвечивается
активная иконка/подпись цветом `primary`.

Учитывает safe-area у устройств с "чёлкой"/жестовой полосой снизу
(`padding-bottom: env(safe-area-inset-bottom)`).

## Пример использования

```razor
<ScarnaBottomMenu @bind-SelectedIndex="_tab">
    <ScarnaBottomMenuItem Text="Главная"><HomeIcon /></ScarnaBottomMenuItem>
    <ScarnaBottomMenuItem Text="Поиск"><SearchIcon /></ScarnaBottomMenuItem>
    <ScarnaBottomMenuItem Text="Профиль"><ProfileIcon /></ScarnaBottomMenuItem>
</ScarnaBottomMenu>
```

Каждый `ScarnaBottomMenuItem` также поддерживает свой `OnClick` в дополнение к выбору
(удобно для навигации через [Navigator](navigator.md), например
`OnClick='() => Navigator.NavigateTo("/search")'`).

## Параметры ScarnaBottomMenu

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `SelectedIndex` / `SelectedIndexChanged` | `int` | `0` | Механика `@bind-SelectedIndex` |
| `ChildContent` | `RenderFragment?` | — | Набор `ScarnaBottomMenuItem` |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |

## Параметры ScarnaBottomMenuItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Text` | `string?` | — | Подпись под иконкой |
| `ChildContent` | `RenderFragment?` | — | Иконка (svg, обычно 20-22px) |
| `OnClick` | `EventCallback` | — | Доп. коллбэк клика (в дополнение к выбору пункта) |
