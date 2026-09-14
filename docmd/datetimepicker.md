# ScarnaDateTimePicker

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Forms/ScarnaDateTimePicker.razor`

## Описание

Поле выбора даты и времени. Под капотом — нативный `input[type=datetime-local]`
(браузерный календарь + time-picker бесплатно, тот же подход, что у
[ScarnaTimeField](timefield.md)). Основан на `ScarnaFormComponentBase<DateTime?>` —
формат `"yyyy-MM-ddTHH:mm"`, который ожидает атрибут `value`, собирается/парсится
внутри компонента.

Полноценный кастомный попап-календарь (с собственной сеткой дней, как в
MudDatePicker) в этом проходе не строился — нативный контрол закрывает 90% случаев
без единой строчки JS и с полной поддержкой клавиатуры/доступности из коробки.

## Пример использования

```razor
<ScarnaDateTimePicker Label="Дата и время встречи" @bind-Value="model.MeetingAt" />
<ScarnaDateTimePicker Label="Срок" @bind-Value="model.Deadline" Min="DateTime.Today" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `DateTime?` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над полем |
| `HelperText` / `ErrorText` / `Error` | — | — | См. [getting-started.md](getting-started.md) |
| `Min` / `Max` | `DateTime?` | — | Границы диапазона (нативная валидация браузера) |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | — |
| `Variant` | `ScarnaVariant` | `Bordered` | `Bordered`/`Flat` |
| `Bordered` | `bool` | `true` | Показывать ли контур поля |
| `Size` | `ScarnaSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Density` | `ScarnaDensity` | `Normal` | `Dense` — компактное поле |
| `Radius` | `ScarnaRadius` | `Medium` | Радиус поля |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
