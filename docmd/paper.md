# ScarnaPaper

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaPaper.razor`

## Описание

Простая поверхность — фон `content1`, скругление, опциональные рамка/тень. Почти как
[ScarnaCard](card.md), но без секций `HeaderContent`/`FooterContent` — один произвольный
`ChildContent` (аналог `MudPaper`).

Дополнительно умеет:
- **`Hovered`** — смена фона (`bg-content2`) при наведении курсора, без смещения/тени —
  не "прыгает" в плотных сетках плиток. Полезно для кликабельных/интерактивных
  поверхностей (карточки в списке, плитки в дашборде).
- **`OnClick`** — если задан, поверхность становится кликабельной: курсор `pointer`,
  `role="button"`, доступна с клавиатуры (`tabindex="0"`, `Enter`/`Space` тоже вызывают
  обработчик — обычный `<div>`, в отличие от `<button>`, сам на клавиатуру не реагирует,
  поэтому это сделано вручную), лёгкое сжатие при нажатии (`active:scale-[0.99]`) и
  кольцо фокуса. Если `OnClick` не задан — обычный статичный `<div>`, как `ScarnaCard`.

## Пример использования

```razor
<ScarnaPaper>Обычная поверхность</ScarnaPaper>

<ScarnaPaper Hovered="true" OnClick="() => Selected = true">
    Кликабельная плитка — курсор pointer, смена фона при наведении, доступна с клавиатуры
</ScarnaPaper>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Bordered` | `bool` | `false` | Тонкая рамка в дополнение к тени |
| `Shadow` | `bool` | `true` | Тень под поверхностью |
| `Hovered` | `bool` | `false` | Смена фона (bg-content2) при наведении |
| `Radius` | `ScarnaRadius` | `Medium` | Радиус скругления |
| `OnClick` | `EventCallback<MouseEventArgs>` | — | Если задан — поверхность кликабельна (role="button", Enter/Space). Не задан — обычный статичный контейнер |
| `ChildContent` | `RenderFragment?` | — | Содержимое |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
