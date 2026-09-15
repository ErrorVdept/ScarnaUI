# ScarnaSpinner

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaSpinner.razor`

## Описание

Вращающееся кольцо-индикатор загрузки (indeterminate, без конкретного значения
прогресса). Для линейного прогресса со значением — см. [ScarnaProgress](progress.md)
(там же `Indeterminate="true"` даёт линейный вариант того же смысла). Вращение —
стандартная Tailwind-утилита `animate-spin`, свой CSS не понадобился.

## Пример использования

```razor
<ScarnaSpinner />
<ScarnaSpinner Color="Color.Success" Size="Size.Large" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `Color` | `Primary` | Цвет кольца |
| `Size` | `Size` | `Medium` | `Small`/`Medium`/`Large` |
| `Label` | `string` | `"Загрузка"` | Текст для скринридеров (`aria-label`), визуально не отображается |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
