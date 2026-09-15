# ScarnaSelectGroup / ScarnaSelectGroupItem

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaSelectGroup.razor`
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaSelectGroupItem.razor`
- `src/ScarnaUI/ScarnaUI/wwwroot/js/scarna/tabs.js` (переиспользуется для скользящего индикатора)

## Описание

Тот же функционал, что у [ScarnaSelect](select.md) (одиночный выбор, `@bind-Value`,
дженерик с автовыводом `TValue`), но без выпадающего списка — оформлен как
[ScarnaTabs](tabs.md): все варианты видны сразу, pill-индикатор плавно "переезжает"
между кнопками. Хорош для короткого списка из 2-5 вариантов (период, единицы
измерения, режим отображения). Для длинных списков используйте `ScarnaSelect`.

## Пример использования

```razor
<ScarnaSelectGroup Label="Период" @bind-Value="_period">
    <ScarnaSelectGroupItem Value="@("day")" Text="День" />
    <ScarnaSelectGroupItem Value="@("week")" Text="Неделя" />
    <ScarnaSelectGroupItem Value="@("month")" Text="Месяц" />
</ScarnaSelectGroup>
```

## Параметры ScarnaSelectGroup

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `TValue?` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над группой |
| `Justify` | `Justify` | `Start` | К какому краю прижимать кнопки (заметно, только если `FullWidth="false"`) |
| `FullWidth` | `bool` | `false` | Растянуть кнопки на всю ширину родителя поровну — например, `<ScarnaSelectGroup FullWidth="true">` |
| `ChildContent` | `RenderFragment?` | — | Набор `ScarnaSelectGroupItem` |

## Параметры ScarnaSelectGroupItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` | `TValue` | — | **Обязательный.** Значение пункта |
| `Text` | `string` | — | **Обязательный.** Текст на кнопке |
