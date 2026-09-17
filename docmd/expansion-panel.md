# ScarnaExpansionPanel

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaExpansionPanel.razor`

## Описание

Сворачиваемая панель (аккордеон-секция). Каждая панель независима — если нужно,
чтобы одновременно была открыта только одна, храните "какая панель открыта" во
внешнем состоянии родителя и биндите `Expanded` у каждой панели к этому состоянию.

Высота анимируется CSS grid-трюком (`grid-template-rows: 0fr → 1fr`) — плавно, без
замера высоты через JS.

## Пример использования

```razor
<ScarnaExpansionPanel Title="Доставка">
    Бесплатно при заказе от 3000 ₽, курьером 1-2 дня.
</ScarnaExpansionPanel>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Title` | `string?` | — | Текстовый заголовок |
| `TitleContent` | `RenderFragment?` | — | Заголовок сложной разметкой (вместо `Title`) |
| `Expanded` | `bool` | `false` | Развёрнута ли панель. Поддерживает `@bind-Expanded` |
| `Bordered` | `bool` | `true` | Обводка панели |
| `Color` | `Color` | `Default` | Фон панели. `Default` — нейтральная поверхность темы (`bg-content1`); `Transparent` — без заливки; остальные — заливка цветом темы |
| `ChildContent` | `RenderFragment?` | — | Содержимое панели |
