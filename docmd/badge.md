# NorthBadge

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthBadge.razor`

## Описание

Индикатор-оверлей поверх произвольного содержимого (аватара, иконки, кнопки) —
счётчик непрочитанных сообщений, точка "есть событие" и т.п. В отличие от
[NorthChip](chip.md), не самостоятелен — всегда оборачивает `ChildContent`.

## Пример использования

```razor
<NorthBadge Content="@unreadCount.ToString()" Visible="@(unreadCount > 0)">
    <NorthAvatar Initials="ИИ" />
</NorthBadge>

<NorthBadge Dot="true" Color="NorthColor.Success">
    <NorthAvatar Src="/img/user.jpg" />
</NorthBadge>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `NorthColor` | `Danger` | Цвет заливки значка |
| `Content` | `string?` | — | Текст/число внутри значка (игнорируется при `Dot`) |
| `Dot` | `bool` | `false` | Показать маленькую точку вместо текста |
| `Visible` | `bool` | `true` | Управляет видимостью значка целиком |
| `Bordered` | `bool` | `true` | Обводка в цвет фона страницы, отделяет значок от того, что под ним |
| `ChildContent` | `RenderFragment?` | — | Элемент, поверх которого рисуется значок |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
