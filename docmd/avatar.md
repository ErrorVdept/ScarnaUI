# ScarnaAvatar

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaAvatar.razor`

## Описание

Аватар: изображение по `Src`, а если его нет или оно не загрузилось —
инициалы (`Initials`) на цветном фоне; если нет и того, и другого — стандартная
силуэт-иконка. По умолчанию круглый; `Square="true"` переключает на закруглённый
квадрат (радиус — `Radius`).

## Пример использования

```razor
<ScarnaAvatar Src="/img/user.jpg" Alt="Иван Иванов" />
<ScarnaAvatar Initials="ИИ" Color="Color.Primary" Size="Size.Large" />
<ScarnaAvatar Src="/img/user.jpg" Square="true" Radius="Radius.Medium" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Src` | `string?` | — | URL изображения |
| `Alt` | `string?` | — | `alt` для изображения |
| `Initials` | `string?` | — | Инициалы (1-2 буквы), показываются, если нет `Src` или изображение не загрузилось |
| `Size` | `Size` | `Medium` | `Small`/`Medium`/`Large` |
| `Color` | `Color` | `Default` | Цвет фона под инициалами |
| `Bordered` | `bool` | `false` | Тонкая обводка в цвет фона страницы, отделяет от подложки |
| `Square` | `bool` | `false` | Закруглённый квадрат вместо круга |
| `Radius` | `Radius` | `Medium` | Радиус скругления при `Square="true"` (на круглый аватар не влияет) |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
