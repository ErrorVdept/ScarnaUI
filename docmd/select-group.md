# NorthSelectGroup / NorthSelectGroupItem

## Файлы
- `src/NorthUI/NorthUI/Components/Forms/NorthSelectGroup.razor`
- `src/NorthUI/NorthUI/Components/Forms/NorthSelectGroupItem.razor`
- `src/NorthUI/NorthUI/wwwroot/js/north/tabs.js` (переиспользуется для скользящего индикатора)

## Описание

Тот же функционал, что у [NorthSelect](select.md) (одиночный выбор, `@bind-Value`,
дженерик с автовыводом `TValue`), но без выпадающего списка — оформлен как
[NorthTabs](tabs.md): все варианты видны сразу, pill-индикатор плавно "переезжает"
между кнопками. Хорош для короткого списка из 2-5 вариантов (период, единицы
измерения, режим отображения). Для длинных списков используйте `NorthSelect`.

## Пример использования

```razor
<NorthSelectGroup Label="Период" @bind-Value="_period">
    <NorthSelectGroupItem Value="@("day")" Text="День" />
    <NorthSelectGroupItem Value="@("week")" Text="Неделя" />
    <NorthSelectGroupItem Value="@("month")" Text="Месяц" />
</NorthSelectGroup>
```

## Параметры NorthSelectGroup

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `TValue?` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над группой |
| `Justify` | `NorthJustify` | `Start` | К какому краю прижимать кнопки (заметно, только если `FullWidth="false"`) |
| `FullWidth` | `bool` | `false` | Растянуть кнопки на всю ширину родителя поровну — например, `<NorthSelectGroup FullWidth="true">` |
| `ChildContent` | `RenderFragment?` | — | Набор `NorthSelectGroupItem` |

## Параметры NorthSelectGroupItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` | `TValue` | — | **Обязательный.** Значение пункта |
| `Text` | `string` | — | **Обязательный.** Текст на кнопке |
