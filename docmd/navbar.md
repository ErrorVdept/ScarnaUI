# ScarnaNavBar

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaNavBar.razor`

## Описание

Верхняя панель приложения (аналог `MudAppBar`/шапки сайта). Формализует то, что
раньше в демо собиралось вручную из `<header>` + `ScarnaContainer` + `ScarnaStack` —
задаёт высоту, подложку и границу снизу; содержимое — произвольное (логотип, ссылки,
действия — расставляйте через `ScarnaStack`).

## Пример использования

```razor
<ScarnaNavBar Density="ScarnaDensity.Dense" Color="ScarnaColor.Primary">
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
| `Fluid` | `bool` | `true` | Растянуть содержимое на всю ширину (без `ScarnaContainer.MaxWidth`) |
| `Density` | `ScarnaDensity` | `Normal` | `Normal` — 56px высотой, `Dense` — 48px |
| `Color` | `ScarnaColor` | `Default` | `Default` — нейтральная подложка с блюром; любой другой — сплошная заливка цветом |
| `Justify` | `ScarnaJustify` | `SpaceBetween` | Распределение содержимого по горизонтали — как AlignItems в MudBlazor |
| `ChildContent` | `RenderFragment?` | — | Содержимое панели |
