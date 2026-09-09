# NorthSelect / NorthSelectItem

## Файлы
- `src/NorthUI/NorthUI/Components/Forms/NorthSelect.razor`
- `src/NorthUI/NorthUI/Components/Forms/NorthSelectItem.razor`

## Описание

Выпадающий список с одиночным выбором. Построен на [NorthPopover](popover.md) и
`NorthFormComponentBase<T>` — поддерживает `@bind-Value` как любое другое поле формы
(см. [getting-started.md](getting-started.md#northformcomponentbaset)). Требует
`NorthPopoverProvider` в дереве выше.

Компонент — дженерик (`NorthSelect<TValue>`), но в разметке почти никогда не нужно
писать `TValue` явно: тип выводится автоматически из значений `Value` у вложенных
`NorthSelectItem` через `[CascadingTypeParameter]`.

## Пример использования

```razor
<NorthSelect Label="Страна" @bind-Value="model.CountryCode" Placeholder="Выберите страну">
    <NorthSelectItem Value="@("ru")" Text="Россия" />
    <NorthSelectItem Value="@("kz")" Text="Казахстан" />
    <NorthSelectItem Value="@("by")" Text="Беларусь" />
</NorthSelect>
```

Работает и с enum/числовыми значениями — TValue выводится точно так же:

```razor
<NorthSelect @bind-Value="model.Priority">
    <NorthSelectItem Value="Priority.Low" Text="Низкий" />
    <NorthSelectItem Value="Priority.High" Text="Высокий" />
</NorthSelect>
```

## Параметры NorthSelect

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `TValue?` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над полем |
| `Placeholder` | `string?` | `"Выберите..."` | Текст, пока ничего не выбрано |
| `HelperText` | `string?` | — | Подсказка под полем |
| `ErrorText` / `Error` | — | — | См. [getting-started.md](getting-started.md) |
| `ReadOnly` | `bool` | `false` | Запрещает открытие списка (клик по полю игнорируется), но, в отличие от `Disabled`, поле визуально не тускнеет |
| `Variant` | `NorthVariant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля, независимо от Variant |
| `Size` | `NorthSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Density` | `NorthDensity` | `Normal` | `Dense` — компактное поле |
| `Radius` | `NorthRadius` | `Medium` | Радиус поля |
| `Position` | `NorthPosition` | `BottomStart` | Позиция выпадающего списка (см. [popover.md](popover.md#northposition)) |
| `IsParentFullSize` | `bool` | `true` | Растянуть список на ширину поля (не уже `min-w-[220px]`) |
| `ChildContent` | `RenderFragment?` | — | Набор `NorthSelectItem` |

## Параметры NorthSelectItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` | `TValue` | — | **Обязательный.** Значение пункта |
| `Text` | `string` | — | **Обязательный.** Текст пункта — используется и в списке, и как подпись в закрытом поле после выбора |
| `ChildContent` | `RenderFragment?` | — | Кастомная разметка пункта (иконка и т.п.) вместо простого текста |

## Известное ограничение

Подпись закрытого поля берётся из текста выбранного пункта. Она кэшируется в момент
клика (или при первом открытии, если `Value` уже был задан программно) — потому что
список пунктов физически размонтируется, когда поповер закрыт (см.
["Ограничение" в popover.md](popover.md)). Практическое следствие: если задать `Value`
программно (например, предзаполнить форму данными с сервера) **до того, как
пользователь хотя бы раз открыл список**, поле временно покажет `Placeholder` вместо
подписи — до первого открытия. Для большинства сценариев (пользователь сам выбирает
из списка) это не проявляется.
