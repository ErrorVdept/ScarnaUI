# ScarnaNavBar

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaNavBar.razor`

## Описание

Верхняя панель приложения (аналог `MudAppBar`/шапки сайта). Формализует то, что
раньше в демо собиралось вручную из `<header>` + `ScarnaStack` — задаёт высоту,
подложку, боковой паддинг и границу снизу; содержимое — произвольное (логотип, ссылки,
действия — расставляйте через `ScarnaStack`).

Боковой паддинг (`px-4 sm:px-6 lg:px-8`) — часть классов корневого `<header>`, а не
вложенного `ScarnaContainer` (как было раньше) — поэтому переопределяется как у любого
другого компонента, через `!`-модификатор (см. [custom-classes.md](custom-classes.md)):

```razor
<ScarnaNavBar Class="px-0!">...</ScarnaNavBar>
```

## Пример использования

```razor
<ScarnaNavBar Density="ScarnaDensity.Dense" Color="Color.Primary">
    <a href="/" class="text-lg font-semibold">Logo</a>
    <ScarnaStack Row="true" Spacing="6">
        <NavLink href="/">Главная</NavLink>
        <NavLink href="/about">О нас</NavLink>
    </ScarnaStack>
</ScarnaNavBar>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Sticky` | `bool` | `true` | Прилипает к верху при прокрутке |
| `Bordered` | `bool` | `true` | Граница снизу |
| `Density` | `ScarnaDensity` | `Normal` | `Normal` — 56px высотой, `Dense` — 48px |
| `Color` | `Color` | `Default` | `Default` — нейтральная подложка (см. `GlassEffect`); любой другой — сплошная заливка цветом |
| `GlassEffect` | `bool` | `true` | Полупрозрачная подложка с блюром вместо сплошного `bg-content1` (действует только при `Color.Default`). Выключите, чтобы AppBar был того же сплошного цвета, что и `Color.Default` у `ScarnaDrawer` |
| `Justify` | `Justify` | `SpaceBetween` | Распределение содержимого по горизонтали — как AlignItems в MudBlazor |
| `ChildContent` | `RenderFragment?` | — | Содержимое панели |
