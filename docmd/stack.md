# ScarnaStack

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaStack.razor`
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaLayoutEnums.cs` (`Justify`, `Align`)

## Описание

Flex-контейнер для расстановки дочерних элементов в ряд или колонку с равным
отступом между ними — самый часто используемый layout-компонент (аналог `MudStack`).

## Пример использования

```razor
<ScarnaStack Row="true" Spacing="3" Justify="Justify.SpaceBetween" Align="Align.Center">
    <span>Итого: 1 200 ₽</span>
    <ScarnaButton Color="Color.Primary">Оформить</ScarnaButton>
</ScarnaStack>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Row` | `bool` | `false` | `true` — flex-row, `false` — flex-column |
| `Spacing` | `int` | `2` | Отступ между элементами в шагах по 0.25rem (2 = 0.5rem) |
| `Wrap` | `bool` | `false` | Перенос элементов при нехватке места |
| `Justify` | `Justify` | `Start` | `Start` / `End` / `Center` / `SpaceBetween` / `SpaceAround` / `SpaceEvenly` (набор как у Justify в MudBlazor) |
| `Align` | `Align` | `Stretch` | `Start` / `End` / `Center` / `Stretch` / `Baseline` (набор как у AlignItems в MudBlazor) |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
