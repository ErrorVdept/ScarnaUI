# ScarnaBadge

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaBadge.razor`

## Описание

Индикатор-оверлей поверх произвольного содержимого (аватара, иконки, кнопки) —
счётчик непрочитанных сообщений, точка "есть событие" и т.п. В отличие от
[ScarnaChip](chip.md), не самостоятелен — всегда оборачивает `ChildContent`.

## Пример использования

```razor
<ScarnaBadge Content="@unreadCount.ToString()" Visible="@(unreadCount > 0)">
    <ScarnaAvatar Initials="ИИ" />
</ScarnaBadge>

<ScarnaBadge Dot="true" Color="ScarnaColor.Success">
    <ScarnaAvatar Src="/img/user.jpg" />
</ScarnaBadge>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Color` | `ScarnaColor` | `Danger` | Цвет заливки значка |
| `Content` | `string?` | — | Текст/число внутри значка (игнорируется при `Dot`) |
| `Dot` | `bool` | `false` | Показать маленькую точку вместо текста |
| `Visible` | `bool` | `true` | Управляет видимостью значка целиком |
| `Bordered` | `bool` | `true` | Обводка в цвет фона страницы, отделяет значок от того, что под ним |
| `ChildContent` | `RenderFragment?` | — | Элемент, поверх которого рисуется значок |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
