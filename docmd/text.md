# ScarnaText

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaText.cs`
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaTextEnums.cs` (`Typo`, `TextAlign`)

## Описание

Типографический текст — аналог `MudText`. Чистый C#-компонент (не `.razor`): HTML-тег
зависит от `Typo` (h1 для `Typo.H1`, `p` для `Body1`, `span` для `Caption` и т.д.) или
явного `As`, а в `.razor`-разметке имя тега всегда статическое — поэтому рендер
собран вручную через `RenderTreeBuilder`.

## Пример использования

```razor
<ScarnaText Typo="Typo.H2" GutterBottom="true">Заголовок раздела</ScarnaText>
<ScarnaText Typo="Typo.Body1">Обычный текст абзаца.</ScarnaText>
<ScarnaText Typo="Typo.Body2" Color="Color.Danger">Ошибка сохранения</ScarnaText>
<ScarnaText Typo="Typo.Caption">Приглушённая подпись</ScarnaText>

@* Заголовок H3 визуально, но семантически <span> (например, внутри <button>) *@
<ScarnaText Typo="Typo.H3" As="span">...</ScarnaText>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Typo` | `Typo` | `Body1` | `H1`-`H6`, `Subtitle1`/`Subtitle2`, `Body1`/`Body2`, `Caption`, `Overline`, `Button` — определяет и тег, и размер/начертание шрифта |
| `Color` | `Color?` | `null` | Не задан — обычный `text-foreground` (у `Caption`/`Overline` по умолчанию приглушённый `text-foreground/60`) |
| `TextAlign` | `TextAlign` | `Inherit` | `Start`/`Center`/`End`/`Justify` |
| `UseCaptionColor` | `bool` | `false` | Приглушённый цвет как у `Caption` (`text-foreground/60`) для любого `Typo`. Игнорируется, если задан `Color` |
| `GutterBottom` | `bool` | `false` | Отступ снизу — для последовательных абзацев |
| `Truncate` | `bool` | `false` | Обрезать в одну строку с многоточием при переполнении |
| `As` | `string?` | — | Переопределить HTML-тег по умолчанию для выбранного `Typo` |
| `ChildContent` | `RenderFragment?` | — | Содержимое |
