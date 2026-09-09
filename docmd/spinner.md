# NorthSpinner

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthSpinner.razor`

## Описание

Вращающееся кольцо-индикатор загрузки (indeterminate, без конкретного значения
прогресса). Для линейного прогресса со значением — см. [NorthProgress](progress.md)
(там же `Indeterminate="true"` даёт линейный вариант того же смысла). Вращение —
стандартная Tailwind-утилита `animate-spin`, свой CSS не понадобился.

## Пример использования

```razor
<NorthSpinner />
<NorthSpinner Color="NorthColor.Success" Size="NorthSize.Large" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `NorthColor` | `Primary` | Цвет кольца |
| `Size` | `NorthSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Label` | `string` | `"Загрузка"` | Текст для скринридеров (`aria-label`), визуально не отображается |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
