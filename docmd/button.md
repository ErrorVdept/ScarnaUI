# ScarnaButton

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Actions/ScarnaButton.razor`
- `src/ScarnaUI/ScarnaUI/Components/Base/ScarnaColorVariantClasses.cs` (таблица цвет × вариант)
- `src/ScarnaUI/ScarnaUI/Components/Base/ScarnaEnums.cs` (`ScarnaColor`, `ScarnaSize`, `ScarnaRadius`, `ScarnaVariant`)

## Описание

Флагманский компонент библиотеки — на нём виден весь набор дизайн-токенов: цвет,
вариант заливки, размер, радиус. Если задан `Href`, рендерится как `<a>`, иначе —
`<button>`.

## Пример использования

```razor
<ScarnaButton Color="ScarnaColor.Primary" Variant="ScarnaVariant.Solid" OnClick="Save">
    Сохранить
</ScarnaButton>

<ScarnaButton Href="/settings" Variant="ScarnaVariant.Light">Настройки</ScarnaButton>

<ScarnaButton Loading="@_isSaving" Disabled="@_isSaving" OnClick="SaveAsync">
    <StartIcon><svg>...</svg></StartIcon>
    Сохранить
</ScarnaButton>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `ScarnaColor` | `Default` | `Default`/`Primary`/`Secondary`/`Success`/`Warning`/`Danger` |
| `Variant` | `ScarnaVariant` | `Solid` | `Solid`/`Flat`/`Bordered`/`Light`/`Faded`/`Shadow` |
| `Size` | `ScarnaSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Radius` | `ScarnaRadius` | `Medium` | `None`/`Small`/`Medium`/`Large`/`Full` |
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
