# NorthTable

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthTable.razor`

## Описание

Обёртка стилизации над обычной HTML-таблицей — аналог `MudSimpleTable`. Разметку
(`<thead>`/`<tbody>`/`<tr>`/`<th>`/`<td>`) вы пишете сами как обычный HTML — NorthTable
не подменяет её своими компонентами (в отличие от `MudTable`), а лишь добавляет
оформление в духе HeroUI Table через Tailwind descendant-селекторы
(`[&_thead_th]:...`, `[&_tbody_tr:hover]:...`) на обёртке.

## Пример использования

```razor
<NorthTable Hover="true" Striped="false">
    <thead>
        <tr><th>Имя</th><th>Email</th><th>Статус</th></tr>
    </thead>
    <tbody>
        <tr>
            <td>Иван Петров</td>
            <td>ivan@example.com</td>
            <td><NorthChip Color="NorthColor.Success" Size="NorthSize.Small">Активен</NorthChip></td>
        </tr>
    </tbody>
</NorthTable>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Hover` | `bool` | `true` | Подсветка строки `<tbody>` при наведении |
| `Striped` | `bool` | `false` | Зебра — чередующийся фон чётных строк |
| `Bordered` | `bool` | `true` | Обводка контейнера таблицы |
| `DividedColumns` | `bool` | `false` | Вертикальные разделители между колонками (кроме последней) |
| `Density` | `NorthDensity` | `Normal` | `Dense` — компактные отступы ячеек, для больших наборов данных |
| `ChildContent` | `RenderFragment?` | — | Произвольная разметка `<thead>`/`<tbody>` |

## Технический нюанс

Поскольку `<thead>`/`<tr>`/`<td>` пишет вызывающий код, а не сама библиотека, применить
к ним Tailwind-классы напрямую нельзя — вместо этого используются arbitrary
descendant-варианты Tailwind (`[&_thead_th]:uppercase` и т.п.) прямо на `<table>`,
которые находят нужные потомки через обычный CSS-селектор, не требуя контроля над
их разметкой.
