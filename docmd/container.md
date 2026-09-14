# ScarnaContainer

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaContainer.razor`
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaLayoutEnums.cs` (`ScarnaContainerMaxWidth`)

## Описание

Центрирующий контейнер с максимальной шириной и боковыми отступами — типовая обёртка
содержимого страницы (аналог `MudContainer`).

## Пример использования

```razor
<ScarnaContainer MaxWidth="ScarnaContainerMaxWidth.Large">
    <h1>Заголовок страницы</h1>
    ...
</ScarnaContainer>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `MaxWidth` | `ScarnaContainerMaxWidth` | `Large` | `Small` / `Medium` / `Large` / `ExtraLarge` |
| `Fluid` | `bool` | `false` | `true` — растянуть на всю ширину, игнорируя `MaxWidth` |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
