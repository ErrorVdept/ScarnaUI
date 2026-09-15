# ScarnaCodeBlock

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaCodeBlock.razor`
- `src/ScarnaUI/ScarnaUI/wwwroot/js/scarna/codeblock.js` (копирование — `navigator.clipboard` доступен только через JS interop)

## Описание

Блок кода: моноширинный шрифт, тонкая рамка, опциональная кнопка копирования в правом
верхнем углу (`navigator.clipboard.writeText`, с иконкой-подтверждением на 1.5 секунды
после клика).

По умолчанию высота **авто** — блок растягивается по содержимому без ограничений.
Три параметра задают другое поведение:
- **`MinLines`** — минимальная высота в строках (блок не сожмётся меньше).
- **`MaxLines`** — максимальная высота в строках, дальше появляется вертикальная
  прокрутка (`overflow-y`).
- **`Lines`** — точное количество строк (одновременно min и max) — фиксированная
  высота независимо от объёма содержимого.

Высота в строках переводится в `em` через множитель `1.625` — тот же коэффициент,
что у Tailwind-класса `leading-relaxed`, применённого к тексту блока, поэтому
запрошенное число строк совпадает с реальным, а не "на пиксель мимо" из-за
рассинхронизации с фактическим line-height.

Используется внутри самой библиотеки документации (`ComponentLib.Shared.DocSection`
"Показать код" и страница `/getting-started`) — то есть каждый блок кода, который вы
видите на этом сайте, это и есть `ScarnaCodeBlock`.

## Пример использования

```razor
<ScarnaCodeBlock Code="dotnet add package ScarnaUI" />
<ScarnaCodeBlock Code="@longSnippet" MaxLines="12" />
<ScarnaCodeBlock Code="@longSnippet" Lines="6" ShowCopyButton="false" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Code` | `string` | — | **Обязательный.** Текст кода |
| `ShowCopyButton` | `bool` | `true` | Кнопка копирования в правом верхнем углу |
| `MinLines` | `int?` | — | Минимальное количество видимых строк |
| `MaxLines` | `int?` | — | Максимальное количество видимых строк — дальше вертикальная прокрутка |
| `Lines` | `int?` | — | Точное количество строк (вместо `MinLines`/`MaxLines`) — фиксированная высота |
| `Radius` | `Radius` | `Medium` | Радиус скругления |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
