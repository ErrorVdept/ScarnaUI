# NorthSlider

## Файлы
- `src/NorthUI/NorthUI/Components/Forms/NorthSlider.razor`
- `src/NorthUI/NorthUI/Styles/input.css` (`.north-slider::-webkit-slider-thumb`/`::-moz-range-thumb`, `::-webkit-slider-runnable-track`/`::-moz-range-track`)
- `src/NorthUI/NorthUI/wwwroot/js/north/slider.js` (только для `ReadOnly` — см. ниже)

## Описание

Слайдер числового значения. Под капотом — нативный `<input type="range">` (бесплатно
даёт клавиатуру, touch, доступность). Дорожка стилизуется обычными Tailwind-классами
+ инлайн-градиент (залитая часть), а ползунок — одним рукописным CSS-блоком в
`Styles/input.css` на `::-webkit-slider-thumb`/`::-moz-range-thumb`, а не россыпью
Tailwind arbitrary-variant классов (`[&::-webkit-slider-thumb]:w-7` и т.п. — так было
раньше и не работало надёжно: браузер применял часть сгенерированных правил, часть —
нет, из-за чего у ползунка пропадали фон/обводка/размер, оставалась только дорожка).
Один консолидированный блок обычного CSS — стандартный, проверенный способ стилизовать
нативный range-thumb, и он же уже используется для скроллбара (`.scrollable`, см.
[custom-classes.md](custom-classes.md)). Цвет обводки передаётся через CSS-переменную
`--north-slider-thumb-color`, выставляемую инлайн-стилем из `NorthSlider.razor` (тот
же `style`, что задаёт градиент дорожки). Компонент основан на
`NorthFormComponentBase<double>` — поддерживает `@bind-Value`.

По той же причине (браузер по умолчанию нежно оставляет собственную дорожку поверх
всего остального) отдельно сброшены `::-webkit-slider-runnable-track`/`::-moz-range-track`
(`appearance: none`, прозрачный фон) — без этого поверх нашего скруглённого градиента
на самом `<input>` Chrome/Edge продолжали рисовать свою дефолтную дорожку — плоский
серый прямоугольник с прямыми углами, из-за чего слайдер выглядел как "два слайдера,
один из которых квадратный".

`ReadOnly` реализован вручную (в отличие от текстовых полей, HTML-атрибут `readonly`
браузеры для `input[type=range]` игнорируют): изменения игнорируются в
`HandleInputAsync`, `pointer-events: none` блокирует перетаскивание мышью/тачем. Но
раз браузер уже успевает подвинуть значение в DOM до срабатывания `oninput` (сама
природа `readonly`-обхода), а `Value` в C# при этом не меняется — обычный рендер
Blazor не станет откатывать `value` того же самого input обратно (в его дереве рендера
атрибут и так не изменился). Поэтому readonly-ветка `HandleInputAsync` дополнительно
форсирует `input.value` обратно через маленький JS-модуль `slider.js`.

Пропорции (дорожка 20px, ползунок 28×20px со скруглением 12px) сняты напрямую с
`heroui.com` — computed styles их реального слайдера были прочитаны через
`getBoundingClientRect`/`getComputedStyle` прямо в браузере, а не подобраны на глаз.
Точное совпадение недостижимо (у HeroUI ползунок — белая "таблетка" под отдельным
чуть большим цветным слоем; на нативном `<input type="range">` такой двухслойности
нет, поэтому здесь цветное кольцо сделано через `border`), но пропорции и общий
силуэт — те же.

## Пример использования

```razor
<NorthSlider Label="Громкость" @bind-Value="_volume" Min="0" Max="100" ShowValue="true" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Value` / `ValueChanged` / `ValueExpression` | `double` / ... | — | Механика `@bind-Value` |
| `Label` | `string?` | — | Подпись над слайдером |
| `Min` / `Max` | `double` | `0` / `100` | Диапазон значений |
| `Step` | `double` | `1` | Шаг |
| `Disabled` | `bool` | `false` | — |
| `ReadOnly` | `bool` | `false` | Игнорирует ввод (мышь/тач/клавиатура), но, в отличие от `Disabled`, не тускнеет — см. описание выше |
| `ShowValue` | `bool` | `false` | Показывать текущее значение справа от подписи |
| `Format` | `string` | `"0.##"` | .NET numeric format string для отображаемого значения |
| `Color` | `NorthColor` | `Primary` | Цвет ползунка и залитой части дорожки |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
