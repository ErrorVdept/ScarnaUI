# NorthDivider

## Файлы
- `src/NorthUI/NorthUI/Components/DataDisplay/NorthDivider.razor`

## Описание

Тонкая разделительная линия цвета `divider` (см. [theme-provider.md](theme-provider.md)).

## Пример использования

```razor
<NorthDivider />

<NorthStack Row="true" Align="NorthAlign.Center">
    <span>Профиль</span>
    <NorthDivider Vertical="true" Class="h-6" />
    <span>Настройки</span>
</NorthStack>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Vertical` | `bool` | `false` | Вертикальная линия вместо горизонтальной (нужна явная высота через `Class`/`Style`, т.к. родитель обычно не задаёт высоту сам) |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
