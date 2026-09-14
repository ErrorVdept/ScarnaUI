# ScarnaSelect / ScarnaSelectItem

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaSelect.razor`
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaSelectItem.razor`

## Описание

Выпадающий список с одиночным выбором. Построен на [ScarnaPopover](popover.md) и
`ScarnaFormComponentBase<T>` — поддерживает `@bind-Value` как любое другое поле формы
(см. [getting-started.md](getting-started.md#scarnaformcomponentbaset)). Требует
`ScarnaPopoverProvider` в дереве выше.

Компонент — дженерик (`ScarnaSelect<TValue>`), но в разметке почти никогда не нужно
писать `TValue` явно: тип выводится автоматически из значений `Value` у вложенных
`ScarnaSelectItem` через `[CascadingTypeParameter]`.

## Пример использования

```razor
<ScarnaSelect Label="Страна" @bind-Value="model.CountryCode" Placeholder="Выберите страну">
    <ScarnaSelectItem Value="@("ru")" Text="Россия" />
    <ScarnaSelectItem Value="@("kz")" Text="Казахстан" />
    <ScarnaSelectItem Value="@("by")" Text="Беларусь" />
</ScarnaSelect>
```

Работает и с enum/числовыми значениями — TValue выводится точно так же:

```razor
<ScarnaSelect @bind-Value="model.Priority">
    <ScarnaSelectItem Value="Priority.Low" Text="Низкий" />
    <ScarnaSelectItem Value="Priority.High" Text="Высокий" />
</ScarnaSelect>
```

## Параметры ScarnaSelect

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `TValue?` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над полем |
| `Placeholder` | `string?` | `"Выберите..."` | Текст, пока ничего не выбрано |
| `HelperText` | `string?` | — | Подсказка под полем |
| `ErrorText` / `Error` | — | — | См. [getting-started.md](getting-started.md) |
| `ReadOnly` | `bool` | `false` | Запрещает открытие списка (клик по полю игнорируется), но, в отличие от `Disabled`, поле визуально не тускнеет |
| `Variant` | `ScarnaVariant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля, независимо от Variant |
| `Size` | `ScarnaSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Density` | `ScarnaDensity` | `Normal` | `Dense` — компактное поле |
| `Radius` | `ScarnaRadius` | `Medium` | Радиус поля |
| `Position` | `ScarnaPosition` | `BottomStart` | Позиция выпадающего списка (см. [popover.md](popover.md#scarnaposition)) |
| `IsParentFullSize` | `bool` | `true` | Растянуть список на ширину поля (не уже `min-w-[220px]`) |
| `ChildContent` | `RenderFragment?` | — | Набор `ScarnaSelectItem` |

## Параметры ScarnaSelectItem

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
