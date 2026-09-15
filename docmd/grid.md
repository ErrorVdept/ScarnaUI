# ScarnaGrid / ScarnaItem

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaGrid.razor`
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaItem.razor`

## Описание

Классическая 12-колоночная сетка на CSS Grid (аналог `MudGrid`/`MudItem`,
Bootstrap-grid). `ScarnaGrid` — контейнер (`grid-template-columns: repeat(12, ...)`),
`ScarnaItem` — ячейка, задающая, сколько из 12 колонок занять на каждой ширине экрана.

## Пример использования

```razor
<ScarnaGrid Spacing="4">
    <ScarnaItem Xs="12" Md="6"><ScarnaCard>Левая колонка</ScarnaCard></ScarnaItem>
    <ScarnaItem Xs="12" Md="6"><ScarnaCard>Правая колонка</ScarnaCard></ScarnaItem>
</ScarnaGrid>
```

## Параметры ScarnaGrid

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Spacing` | `int` | `4` | Отступ между ячейками в шагах по 0.25rem |
| `Justify` | `Justify` | `Start` | `justify-content` сетки (заметно, только если ячейки не заполняют всю ширину) — как Justify в MudBlazor |
| `Align` | `Align` | `Stretch` | `align-items` — вертикальное выравнивание содержимого ячеек — как AlignItems в MudBlazor |

## Параметры ScarnaItem

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Xs` | `int` (1-12) | `12` | Ширина по умолчанию (мобильные и выше) |
| `Sm` | `int?` | — | Ширина от 640px |
| `Md` | `int?` | — | Ширина от 768px |
| `Lg` | `int?` | — | Ширина от 1024px |

## Технический нюанс

Классы вида `md:col-span-6` должны быть написаны в исходниках буквально — Tailwind
находит их статическим сканированием, а не выполняя код. Поэтому `ScarnaItem.razor`
переводит число в класс через явный `switch` на каждый брейкпоинт, а не через
интерполяцию строки (`$"md:col-span-{n}"` в CSS никогда бы не попал). Подробнее — в
[theme-provider.md](theme-provider.md#почему-классы-написаны-буквально). Брейкпоинт
`Xl` в этой версии не реализован (легко добавить по аналогии в `ScarnaItem.razor`).
