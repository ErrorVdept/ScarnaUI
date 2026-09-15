# ScarnaImage

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaImage.razor`

## Описание

Обёртка над `<img>` со скруглением, обводкой и запасным содержимым при ошибке
загрузки (по умолчанию — иконка-заглушка, можно передать своё через `Fallback`).

## Пример использования

```razor
<ScarnaImage Src="/img/cover.jpg" Alt="Обложка" Radius="Radius.Large" Class="h-40 w-full" />
```

Base64: готовый `data:`-URI работает прямо в `Src` (это обычный HTML-атрибут `src`,
никакой специальной поддержки не требуется). Если под рукой только сырой base64
без префикса — задайте `MimeType`, компонент сам соберёт `data:`-URI:

```razor
<ScarnaImage Src="@rawBase64" MimeType="image/png" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Src` | `string` | — | **Обязательный.** URL изображения, готовый `data:`-URI, либо сырой base64 (вместе с `MimeType`) |
| `MimeType` | `string?` | — | Если задан — `Src` трактуется как сырой base64 без префикса, компонент сам собирает `data:{MimeType};base64,{Src}` |
| `Alt` | `string?` | — | — |
| `Radius` | `Radius` | `Large` | Радиус скругления |
| `Bordered` | `bool` | `false` | Обводка |
| `Lazy` | `bool` | `true` | Отложенная загрузка (`loading="lazy"`) |
| `Fallback` | `RenderFragment?` | — | Что показать вместо изображения, если оно не загрузилось (по умолчанию — иконка-заглушка) |
