# ScarnaNavMenu / ScarnaNavLink

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaNavMenu.razor`
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaNavLink.razor`

## Описание

Вертикальное меню навигации (аналог `MudNavMenu`/`MudNavLink`) — обычно кладётся
внутрь [ScarnaDrawer](drawer.md). `ScarnaNavMenu` — просто контейнер (`flex flex-col`),
`ScarnaNavLink` — пункт-ссылка, который сам определяет, активен ли он, сверяясь с
текущим URL через [Navigator (INavigator)](navigator.md), и подсвечивается фоном/цветом
(`bg-primary/10 text-primary`), как выбранный пункт в [ScarnaList](list.md). Не требует
`@bind-`-состояния и ручного сравнения адреса — обновляется сам при любой навигации,
в т.ч. по кнопкам браузера "назад"/"вперёд".

## Пример использования

```razor
<ScarnaNavMenu>
    <ScarnaNavLink Href="/">
        <StartContent><HomeIcon /></StartContent>
        <ChildContent>Главная</ChildContent>
    </ScarnaNavLink>
    <ScarnaNavLink Href="/orders">
        <StartContent><OrdersIcon /></StartContent>
        <ChildContent>Заказы</ChildContent>
        <EndContent><ScarnaTag Color="Color.Primary">7</ScarnaTag></EndContent>
    </ScarnaNavLink>
    <ScarnaNavLink Href="/settings" Exact="true">Настройки</ScarnaNavLink>
</ScarnaNavMenu>
```

Внутри [ScarnaDrawer](drawer.md) с `Variant.Temporary` (мобильное меню) обычно нужно
закрыть панель сразу после выбора пункта — для этого есть `OnClick`, вызывается в
дополнение к самому переходу:

```razor
<ScarnaDrawer @bind-Open="_menuOpen">
    <ScarnaNavMenu Class="p-3">
        <ScarnaNavLink Href="/orders" OnClick="() => _menuOpen = false">Заказы</ScarnaNavLink>
    </ScarnaNavMenu>
</ScarnaDrawer>
```

## Параметры ScarnaNavMenu

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | — | Набор `ScarnaNavLink` |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |

## Параметры ScarnaNavLink

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Href` | `string` | — | **Обязательный.** Адрес перехода |
| `Exact` | `bool` | `false` | Требовать точного совпадения адреса. `false` — активен и для дочерних путей (например, `Href="/orders"` активен и на `/orders/5`) |
| `Target` | `string?` | — | `target` для ссылки |
| `Disabled` | `bool` | `false` | Блокирует переход и клики, гасит непрозрачность |
| `StartContent` | `RenderFragment?` | — | Иконка перед текстом |
| `EndContent` | `RenderFragment?` | — | Содержимое после текста — например, счётчик/бейдж |
| `ChildContent` | `RenderFragment?` | — | Текст пункта |
| `OnClick` | `EventCallback` | — | Доп. обработчик клика в дополнение к переходу (не вызывается, если `Disabled`) |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
