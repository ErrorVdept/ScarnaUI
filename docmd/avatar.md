# NorthAvatar

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthAvatar.razor`

## Описание

Круглый аватар: изображение по `Src`, а если его нет или оно не загрузилось —
инициалы (`Initials`) на цветном фоне; если нет и того, и другого — стандартная
силуэт-иконка.

## Пример использования

```razor
<NorthAvatar Src="/img/user.jpg" Alt="Иван Иванов" />
<NorthAvatar Initials="ИИ" Color="NorthColor.Primary" Size="NorthSize.Large" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Src` | `string?` | — | URL изображения |
| `Alt` | `string?` | — | `alt` для изображения |
| `Initials` | `string?` | — | Инициалы (1-2 буквы), показываются, если нет `Src` или изображение не загрузилось |
| `Size` | `NorthSize` | `Medium` | `Small`/`Medium`/`Large` |
| `Color` | `NorthColor` | `Default` | Цвет фона под инициалами |
| `Bordered` | `bool` | `false` | Тонкая обводка в цвет фона страницы, отделяет от подложки |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
