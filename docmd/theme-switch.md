# ScarnaThemeSwitch

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Theme/ScarnaThemeSwitch.razor`

## Описание

Готовая кнопка переключения светлой/тёмной темы. Сама читает и меняет тему через
каскадный `ScarnaThemeContext` (см. [theme-provider.md](theme-provider.md)) — в
приложении не нужно писать никакого кода, достаточно разместить компонент внутри
`ScarnaThemeProvider`.

## Пример использования

```razor
<ScarnaThemeProvider>
    <header>
        <ScarnaThemeSwitch />
    </header>
    @Body
</ScarnaThemeProvider>
```

## Параметры

Без параметров — иконка (солнце/луна) и обработчик клика зависят только от текущего
`ScarnaThemeContext.IsDarkMode`.
