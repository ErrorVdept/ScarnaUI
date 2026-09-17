# ScarnaContainer

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaContainer.razor`
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaLayoutEnums.cs` (`ScarnaContainerMaxWidth`)

## Описание

Центрирующий контейнер с максимальной шириной и боковыми отступами — типовая обёртка
содержимого страницы (аналог `MudContainer`).

## Пример использования

```razor
<ScarnaContainer Breakpoint="ScarnaContainerMaxWidth.Large">
    <h1>Заголовок страницы</h1>
    ...
</ScarnaContainer>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Breakpoint` | `ScarnaContainerMaxWidth` | `Large` | `Small` / `Medium` / `Large` / `ExtraLarge` — пресет максимальной ширины |
| `Fluid` | `bool` | `false` | `true` — растянуть на всю ширину, игнорируя `Breakpoint` |
| `Class`, `Style`, `UserAttributes`, `MaxWidth`, ... | — | — | см. [getting-started.md](getting-started.md) — `MaxWidth` здесь означает точное CSS-значение (унаследован из `ScarnaComponentBase`), не пресет |
