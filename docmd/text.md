# ScarnaText

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaText.cs`
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaTextEnums.cs` (`ScarnaTypo`, `ScarnaTextAlign`)

## Описание

Типографический текст — аналог `MudText`. Чистый C#-компонент (не `.razor`): HTML-тег
зависит от `Typo` (h1 для `Typo.H1`, `p` для `Body1`, `span` для `Caption` и т.д.) или
явного `As`, а в `.razor`-разметке имя тега всегда статическое — поэтому рендер
собран вручную через `RenderTreeBuilder`.

## Пример использования

```razor
<ScarnaText Typo="ScarnaTypo.H2" GutterBottom="true">Заголовок раздела</ScarnaText>
<ScarnaText Typo="ScarnaTypo.Body1">Обычный текст абзаца.</ScarnaText>
<ScarnaText Typo="ScarnaTypo.Body2" Color="ScarnaColor.Danger">Ошибка сохранения</ScarnaText>
<ScarnaText Typo="ScarnaTypo.Caption">Приглушённая подпись</ScarnaText>

@* Заголовок H3 визуально, но семантически <span> (например, внутри <button>) *@
<ScarnaText Typo="ScarnaTypo.H3" As="span">...</ScarnaText>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Typo` | `ScarnaTypo` | `Body1` | `H1`-`H6`, `Subtitle1`/`Subtitle2`, `Body1`/`Body2`, `Caption`, `Overline`, `Button` — определяет и тег, и размер/начертание шрифта |
| `Color` | `ScarnaColor?` | `null` | Не задан — обычный `text-foreground` (у `Caption`/`Overline` по умолчанию приглушённый `text-foreground/60`) |
| `Align` | `ScarnaTextAlign` | `Inherit` | `Start`/`Center`/`End`/`Justify` |
| `GutterBottom` | `bool` | `false` | Отступ снизу — для последовательных абзацев |
| `Truncate` | `bool` | `false` | Обрезать в одну строку с многоточием при переполнении |
| `As` | `string?` | — | Переопределить HTML-тег по умолчанию для выбранного `Typo` |
| `ChildContent` | `RenderFragment?` | — | Содержимое |
