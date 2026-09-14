# ScarnaDivider

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/DataDisplay/ScarnaDivider.razor`

## Описание

Тонкая разделительная линия цвета `divider` (см. [theme-provider.md](theme-provider.md)).

## Пример использования

```razor
<ScarnaDivider />

<ScarnaStack Row="true" Align="ScarnaAlign.Center">
    <span>Профиль</span>
    <ScarnaDivider Vertical="true" Class="h-6" />
    <span>Настройки</span>
</ScarnaStack>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Vertical` | `bool` | `false` | Вертикальная линия вместо горизонтальной (нужна явная высота через `Class`/`Style`, т.к. родитель обычно не задаёт высоту сам) |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |
