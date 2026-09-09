# NorthButton

## Файлы
- `src/NorthUI/NorthUI/Components/Actions/NorthButton.razor`
- `src/NorthUI/NorthUI/Components/Base/NorthColorVariantClasses.cs` (таблица цвет × вариант)
- `src/NorthUI/NorthUI/Components/Base/NorthEnums.cs` (`NorthColor`, `NorthSize`, `NorthRadius`, `NorthVariant`)

## Описание

Флагманский компонент библиотеки — на нём виден весь набор дизайн-токенов: цвет,
вариант заливки, размер, радиус. Если задан `Href`, рендерится как `<a>`, иначе —
`<button>`.

## Пример использования

```razor
<NorthButton Color="NorthColor.Primary" Variant="NorthVariant.Solid" OnClick="Save">
    Сохранить
</NorthButton>

<NorthButton Href="/settings" Variant="NorthVariant.Light">Настройки</NorthButton>

<NorthButton Loading="@_isSaving" Disabled="@_isSaving" OnClick="SaveAsync">
    <StartIcon><svg>...</svg></StartIcon>
    Сохранить
</NorthButton>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `NorthColor` | `Default` | `Default`/`Primary`/`Secondary`/`Success`/`Warning`/`Danger` |
| `Variant` | `NorthVariant` | `Solid` | `Solid`/`Flat`/`Bordered`/`Light`/`Faded`/`Shadow` |
| `Size` | `NorthSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Radius` | `NorthRadius` | `Medium` | `None`/`Small`/`Medium`/`Large`/`Full` |
| `Bordered` | `bool` | `false` | Доп. обводка в цвет Color поверх Variant (для Variant.Bordered ничего не меняет) |
| `Type` | `string` | `"button"` | HTML `type` для `<button>` |
| `Href` | `string?` | — | Если задан — рендер как `<a href>` |
| `Target` | `string?` | — | `target` для ссылки |
| `Disabled` | `bool` | `false` | Блокирует клики, гасит непрозрачность |
| `Loading` | `bool` | `false` | Показывает спиннер вместо `StartIcon`, блокирует клики |
| `FullWidth` | `bool` | `false` | Растянуть на всю ширину родителя |
| `StartIcon` | `RenderFragment?` | — | Контент перед текстом |
| `EndIcon` | `RenderFragment?` | — | Контент после текста |
| `OnClick` | `EventCallback<MouseEventArgs>` | — | Обработчик клика (не вызывается, если `Disabled`/`Loading`) |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
