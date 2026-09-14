# ScarnaImage

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaImage.razor`

## Описание

Обёртка над `<img>` со скруглением, обводкой и запасным содержимым при ошибке
загрузки (по умолчанию — иконка-заглушка, можно передать своё через `Fallback`).

## Пример использования

```razor
<ScarnaImage Src="/img/cover.jpg" Alt="Обложка" Radius="ScarnaRadius.Large" Class="h-40 w-full" />
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Src` | `string` | — | **Обязательный.** URL изображения |
| `Alt` | `string?` | — | — |
| `Radius` | `ScarnaRadius` | `Large` | Радиус скругления |
| `Bordered` | `bool` | `false` | Обводка |
| `Lazy` | `bool` | `true` | Отложенная загрузка (`loading="lazy"`) |
| `Fallback` | `RenderFragment?` | — | Что показать вместо изображения, если оно не загрузилось (по умолчанию — иконка-заглушка) |
