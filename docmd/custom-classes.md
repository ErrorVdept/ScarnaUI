# Кастомные CSS-классы NorthUI

## Файлы
- `src/NorthUI/NorthUI/Styles/input.css` — определения (`@utility`, `@source inline(...)`)

## Что это

Помимо обычных Tailwind-утилит (`bg-primary`, `rounded-north-md`, `flex`, ...), NorthUI
определяет несколько именованных классов "для удобства" — короткие, читаемые имена под
частые задачи, которые иначе пришлось бы каждый раз собирать из нескольких utility-классов.
Использовать их можно в любом месте разметки приложения, не только внутри компонентов
NorthUI — как обычный CSS-класс.

## ⚠️ Технический нюанс: почему они не пропадают из собранного CSS

Tailwind включает в итоговый `north.css` только те классы, которые **статически
находит** при сканировании исходников (`.razor`/`.cshtml`/`.cs` — см.
[getting-started.md](getting-started.md#⚠️-локаль-сервера-и-числа-в-css) про похожий
нюанс со сборкой). Поскольку эти классы задуманы для использования в **чужом**
проекте-потребителе (который NorthUI не сканирует — см. [getting-started.md](getting-started.md)),
их принудительно включают через `@source inline(...)` в `Styles/input.css`:

```css
@source inline("north-width-full north-height-full scrollable scrollable-h scrollable-v");
```

**Если добавляете новый класс в этот файл — не забудьте дописать его имя и в
`@source inline(...)`, иначе он пропадёт из собранного CSS у всех, кто не использует
его ещё и где-то в исходниках самого NorthUI.**

## north-width-full / north-height-full

Растягивают элемент на 100% ширины/высоты родителя. Названы иначе, чем стандартные
Tailwind-утилиты `w-full`/`h-full` (тоже доступны, это просто более явные "фирменные"
алиасы, как `mud-width-full`/`mud-height-full` в MudBlazor).

```razor
<div class="flex h-24 gap-3">
    <div class="north-height-full w-32 bg-content2">100% высоты родителя</div>
    <div class="north-width-full bg-content2">Занимает всё оставшееся место</div>
</div>
```

## scrollable / scrollable-h / scrollable-v

Делают элемент прокручиваемым, с тонким скроллбаром в цвет темы (`--north-divider`)
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
<div class="scrollable-v h-64 rounded-north-md border border-divider p-3">
    @foreach (var item in items)
    {
        <p>@item.Name</p>
    }
</div>
```

## Justify / AlignItems (NorthStack, NorthGrid)

Не CSS-классы, а C#-параметры — упомянуты здесь, потому что решают ту же задачу
"частых компоновочных нужд", что и остальное на этой странице, и сделаны по образцу
`Justify`/`AlignItems` в MudBlazor. Полное описание — в [stack.md](stack.md) и
[grid.md](grid.md).

```razor
<NorthStack Row="true" Justify="NorthJustify.SpaceEvenly" Align="NorthAlign.Baseline">
```

`NorthJustify`: `Start`, `End`, `Center`, `SpaceBetween`, `SpaceAround`, `SpaceEvenly`.
`NorthAlign`: `Start`, `End`, `Center`, `Stretch`, `Baseline`.

---

*Сюда добавляются новые "фишки" по мере появления — см. правило про
`@source inline(...)` выше при добавлении нового класса.*
