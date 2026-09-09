# NorthPopoverProvider / NorthPopover

## Файлы
- `src/NorthUI/NorthUI/Components/Overlays/NorthPopoverProvider.razor` + `.razor.cs`
- `src/NorthUI/NorthUI/Components/Overlays/NorthPopover.razor`
- `src/NorthUI/NorthUI/Components/Overlays/NorthPopoverEnums.cs` (`NorthPosition`, внутренние `NorthPopoverSide`/`NorthPopoverAlign`)
- `src/NorthUI/NorthUI/wwwroot/js/north/popover.js` — расчёт позиции (flip/shift), подписки на outside-click/Escape/scroll/resize

## Описание

Низкоуровневый примитив позиционирования "плавающей поверхности" относительно
якорного элемента. Сам по себе в разметке приложения почти не используется —
на нём построены [NorthTooltip](tooltip.md), [NorthMenu](menu.md), [NorthSelect](select.md),
а в будущем — Autocomplete, DatePicker/TimePicker/ColorPicker (см. `docmd/index.md`,
раздел "Что дальше").

## Принцип работы

`NorthPopoverProvider` кладётся один раз в layout, оборачивая всё приложение:

```razor
<NorthPopoverProvider>
    @Body
</NorthPopoverProvider>
```

Он делает две вещи:
1. Каскадирует сам себя — любой `NorthPopover` в дереве находит его через
   `[CascadingParameter]`.
2. Рендерит **портал** — `<div class="fixed inset-0 ...">` в самом конце своей
   разметки, куда физически попадает содержимое всех открытых поповеров.

Когда `NorthPopover.Open` становится `true`, он не рисует содержимое сам, а
регистрирует свой `RenderFragment` в провайдере (`Provider.Register(id, fragment)`).
Blazor рендерит этот фрагмент **в дереве провайдера**, а не там, где стоит
`<NorthPopover>` в разметке — получается настоящий портал средствами самого Blazor,
без прямых манипуляций с DOM.

Позиционирование (сторона+выравнивание одним значением `Position`, flip при нехватке
места, shift, чтобы не вылезать за экран) делает `popover.js` — простая независимая
реализация без внешних библиотек (`getBoundingClientRect` + `position: fixed`).

> Ограничение: поповер не переносится в DOM физически (в отличие от `MudPopoverProvider`),
> он остаётся в обычном месте дерева компонентов с `position: fixed`. Это покрывает
> подавляющее большинство случаев, но если якорь находится внутри контейнера с
> CSS `transform`/`filter` (создаёт новый containing block), позиционирование может
> съехать — в таком случае переносите триггер поповера за пределы такого контейнера.

### Исправленный баг: "меню открывается не под собой, встаёт на место после скролла"

Причина была в порядке операций: `popoverEl.getBoundingClientRect()` вызывался
**до** простановки `position: fixed`. Пока элемент ещё в обычном потоке документа
(блочный `<div>` внутри `position:fixed`-портала), он растягивается на всю ширину
портала, и текст внутри переносится иначе — измеренные `width`/`height` оказывались
неверными. Позиция вычислялась по этим неверным размерам, а после скролла/resize
(когда `position:fixed` уже стоял с предыдущего вызова) пересчёт внезапно давал
верный результат — поповер "запрыгивал" на место. Исправлено: `position:fixed`
теперь прописан прямо в разметке `NorthPopover.razor` (`Surface()`), а не
выставляется из JS позже — первое же измерение уже верное. Дополнительно
`ResizeObserver` следит за размером поповера и якоря на случай, если контент
изменится уже после открытия (асинхронная загрузка и т.п.).

### Устойчивость к нулевому viewport

`popover.js` также защищён от вырожденного случая, когда `window.innerWidth`/`innerHeight`
временно равны 0 (скрытая/фоновая вкладка в момент открытия) — раньше это тоже
приводило к отрицательным координатам. Теперь при отсутствующем
`innerWidth`/`innerHeight` используется `document.documentElement.clientWidth/clientHeight`,
а итоговая позиция всегда прижимается к видимой области с отступом (never negative
beyond margin). Также исправлена логика flip: если перевёрнутая сторона тоже не
помещается целиком, выбирается та, где вылезание за край **меньше**, а не слепо исходная.

## Пример использования (внутри своего компонента)

```razor
<div @ref="_anchor" @onclick="() => _open = !_open">Открыть меню</div>

<NorthPopover Open="_open" Anchor="_anchor" Position="NorthPosition.BottomStart"
              OnClose="() => _open = false">
    <div class="p-2 bg-content1 rounded-north-md shadow-lg">Содержимое</div>
</NorthPopover>

@code {
    private ElementReference _anchor;
    private bool _open;
}
```

## Параметры NorthPopover

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Open` | `bool` | — | Открыт ли поповер |
| `Anchor` | `ElementReference` | — | **Обязательный.** Якорный элемент (захватывается вызывающим компонентом через `@ref`) |
| `ChildContent` | `RenderFragment?` | — | Содержимое поповера |
| `Position` | `NorthPosition` | `BottomStart` | Единое свойство позиции — сторона + выравнивание (см. ниже) |
| `IsParentFullSize` | `bool` | `false` | Растянуть поверхность на ширину якоря (родителя) — например, чтобы выпадающий список был не уже поля |
| `OffsetPx` | `int` | `8` | Отступ от якоря в пикселях |
| `CloseOnOutsideClick` | `bool` | `true` | Закрывать по клику вне поповера/якоря |
| `SurfaceClass` | `string?` | — | Доп. CSS-классы на контейнер поверхности |
| `OnClose` | `EventCallback` | — | Клик снаружи или Escape — сам поповер себя не закрывает, решение остаётся за вызывающим кодом |

## NorthPosition

Единая позиция вместо двух отдельных параметров (сторона + выравнивание) — 12 значений,
как в floating-ui/Radix. Используется в `NorthPopover`, `NorthTooltip`, `NorthMenu`, `NorthSelect`.

```
TopStart     Top     TopEnd
LeftStart  ┌─────────┐  RightStart
Left       │  якорь  │  Right
LeftEnd    └─────────┘  RightEnd
BottomStart  Bottom  BottomEnd
```

`Start`/`End` — относительно направления письма (в LTR: Start = слева/сверху для
горизонтальных/вертикальных сторон соответственно). Например, `TopStart` — "сверху слева",
`BottomEnd` — "снизу справа".
