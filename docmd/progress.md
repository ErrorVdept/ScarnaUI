# NorthProgress

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthProgress.razor`
- `src/NorthUI/NorthUI/Styles/input.css` (`--animate-north-progress-indeterminate`, keyframes)

## Описание

Линейная полоса прогресса — аналог `MudProgressLinear`. Два режима: **Determinate**
(по умолчанию) — конкретное значение `Value`/`Max`, полоса заполняется плавно
(`transition-[width]`); **Indeterminate** — бегущая полоса для операций неизвестной
длительности (CSS keyframe-анимация, JS не нужен).

## Пример использования

```razor
<NorthProgress Label="Загрузка файла" Value="64" ShowValue="true" />
<NorthProgress Label="Синхронизация" Indeterminate="true" Color="NorthColor.Secondary" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Label` | `string?` | — | Подпись над полосой |
| `Value` | `double` | `0` | Текущее значение (игнорируется при `Indeterminate`) |
| `Max` | `double` | `100` | Максимум |
| `Indeterminate` | `bool` | `false` | Бегущая полоса вместо конкретного значения |
| `ShowValue` | `bool` | `false` | Показывать процент справа от подписи |
| `Color` | `NorthColor` | `Primary` | Цвет заливки |
| `Size` | `NorthSize` | `Medium` | Высота полосы: `Small` (4px) / `Medium` (8px) / `Large` (12px) |
