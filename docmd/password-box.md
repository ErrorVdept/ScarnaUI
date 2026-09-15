# ScarnaPasswordBox

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaPasswordBox.razor`

## Описание

Текстовое поле пароля с переключаемым отображением (кнопка-глазок) и настраиваемым
символом маски (`MaskChar`, по умолчанию `*`). Основан на
`ScarnaFormComponentBase<string>` — поддерживает `@bind-Value`.

Не использует нативный `input[type=password]` для скрытого состояния — браузер сам
решает, каким символом рисовать маску (обычно точка), и это никак не настроить через
атрибуты/CSS. Поле всегда `type="text"`, а маскированием занимается сам компонент:
реальный пароль живёт в `Value`, а в DOM идёт строка из `MaskChar` той же длины.

Раз браузер не знает реальных символов, при вводе/удалении компонент сам восстанавливает,
что изменилось: сравнивает новую отображаемую строку со старой маской — совпала длина
⇒ заменили один символ на позиции первого расхождения; выросла ⇒ там же произошла
вставка; уменьшилась ⇒ там же удаление. Как следствие, только что введённый символ на
миг виден как есть до следующего рендера, после которого он тоже маскируется — это
нормальное поведение такого подхода (многие мобильные клавиатуры делают ровно то же).

## Пример использования

```razor
<ScarnaPasswordBox Label="Пароль" @bind-Value="model.Password" />
<ScarnaPasswordBox Label="PIN-код" @bind-Value="model.Pin" MaskChar="'•'" ShowToggle="false" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `string` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над полем |
| `Placeholder` | `string?` | — | — |
| `HelperText` / `ErrorText` / `Error` | — | — | См. [getting-started.md](getting-started.md) |
| `MaskChar` | `char` | `'*'` | Символ маски, пока пароль скрыт |
| `ShowToggle` | `bool` | `true` | Показывать ли кнопку-глазок переключения видимости |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | — |
| `Variant` | `Variant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля |
| `Size` | `Size` | `Medium` | `Small`/`Medium`/`Large` |
| `Density` | `ScarnaDensity` | `Normal` | `Dense` — компактное поле |
| `Radius` | `Radius` | `Medium` | Радиус поля |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
