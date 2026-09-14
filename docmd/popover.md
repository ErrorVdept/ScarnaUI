# ScarnaPopoverProvider / ScarnaPopover

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Overlays/ScarnaPopoverProvider.razor` + `.razor.cs`
- `src/ScarnaUI/ScarnaUI/Components/Overlays/ScarnaPopover.razor`
- `src/ScarnaUI/ScarnaUI/Components/Overlays/ScarnaPopoverEnums.cs` (`ScarnaPosition`, внутренние `ScarnaPopoverSide`/`ScarnaPopoverAlign`)
- `src/ScarnaUI/ScarnaUI/wwwroot/js/scarna/popover.js` — расчёт позиции (flip/shift), подписки на outside-click/Escape/scroll/resize

## Описание

Низкоуровневый примитив позиционирования "плавающей поверхности" относительно
якорного элемента. Сам по себе в разметке приложения почти не используется —
на нём построены [ScarnaTooltip](tooltip.md), [ScarnaMenu](menu.md), [ScarnaSelect](select.md),
а в будущем — Autocomplete, DatePicker/TimePicker/ColorPicker (см. `docmd/index.md`,
раздел "Что дальше").

## Принцип работы

`ScarnaPopoverProvider` кладётся один раз в layout, оборачивая всё приложение:

```razor
<ScarnaPopoverProvider>
    @Body
</ScarnaPopoverProvider>
```

Он делает две вещи:
1. Каскадирует сам себя — любой `ScarnaPopover` в дереве находит его через
   `[CascadingParameter]`.
2. Рендерит **портал** — `<div class="fixed inset-0 ...">` в самом конце своей
   разметки, куда физически попадает содержимое всех открытых поповеров.

Когда `ScarnaPopover.Open` становится `true`, он не рисует содержимое сам, а
регистрирует свой `RenderFragment` в провайдере (`Provider.Register(id, fragment)`).
Blazor рендерит этот фрагмент **в дереве провайдера**, а не там, где стоит
`<ScarnaPopover>` в разметке — получается настоящий портал средствами самого Blazor,
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
теперь прописан прямо в разметке `ScarnaPopover.razor` (`Surface()`), а не
выставляется из JS позже — первое же измерение уже верное. Дополнительно
`ResizeObserver` следит за размером поповера и якоря на случай, если контент
изменится уже после открытия (асинхронная загрузка и т.п.).

### Исправленный баг: поповер не открывается, если его триггер лежит прямо в MainLayout

Проявлялось так: `ScarnaMenu`/любой другой компонент на `ScarnaPopover`, помещённый
**прямо в разметку layout'а** (например, выпадающее меню в шапке сайта — не через
`@Body`, а буквально в `MainLayout.razor`), не открывался вообще — клик по триггеру
доходил до C# (`_open` менялся на `true`, проверено логированием), но содержимое
поповера так и не появлялось в DOM. Тот же самый компонент с тем же кодом прекрасно
открывался, если его перенести в содержимое страницы (`@Body`).

Причина — бесконечный цикл повторного рендера, который в большинстве раскладок дерева
компонентов Blazor успевал "погаситься" сам, но не в этой. `ScarnaPopover.OnParametersSet`
раньше вызывал `Provider.Register(id, fragment)` **на каждый вызов, пока `Open == true`**
(не только при переходе false→true — специально, чтобы поповер подхватывал изменения
своего содержимого, пока открыт). `Register` дёргает `Provider.StateHasChanged()`, а это
перерисовывает **весь** `ChildContent` провайдера — то есть буквально всё, что обёрнуто
`<ScarnaPopoverProvider>...</ScarnaPopoverProvider>` в layout'е. Если якорь поповера сам
находится в этом `ChildContent` (как раз случай "меню прямо в MainLayout"), перерисовка
снова доходит до него, `OnParametersSet` вызывается заново с тем же `Open=true`, снова
`Register`, снова `StateHasChanged` — и так по кругу, вечно перерисовывая одно и то же,
ни разу не давая браузеру фактически отрисовать открывшийся поповер. Когда триггер лежит
в `@Body`, а не в самой разметке layout'а, тот же цикл на практике не запускался (иначе
рвался в рамках того же батча рендера) — поэтому баг долго не проявлялся ни на одном из
существующих поповеров (`ScarnaSelect`, `ScarnaMenu` на странице `/components` и т.п.).

Исправлено: `Register`/`Unregister` теперь вызываются только на самом переходе
`Open` false↔true (`OnParametersSet` сравнивает с `_wasOpen`), а не на каждый вызов
с `Open==true`. Актуальность содержимого при этом не страдает: `Surface()` читает
`ChildContent` как свойство **в момент вызова фрагмента провайдером**, а не как
захваченное значение, поэтому уже зарегистрированный фрагмент и так показывает
свежее содержимое при любой следующей перерисовке провайдера (по любой другой причине).

**Практический вывод**: `ScarnaPopover`-based компоненты (Menu/Select/Tooltip/будущий
Autocomplete) полностью безопасно размещать прямо в `MainLayout` — этот класс багов
исправлен на уровне примитива, отдельно обходить его в компонентах-потребителях
не нужно.

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

<ScarnaPopover Open="_open" Anchor="_anchor" Position="ScarnaPosition.BottomStart"
              OnClose="() => _open = false">
    <div class="p-2 bg-content1 rounded-scarna-md shadow-lg">Содержимое</div>
</ScarnaPopover>

@code {
    private ElementReference _anchor;
    private bool _open;
}
```

## Параметры ScarnaPopover

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Open` | `bool` | — | Открыт ли поповер |
| `Anchor` | `ElementReference` | — | **Обязательный.** Якорный элемент (захватывается вызывающим компонентом через `@ref`) |
| `ChildContent` | `RenderFragment?` | — | Содержимое поповера |
| `Position` | `ScarnaPosition` | `BottomStart` | Единое свойство позиции — сторона + выравнивание (см. ниже) |
| `IsParentFullSize` | `bool` | `false` | Растянуть поверхность на ширину якоря (родителя) — например, чтобы выпадающий список был не уже поля |
| `OffsetPx` | `int` | `8` | Отступ от якоря в пикселях |
| `CloseOnOutsideClick` | `bool` | `true` | Закрывать по клику вне поповера/якоря |
| `SurfaceClass` | `string?` | — | Доп. CSS-классы на контейнер поверхности |
| `OnClose` | `EventCallback` | — | Клик снаружи или Escape — сам поповер себя не закрывает, решение остаётся за вызывающим кодом |

## ScarnaPosition

Единая позиция вместо двух отдельных параметров (сторона + выравнивание) — 12 значений,
как в floating-ui/Radix. Используется в `ScarnaPopover`, `ScarnaTooltip`, `ScarnaMenu`, `ScarnaSelect`.

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
