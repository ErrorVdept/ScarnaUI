# NorthContainer

## Файлы
- `src/NorthUI/NorthUI/Components/Layout/NorthContainer.razor`
- `src/NorthUI/NorthUI/Components/Layout/NorthLayoutEnums.cs` (`NorthContainerMaxWidth`)

## Описание

Центрирующий контейнер с максимальной шириной и боковыми отступами — типовая обёртка
содержимого страницы (аналог `MudContainer`).

## Пример использования

```razor
<NorthContainer MaxWidth="NorthContainerMaxWidth.Large">
    <h1>Заголовок страницы</h1>
    ...
</NorthContainer>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `MaxWidth` | `NorthContainerMaxWidth` | `Large` | `Small` / `Medium` / `Large` / `ExtraLarge` |
| `Fluid` | `bool` | `false` | `true` — растянуть на всю ширину, игнорируя `MaxWidth` |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
