# NorthNavBar

## Файлы
- `src/NorthUI/NorthUI/Components/Layout/NorthNavBar.razor`

## Описание

Верхняя панель приложения (аналог `MudAppBar`/шапки сайта). Формализует то, что
раньше в демо собиралось вручную из `<header>` + `NorthContainer` + `NorthStack` —
задаёт высоту, подложку и границу снизу; содержимое — произвольное (логотип, ссылки,
действия — расставляйте через `NorthStack`).

## Пример использования

```razor
<NorthNavBar Density="NorthDensity.Dense" Color="NorthColor.Primary">
    <a href="/" class="text-lg font-semibold">Logo</a>
    <NorthStack Row="true" Spacing="6">
        <NavLink href="/">Главная</NavLink>
        <NavLink href="/about">О нас</NavLink>
    </NorthStack>
</NorthNavBar>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Sticky` | `bool` | `true` | Прилипает к верху при прокрутке |
| `Bordered` | `bool` | `true` | Граница снизу |
| `Fluid` | `bool` | `true` | Растянуть содержимое на всю ширину (без `NorthContainer.MaxWidth`) |
| `Density` | `NorthDensity` | `Normal` | `Normal` — 56px высотой, `Dense` — 48px |
| `Color` | `NorthColor` | `Default` | `Default` — нейтральная подложка с блюром; любой другой — сплошная заливка цветом |
| `Justify` | `NorthJustify` | `SpaceBetween` | Распределение содержимого по горизонтали — как AlignItems в MudBlazor |
| `ChildContent` | `RenderFragment?` | — | Содержимое панели |
