# NorthInput

## Файлы
- `src/NorthUI/NorthUI/Components/Forms/NorthInput.razor`
- `src/NorthUI/NorthUI/Components/Base/NorthFormComponentBase.cs` (Value/ValueChanged/ValueExpression, EditContext)

## Описание

Однострочное текстовое поле. Поддерживает `@bind-Value` "из коробки" и подсветку
ошибок валидации, если используется внутри `<EditForm>` (см.
[getting-started.md](getting-started.md#northformcomponentbaset)).

## Пример использования

```razor
<EditForm Model="model" OnValidSubmit="Submit">
    <DataAnnotationsValidator />
    <NorthInput Label="Email" Type="email" @bind-Value="model.Email" HelperText="Мы не передаём его третьим лицам" />
</EditForm>
```

Ручная ошибка без формы:

```razor
<NorthInput Label="Купон" @bind-Value="_coupon"
            Error="@(!_couponValid)" ErrorText="Купон не найден" />
```

Компактное поле для панели фильтров:

```razor
<NorthInput Placeholder="Поиск..." Density="NorthDensity.Dense" @bind-Value="_query" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `string?` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над полем |
| `Placeholder` | `string?` | — | Плейсхолдер |
| `HelperText` | `string?` | — | Подсказка под полем (скрывается при ошибке) |
| `ErrorText` | `string?` | — | Текст ошибки под полем |
| `Error` | `bool` | `false` | Принудительно пометить поле невалидным |
| `Type` | `string` | `"text"` | HTML `type` (`text`/`email`/`password`/`number`...) |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | — |
| `Variant` | `NorthVariant` | `Bordered` | `Bordered` — контур на прозрачном фоне, `Flat` — заливка без контура |
| `Bordered` | `bool` | `true` | Показывать ли контур поля, независимо от Variant (`Flat`+`Bordered="false"` — заливка совсем без обводки) |
| `Size` | `NorthSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Density` | `NorthDensity` | `Normal` | `Normal` — обычная высота, `Dense` — уменьшенная высота/паддинги для плотных панелей фильтров и таблиц |
| `Radius` | `NorthRadius` | `Medium` | Радиус поля |
| `StartContent` | `RenderFragment?` | — | Иконка/префикс перед полем |
| `EndContent` | `RenderFragment?` | — | Иконка/кнопка после поля |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
