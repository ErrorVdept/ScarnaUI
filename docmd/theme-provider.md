# Тема: NorthThemeProvider

## Файлы
- `src/NorthUI/NorthUI/Components/Theme/NorthThemeProvider.razor` + `.razor.cs`
- `src/NorthUI/NorthUI/Components/Theme/NorthTheme.cs` — модель токенов (`NorthTheme`, `NorthPalette`, `NorthRadiusScale`, `NorthThemeMode`)
- `src/NorthUI/NorthUI/Components/Theme/NorthThemeContext.cs` — каскадный контекст
- `src/NorthUI/NorthUI/wwwroot/js/north/theme.js` — применение CSS-переменных, localStorage, слежение за системной темой
- `src/NorthUI/NorthUI/wwwroot/css/north/variables.css` — значения по умолчанию (до отработки JS)
- `src/NorthUI/NorthUI/Styles/input.css` — вход Tailwind, маппинг токенов на CSS-переменные
- `src/NorthUI/NorthUI/build-css.ps1` — пересборка CSS standalone-бинарником Tailwind

## Что это и зачем

Корневой провайдер темы. Кладётся один раз в `MainLayout.razor`, оборачивая всё
приложение — как `MudThemeProvider` в MudBlazor. Отвечает за три вещи:

1. Кладёт `NorthTheme` (токены) и текущий режим (light/dark/system) в каскад — любой
   компонент ниже по дереву может получить их через `[CascadingParameter] NorthThemeContext? ThemeContext`.
2. Переносит токены темы в CSS-переменные на `<html>` через JS — **это и есть
   механизм рантайм-темы**: смена цвета/радиуса применяется мгновенно, без пересборки CSS.
3. Запоминает выбор пользователя (light/dark/system) в `localStorage` и слушает
   `prefers-color-scheme`, если выбран режим `System`.

## Пример использования

```razor
@* MainLayout.razor *@
<NorthThemeProvider DefaultMode="NorthThemeMode.System">
    <NorthPopoverProvider>
        @Body
    </NorthPopoverProvider>
    <NorthDialogProvider />
    <NorthSnackbarProvider />
</NorthThemeProvider>
```

Своя палитра (например, другой Primary-цвет для конкретного white-label-клиента) —
проще всего поменять нужные поля прямо у палитр по умолчанию:

```razor
<NorthThemeProvider Theme="_customTheme">
    @Body
</NorthThemeProvider>

@code {
    private readonly NorthTheme _customTheme = new();

    protected override void OnInitialized()
    {
        _customTheme.Light.Primary = "142 71% 45%"; // HSL-триплет "H S% L%" — зелёный вместо синего
        _customTheme.Dark.Primary = "142 71% 55%";
    }
}
```

Переключатель темы из кода приложения (например, кнопка в настройках профиля):

```razor
@code {
    [CascadingParameter] private NorthThemeContext? ThemeContext { get; set; }

    private Task SetDark() => ThemeContext!.SetModeAsync(NorthThemeMode.Dark);
}
```

## Параметры NorthThemeProvider

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | — | Дерево приложения |
| `Theme` | `NorthTheme` | палитра NorthUI по умолчанию | Токены (цвета, радиусы, шрифт) |
| `DefaultMode` | `NorthThemeMode` | `System` | Режим при первом запуске (если ещё нет сохранённого выбора) |

## NorthThemeContext (то, что приходит в `[CascadingParameter]`)

| Член | Назначение |
|---|---|
| `Theme` | Активные токены (тот самый объект `NorthTheme`) |
| `ActivePalette` | `Light` или `Dark` палитра — в зависимости от `IsDarkMode` |
| `IsDarkMode` | `bool` — сейчас показана тёмная тема (уже разрешённый `System`) |
| `Mode` | Выбранный пользователем режим |
| `ToggleDarkModeAsync()` | Переключить light ⇄ dark |
| `SetModeAsync(NorthThemeMode)` | Явно установить режим |

## Модель токенов: NorthTheme / NorthPalette

`NorthTheme` хранит `Light`/`Dark` (по одному `NorthPalette` на тему) и `Radius`
(`Small`/`Medium`/`Large`/`Full` — CSS-размеры вроде `"12px"`). Цвета в `NorthPalette`
хранятся как HSL-триплет без функции: `"212 100% 47%"` — это даёт Tailwind-утилитам
поддержку прозрачности вида `bg-primary/50` через `color-mix()`.

Семантические цвета: `Background`/`Foreground`, `Content1..4` (поверхности карточек/
попапов), `Default`, `Primary`, `Secondary`, `Success`, `Warning`, `Danger` (у каждого
есть пара `*Foreground` — контрастный цвет текста), плюс `Focus`, `Divider`, `Overlay`.

### Источник палитры по умолчанию

Значения по умолчанию в `NorthPalette.LightDefault()`/`DarkDefault()` — это палитра
[HeroUI Theme Builder](https://heroui.com/en/themes?lightness=0.6204&chroma=0.195&hue=253.83)
(`lightness=0.6204, chroma=0.195, hue=253.83`, OKLCH), сконвертированная в HSL. HeroUI
отдаёт токены в OKLCH — более перцептивно равномерном пространстве, чем HSL, но чтобы
не переписывать весь пайплайн (`hsl(var(--north-primary))` используется по всей
библиотеке), значения один раз конвертированы скриптом `tools/oklch-convert.mjs`
(формулы OKLab↔sRGB по спецификации Björn Ottosson) и захардкожены как обычные HSL-строки.
Чтобы перенести другую палитру с того же Theme Builder — поменяйте параметры в URL,
откройте панель экспорта (иконка `</>`), возьмите OKLCH-значения из `globals.css` и
прогоните через тот же скрипт: `node tools/oklch-convert.mjs` (отредактировав входные
значения в начале файла).

В новой токен-системе HeroUI нет отдельного Secondary — он подобран отдельно по той же
методологии (тот же приглушённый нейтральный фон, один яркий акцентный оттенок, сдвинутый
по hue от Primary). `Overlay` тоже не взят из HeroUI напрямую: в их токенах `overlay` —
это, по сути, синоним `surface` (фон всплывающих поверхностей), а в NorthUI `Overlay` —
цвет затемняющего скрима под модалками (`bg-overlay/50` в `NorthDialogHost`), поэтому он
остался нейтральным чёрным в обеих темах.

## Как это работает "под капотом"

CSS-переменные `--north-primary` и т.д. определены в `variables.css` (нужны для
первого кадра, до того как отработает JS — иначе будет "моргание" темы). Файл
`Styles/input.css` мапит их на Tailwind-токены:

```css
@theme inline {
  --color-primary: hsl(var(--north-primary));
  --radius-north-md: var(--north-radius-md);
}
```

Дальше `NorthThemeProvider` при инициализации и при каждой смене режима вызывает
`theme.js: applyTheme(vars, isDark)`, который делает `document.documentElement.style
.setProperty("--north-primary", "...")` и переключает `data-theme="dark"/"light"`.
Поскольку Tailwind-утилиты (`bg-primary`, `text-primary`, `rounded-north-md`) уже
ссылаются на эти переменные, они мгновенно подхватывают новое значение — без единого
запроса к серверу за новым CSS.

Тёмная тема переключается атрибутом `data-theme`, а не `prefers-color-scheme`
напрямую — поэтому в `input.css` переопределён Tailwind-вариант `dark:`:

```css
@custom-variant dark (&:where([data-theme="dark"], [data-theme="dark"] *));
```

## Почему классы написаны буквально

Tailwind находит классы для генерации CSS **статическим сканированием исходников**
(`.razor`/`.cshtml`/`.cs`), а не выполняя код. Строка, собранная в рантайме через
`$"bg-{color}"`, никогда не появляется в исходном файле как текст "bg-primary" —
значит, Tailwind её не найдёт и класс не попадёт в CSS. Поэтому во всех компонентах
цвет/вариант/радиус выбираются через `switch`-выражение с буквальными строками
(см. `NorthColorVariantClasses.cs`, `NorthItem.razor`), а не через интерполяцию.

## Пересборка CSS

Нужна только при разработке самой библиотеки (добавили новый компонент/класс) —
не в проектах, которые NorthUI подключают:

```bash
./src/NorthUI/NorthUI/build-css.ps1          # собрать один раз
./src/NorthUI/NorthUI/build-css.ps1 -Watch   # пересобирать при изменениях
```

Скрипт сам скачивает `tailwindcss-windows-x64.exe` (standalone-бинарник, без Node.js
и npm) при первом запуске, кладёт его в `tools/` (в git не коммитится, см. `.gitignore`).
