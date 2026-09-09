# NorthStack

## Файлы
- `src/NorthUI/NorthUI/Components/Layout/NorthStack.razor`
- `src/NorthUI/NorthUI/Components/Layout/NorthLayoutEnums.cs` (`NorthJustify`, `NorthAlign`)

## Описание

Flex-контейнер для расстановки дочерних элементов в ряд или колонку с равным
отступом между ними — самый часто используемый layout-компонент (аналог `MudStack`).

## Пример использования

```razor
<NorthStack Row="true" Spacing="3" Justify="NorthJustify.SpaceBetween" Align="NorthAlign.Center">
    <span>Итого: 1 200 ₽</span>
    <NorthButton Color="NorthColor.Primary">Оформить</NorthButton>
</NorthStack>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Row` | `bool` | `false` | `true` — flex-row, `false` — flex-column |
| `Spacing` | `int` | `2` | Отступ между элементами в шагах по 0.25rem (2 = 0.5rem) |
| `Wrap` | `bool` | `false` | Перенос элементов при нехватке места |
| `Justify` | `NorthJustify` | `Start` | `Start` / `End` / `Center` / `SpaceBetween` / `SpaceAround` / `SpaceEvenly` (набор как у Justify в MudBlazor) |
| `Align` | `NorthAlign` | `Stretch` | `Start` / `End` / `Center` / `Stretch` / `Baseline` (набор как у AlignItems в MudBlazor) |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
