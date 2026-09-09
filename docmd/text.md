# NorthText

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthText.cs`
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthTextEnums.cs` (`NorthTypo`, `NorthTextAlign`)

## Описание

Типографический текст — аналог `MudText`. Чистый C#-компонент (не `.razor`): HTML-тег
зависит от `Typo` (h1 для `Typo.H1`, `p` для `Body1`, `span` для `Caption` и т.д.) или
явного `As`, а в `.razor`-разметке имя тега всегда статическое — поэтому рендер
собран вручную через `RenderTreeBuilder`.

## Пример использования

```razor
<NorthText Typo="NorthTypo.H2" GutterBottom="true">Заголовок раздела</NorthText>
<NorthText Typo="NorthTypo.Body1">Обычный текст абзаца.</NorthText>
<NorthText Typo="NorthTypo.Body2" Color="NorthColor.Danger">Ошибка сохранения</NorthText>
<NorthText Typo="NorthTypo.Caption">Приглушённая подпись</NorthText>

@* Заголовок H3 визуально, но семантически <span> (например, внутри <button>) *@
<NorthText Typo="NorthTypo.H3" As="span">...</NorthText>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Typo` | `NorthTypo` | `Body1` | `H1`-`H6`, `Subtitle1`/`Subtitle2`, `Body1`/`Body2`, `Caption`, `Overline`, `Button` — определяет и тег, и размер/начертание шрифта |
| `Color` | `NorthColor?` | `null` | Не задан — обычный `text-foreground` (у `Caption`/`Overline` по умолчанию приглушённый `text-foreground/60`) |
| `Align` | `NorthTextAlign` | `Inherit` | `Start`/`Center`/`End`/`Justify` |
| `GutterBottom` | `bool` | `false` | Отступ снизу — для последовательных абзацев |
| `Truncate` | `bool` | `false` | Обрезать в одну строку с многоточием при переполнении |
| `As` | `string?` | — | Переопределить HTML-тег по умолчанию для выбранного `Typo` |
| `ChildContent` | `RenderFragment?` | — | Содержимое |
