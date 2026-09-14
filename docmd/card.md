# ScarnaCard

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaCard.razor`

## Описание

Контейнер-карточка с фоном `content1` (см. [theme-provider.md](theme-provider.md)),
скруглением и опциональными заголовком/подвалом.

## Пример использования

```razor
<ScarnaCard>
    <HeaderContent><span class="font-semibold">Заказ #1234</span></HeaderContent>
    Товар: Кроссовки, размер 42. Статус: в пути.
    <FooterContent><ScarnaButton Size="ScarnaSize.Small">Подробнее</ScarnaButton></FooterContent>
</ScarnaCard>
```

> Если контента для заголовка/подвала нет, просто не указывайте `HeaderContent`/
> `FooterContent` — тогда всё содержимое `ScarnaCard` должно идти как обычный дочерний
> контент, без отдельных именованных тегов рядом (см. пример на странице `/` в
> `ScarnaUI.Demo`).

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Bordered` | `bool` | `false` | Тонкая рамка в дополнение к тени |
| `Shadow` | `bool` | `true` | Тень под карточкой |
| `Radius` | `ScarnaRadius` | `Large` | Радиус скругления |
| `HeaderContent` | `RenderFragment?` | — | Заголовок (с разделителем снизу) |
| `FooterContent` | `RenderFragment?` | — | Подвал (с разделителем сверху) |
| `ChildContent` | `RenderFragment?` | — | Основное содержимое |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
