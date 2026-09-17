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
| `Variant` | `Variant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля, независимо от Variant |
| `Size` | `Size` | `Medium` | `Small`/`Medium`/`Large` |
| `Density` | `ScarnaDensity` | `Normal` | `Dense` — компактное поле |
| `Radius` | `Radius` | `Medium` | Радиус поля |
| `Position` | `ScarnaPosition` | `BottomStart` | Позиция выпадающего списка (см. [popover.md](popover.md#scarnaposition)) |
| `IsParentFullSize` | `bool` | `true` | Растянуть список на ширину поля (не уже `min-w-[220px]`) |
| `ChildContent` | `RenderFragment?` | — | Набор `ScarnaSelectItem` |

## Параметры ScarnaSelectItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` | `TValue` | — | **Обязательный.** Значение пункта |
| `Text` | `string` | — | **Обязательный.** Текст пункта — используется и в списке, и как подпись в закрытом поле после выбора |
| `ChildContent` | `RenderFragment?` | — | Кастомная разметка пункта (иконка и т.п.) вместо простого текста |

## Как определяется подпись закрытого поля

`ScarnaPopover` физически монтирует своё содержимое только пока открыт (см.
[popover.md](popover.md)) — сам по себе `ScarnaSelect` не смог бы узнать текст
выбранного пункта до первого открытия списка. Поэтому помимо содержимого внутри
поповера `ScarnaSelect` держит его же вторую, скрытую (`display:none`) копию,
смонтированную всегда — она и даёт подпись сразу, включая случай, когда `Value`
задан программно (например, предзаполнение формы данными с сервера) ещё до того,
как пользователь хоть раз открыл список.
