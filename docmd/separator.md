# ScarnaSeparator

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaSeparator.razor`

## Описание

Разделитель. Без содержимого — то же самое, что [ScarnaDivider](divider.md) (тонкая
линия цвета `divider`). С `ChildContent` — линия-подпись-линия ("или", дата и т.п.).
Оставлен отдельным компонентом (а не параметром у Divider), потому что с подписью
меняется сама разметка (`flex` из трёх частей вместо одного `div`).

## Пример использования

```razor
<ScarnaSeparator />
<ScarnaSeparator>или</ScarnaSeparator>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Vertical` | `bool` | `false` | Вертикальная линия вместо горизонтальной. Игнорируется, если задан `ChildContent` |
| `ChildContent` | `RenderFragment?` | — | Подпись посередине линии |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
