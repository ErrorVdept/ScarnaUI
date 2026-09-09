# NorthThemeSwitch

## Файлы
- `src/NorthUI/NorthUI/Components/Theme/NorthThemeSwitch.razor`

## Описание

Готовая кнопка переключения светлой/тёмной темы. Сама читает и меняет тему через
каскадный `NorthThemeContext` (см. [theme-provider.md](theme-provider.md)) — в
приложении не нужно писать никакого кода, достаточно разместить компонент внутри
`NorthThemeProvider`.

## Пример использования

```razor
<NorthThemeProvider>
    <header>
        <NorthThemeSwitch />
    </header>
    @Body
</NorthThemeProvider>
```

## Параметры

Без параметров — иконка (солнце/луна) и обработчик клика зависят только от текущего
`NorthThemeContext.IsDarkMode`.
