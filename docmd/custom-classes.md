# Кастомные CSS-классы ScarnaUI

## Файлы
- `src/ScarnaUI/ScarnaUI/Styles/input.css` — определения (`@utility`, `@source inline(...)`)

## Что это

Помимо обычных Tailwind-утилит (`bg-primary`, `rounded-scarna-md`, `flex`, ...), ScarnaUI
определяет несколько именованных классов "для удобства" — короткие, читаемые имена под
частые задачи, которые иначе пришлось бы каждый раз собирать из нескольких utility-классов.
Использовать их можно в любом месте разметки приложения, не только внутри компонентов
ScarnaUI — как обычный CSS-класс.

## ⚠️ Технический нюанс: почему они не пропадают из собранного CSS

Tailwind включает в итоговый `scarna.css` только те классы, которые **статически
находит** при сканировании исходников (`.razor`/`.cshtml`/`.cs` — см.
[getting-started.md](getting-started.md#⚠️-локаль-сервера-и-числа-в-css) про похожий
нюанс со сборкой). Поскольку эти классы задуманы для использования в **чужом**
проекте-потребителе (который ScarnaUI не сканирует — см. [getting-started.md](getting-started.md)),
их принудительно включают через `@source inline(...)` в `Styles/input.css`:

```css
@source inline("scarna-width-full scarna-height-full scrollable scrollable-h scrollable-v");
@source inline("pa-{0..16} ma-{0..16} ma-n{1..16} mx-n{1..16} my-n{1..16}");
@source inline("px-{0..16} py-{0..16} mx-{0..16} my-{0..16}");
@source inline("pa-{0..16}! ma-{0..16}! px-{0..16}! py-{0..16}! mx-{0..16}! my-{0..16}!");
@source inline("ma-n{1..16}! mx-n{1..16}! my-n{1..16}!");
```

**Если добавляете новый класс в этот файл — не забудьте дописать его имя и в
`@source inline(...)`, иначе он пропадёт из собранного CSS у всех, кто не использует
его ещё и где-то в исходниках самого ScarnaUI.**

> ⚠️ Это касается **и обычных Tailwind-утилит**, не только фирменных. Изначально
> `px`/`py`/`mx`/`my` в этот список не попали — вроде бы "нативные, работают из коробки" —
> но нативность не спасает от JIT-сканирования готовой библиотеки: если класса с
> конкретным значением (`px-0`) нет буквально в исходниках ScarnaUI, его не будет в
> `scarna.css`, даже стандартного. Итог — `px-0`/`mx-0` у потребителя библиотеки молча
> ничего не делали: класс есть в разметке, правила для него в CSS не было. Строка выше
> это чинит для диапазона 0-16 (симметрично `pa`/`ma`).

## scarna-width-full / scarna-height-full

Растягивают элемент на 100% ширины/высоты родителя. Названы иначе, чем стандартные
Tailwind-утилиты `w-full`/`h-full` (тоже доступны, это просто более явные "фирменные"
алиасы, как `mud-width-full`/`mud-height-full` в MudBlazor).

```razor
<div class="flex h-24 gap-3">
    <div class="scarna-height-full w-32 bg-content2">100% высоты родителя</div>
    <div class="scarna-width-full bg-content2">Занимает всё оставшееся место</div>
</div>
```

## scrollable / scrollable-h / scrollable-v

Делают элемент прокручиваемым, с тонким скроллбаром в цвет темы (`--scarna-divider`)
вместо системного по умолчанию (`scrollbar-width: thin` + стилизация
`::-webkit-scrollbar` для Chrome/Edge).

| Класс | Поведение |
|---|---|
| `scrollable` | `overflow: auto` — прокрутка в обе стороны при переполнении |
| `scrollable-h` | Только по горизонтали (`overflow-x: auto; overflow-y: hidden`) |
| `scrollable-v` | Только по вертикали (`overflow-x: hidden; overflow-y: auto`) |

Сам по себе класс не задаёт высоту/ширину — их нужно ограничить отдельно
(`Class="scrollable-v h-64"` и т.п.), иначе скроллиться будет нечему.

```razor
<div class="scrollable-v h-64 rounded-scarna-md border border-divider p-3">
    @foreach (var item in items)
    {
        <p>@item.Name</p>
    }
</div>
```

## Отступы: pa / ma — MUD-стиль

Padding/margin у ScarnaUI регулируются классами, а не отдельными C#-параметрами
компонента (в отличие от Width/Height — см. [getting-started.md](getting-started.md#scarnacomponentbase)):
классы можно комбинировать с вариантами (`sm:pa-6`, `hover:ma-2`), а inline-style — нет.

`px`/`py`/`pt`/`pb`/`pl`/`pr`/`mx`/`my`/`mt`/`mb`/`ml`/`mr` — это обычные Tailwind-утилиты
(padding-inline/padding-block/padding-top/bottom/left/right и margin-аналоги), не
фирменные — переопределять их логику не нужно, но диапазон 0-16 принудительно включён
в `scarna.css` через `@source inline(...)` (см. предупреждение выше), иначе конкретные
значения вроде `px-0`/`pt-0` у потребителя библиотеки молча не работали бы. `pa`/`ma` —
то, чего в Tailwind нет "из коробки" под таким именем (там всё сразу — `p`/`m`) — это
фирменные алиасы под "все стороны сразу", по образцу `pa`/`ma` в MudBlazor:

| Класс | Свойство | Значение |
|---|---|---|
| `pa-{n}` | `padding` | `n × 0.25rem` (`pa-4` = 16px). Любое целое, не только 0-16 |
| `px-{n}` / `py-{n}` | `padding-inline` / `padding-block` | `n × 0.25rem`. Нативный Tailwind, 0-16 гарантированно есть в `scarna.css` |
| `pt-{n}` / `pb-{n}` / `pl-{n}` / `pr-{n}` | `padding-top`/`-bottom`/`-left`/`-right` | `n × 0.25rem`. Нативный Tailwind, 0-16 гарантированно есть в `scarna.css` |
| `ma-{n}` | `margin` | `n × 0.25rem`. Любое целое, не только 0-16 |
| `mx-{n}` / `my-{n}` | `margin-inline` / `margin-block` | `n × 0.25rem`. Нативный Tailwind, 0-16 гарантированно есть в `scarna.css` |
| `mt-{n}` / `mb-{n}` / `ml-{n}` / `mr-{n}` | `margin-top`/`-bottom`/`-left`/`-right` | `n × 0.25rem`. Нативный Tailwind, 0-16 гарантированно есть в `scarna.css` |
| `ma-n{n}` | `margin` | `-(n × 0.25rem)` — отрицательный margin, `n` от 1 до 16 |
| `mx-n{n}` / `my-n{n}` | `margin-inline` / `margin-block` | `-(n × 0.25rem)`, `n` от 1 до 16 |
| `mt-n{n}` / `mb-n{n}` / `ml-n{n}` / `mr-n{n}` | `margin-top`/`-bottom`/`-left`/`-right` | `-(n × 0.25rem)`, `n` от 1 до 16 |

Если нужно значение вне диапазона 0-16 (например, `px-24`) — оно сработает **только**
если это же число уже встречается где-то в исходниках самого ScarnaUI (маловероятно)
— иначе Tailwind его не найдёт при сканировании и в CSS не включит. Для произвольных
значений вне 0-16 надёжнее `Style="padding-inline: 6rem"` (см. [getting-started.md](getting-started.md#scarnacomponentbase)).

```razor
<div class="pa-4 ma-n2">Паддинг 16px, отрицательный margin -8px по всем сторонам</div>
<div class="mx-n4">Отрицательный горизонтальный margin — компенсирует padding родителя</div>
```

У padding отрицательных значений не бывает (браузер их просто игнорирует), поэтому
негатива у `pa`/`px`/`py`/`pt`/`pb`/`pl`/`pr` нет — только у margin-вариантов
(`ma`/`mx`/`my`/`mt`/`mb`/`ml`/`mr`). Отрицательные варианты названы `ma-n4`, а не
`-ma-4` (в отличие от нативного Tailwind, где отрицательные margin — `-m-4`), чтобы
совпадать буквально с нотацией MudBlazor.

### Переопределение отступа компонента: нужен `!`

У части компонентов отступы зашиты в саму разметку — например, `ScarnaListItem` это
`px-3 py-2`. Просто добавить `Class="pa-0"` (или даже `px-0`) снаружи **не сработает**:
Tailwind кладёт классы одного семейства в CSS по возрастанию числа, а не в порядке,
в котором их написали в `class="..."` — порядок в HTML вообще не влияет на то, какое
правило выигрывает каскад. `.px-0` физически лежит в файле ВЫШЕ `.px-3` (0 < 3), а раз
оба правила равной специфичности, побеждает то, что ниже — то есть встроенный `px-3`,
даже если ваш `px-0` написан позже в разметке.

Единственный надёжный способ переопределить такой встроенный отступ снаружи — модификатор
`!` (important), а не порядок классов:

```razor
<ScarnaListItem Value="@feed" Class="pa-0!">...</ScarnaListItem>
@* или точечно по осям: *@
<ScarnaListItem Value="@feed" Class="px-0! py-0!">...</ScarnaListItem>
```

`!`-варианты форсированы в сборку для `pa`/`ma`/`px`/`py`/`pt`/`pb`/`pl`/`pr`/`mx`/`my`/
`mt`/`mb`/`ml`/`mr` (0-16) и отрицательных `ma-n`/`mx-n`/`my-n`/`mt-n`/`mb-n`/`ml-n`/
`mr-n` (1-16) — см. `@source inline(...)` выше. Без `!` — обычный класс, который **может**
и не выиграть каскад, если у компонента уже есть свой отступ того же семейства (`p`/
`px`/`py` и т.п.); без встроенного отступа (`ScarnaStack`, `ScarnaImage`, `ScarnaAvatar`
и т.п. — там padding/margin не заданы вообще) обычный `pa-0` работает и без `!`,
переопределять там нечего.

Абсолютно надёжный вариант независимо от всего вышеперечисленного — `Style`, он
побеждает любой класс всегда: `Style="padding: 0; margin: 0"`.

## Justify / AlignItems (ScarnaStack, ScarnaGrid)

Не CSS-классы, а C#-параметры — упомянуты здесь, потому что решают ту же задачу
"частых компоновочных нужд", что и остальное на этой странице, и сделаны по образцу
`Justify`/`AlignItems` в MudBlazor. Полное описание — в [stack.md](stack.md) и
[grid.md](grid.md).

```razor
<ScarnaStack Row="true" Justify="Justify.SpaceEvenly" Align="Align.Baseline">
```

`Justify`: `Start`, `End`, `Center`, `SpaceBetween`, `SpaceAround`, `SpaceEvenly`.
`Align`: `Start`, `End`, `Center`, `Stretch`, `Baseline`.

---

*Сюда добавляются новые "фишки" по мере появления — см. правило про
`@source inline(...)` выше при добавлении нового класса.*
