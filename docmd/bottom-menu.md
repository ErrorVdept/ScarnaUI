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

Фон настраивается так же, как у [ScarnaPaper](paper.md) (`Bordered`/`Shadow`/`Radius`/
`GlassEffect`/`Color`). `Floating` переключает вид на плавающую панель с отступом снизу
и с боков (пилюля, а не полоса во всю ширину) — обычно вместе с `Radius="Radius.Full"`
и `Shadow="true"`:

```razor
<ScarnaBottomMenu Floating="true" Radius="Radius.Full" Shadow="true" @bind-SelectedIndex="_tab">
    <ScarnaBottomMenuItem Text="Главная"><HomeIcon /></ScarnaBottomMenuItem>
    <ScarnaBottomMenuItem Text="Поиск"><SearchIcon /></ScarnaBottomMenuItem>
    <ScarnaBottomMenuItem Text="Профиль"><ProfileIcon /></ScarnaBottomMenuItem>
</ScarnaBottomMenu>
```

## Параметры ScarnaBottomMenu

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `SelectedIndex` / `SelectedIndexChanged` | `int` | `0` | Механика `@bind-SelectedIndex` |
| `Floating` | `bool` | `false` | Плавающая панель с отступом снизу и с боков (`inset-x-4 bottom-4`) вместо полосы во всю ширину экрана |
| `Bordered` | `bool` | `true` | Рамка сверху (обычный вид) или по периметру (`Floating`) |
| `Shadow` | `bool` | `false` | Тень под панелью |
| `Radius` | `Radius` | `None` | Радиус скругления углов (для `Floating` обычно `Full`) |
| `GlassEffect` | `bool` | `false` | Полупрозрачный фон с блюром вместо сплошного `bg-content1` — как у `ScarnaNavBar` |
| `Color` | `Color` | `Default` | Фон панели. `Default` — нейтральная поверхность темы; `Transparent` — без заливки; остальные — заливка цветом темы. Игнорируется при `GlassEffect="true"` |
| `ChildContent` | `RenderFragment?` | — | Набор `ScarnaBottomMenuItem` |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |

## Параметры ScarnaBottomMenuItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Text` | `string?` | — | Подпись под иконкой |
| `ChildContent` | `RenderFragment?` | — | Иконка (svg, обычно 20-22px) |
| `OnClick` | `EventCallback` | — | Доп. коллбэк клика (в дополнение к выбору пункта) |
