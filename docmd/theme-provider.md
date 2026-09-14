# Тема: ScarnaThemeProvider

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Theme/ScarnaThemeProvider.razor` + `.razor.cs`
- `src/ScarnaUI/ScarnaUI/Components/Theme/ScarnaTheme.cs` — модель токенов (`ScarnaTheme`, `ScarnaPalette`, `ScarnaRadiusScale`, `ScarnaThemeMode`)
- `src/ScarnaUI/ScarnaUI/Components/Theme/ScarnaThemeContext.cs` — каскадный контекст
- `src/ScarnaUI/ScarnaUI/wwwroot/js/scarna/theme.js` — применение CSS-переменных, localStorage, слежение за системной темой
- `src/ScarnaUI/ScarnaUI/wwwroot/css/scarna/variables.css` — значения по умолчанию (до отработки JS)
- `src/ScarnaUI/ScarnaUI/Styles/input.css` — вход Tailwind, маппинг токенов на CSS-переменные
- `src/ScarnaUI/ScarnaUI/build-css.ps1` — пересборка CSS standalone-бинарником Tailwind

## Что это и зачем

Корневой провайдер темы. Кладётся один раз в `MainLayout.razor`, оборачивая всё
приложение — как `MudThemeProvider` в MudBlazor. Отвечает за три вещи:

1. Кладёт `ScarnaTheme` (токены) и текущий режим (light/dark/system) в каскад — любой
   компонент ниже по дереву может получить их через `[CascadingParameter] ScarnaThemeContext? ThemeContext`.
2. Переносит токены темы в CSS-переменные на `<html>` через JS — **это и есть
   механизм рантайм-темы**: смена цвета/радиуса применяется мгновенно, без пересборки CSS.
3. Запоминает выбор пользователя (light/dark/system) в `localStorage` и слушает
   `prefers-color-scheme`, если выбран режим `System`.

## Пример использования

```razor
@* MainLayout.razor *@
<ScarnaThemeProvider DefaultMode="ScarnaThemeMode.System">
    <ScarnaPopoverProvider>
        @Body
    </ScarnaPopoverProvider>
    <ScarnaDialogProvider />
    <ScarnaSnackbarProvider />
</ScarnaThemeProvider>
```

Своя палитра (например, другой Primary-цвет для конкретного white-label-клиента) —
проще всего поменять нужные поля прямо у палитр по умолчанию:

```razor
<ScarnaThemeProvider Theme="_customTheme">
    @Body
</ScarnaThemeProvider>

@code {
    private readonly ScarnaTheme _customTheme = new();

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
    [CascadingParameter] private ScarnaThemeContext? ThemeContext { get; set; }

    private Task SetDark() => ThemeContext!.SetModeAsync(ScarnaThemeMode.Dark);
}
```

## Параметры ScarnaThemeProvider

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | — | Дерево приложения |
| `Theme` | `ScarnaTheme` | палитра ScarnaUI по умолчанию | Токены (цвета, радиусы, шрифт) |
| `DefaultMode` | `ScarnaThemeMode` | `System` | Режим при первом запуске (если ещё нет сохранённого выбора) |

## ScarnaThemeContext (то, что приходит в `[CascadingParameter]`)

| Член | Назначение |
|---|---|
| `Theme` | Активные токены (тот самый объект `ScarnaTheme`) |
| `ActivePalette` | `Light` или `Dark` палитра — в зависимости от `IsDarkMode` |
| `IsDarkMode` | `bool` — сейчас показана тёмная тема (уже разрешённый `System`) |
| `Mode` | Выбранный пользователем режим |
| `ToggleDarkModeAsync()` | Переключить light ⇄ dark |
| `SetModeAsync(ScarnaThemeMode)` | Явно установить режим |

## Модель токенов: ScarnaTheme / ScarnaPalette

`ScarnaTheme` хранит `Light`/`Dark` (по одному `ScarnaPalette` на тему) и `Radius`
(`Small`/`Medium`/`Large`/`Full` — CSS-размеры вроде `"12px"`). Цвета в `ScarnaPalette`
хранятся как HSL-триплет без функции: `"212 100% 47%"` — это даёт Tailwind-утилитам
поддержку прозрачности вида `bg-primary/50` через `color-mix()`.

Семантические цвета: `Background`/`Foreground`, `Content1..4` (поверхности карточек/
попапов), `Default`, `Primary`, `Secondary`, `Success`, `Warning`, `Danger` (у каждого
есть пара `*Foreground` — контрастный цвет текста), плюс `Focus`, `Divider`, `Overlay`.

### Источник палитры по умолчанию

Значения по умолчанию в `ScarnaPalette.LightDefault()`/`DarkDefault()` — это палитра
[HeroUI Theme Builder](https://heroui.com/en/themes?lightness=0.6204&chroma=0.195&hue=253.83)
(`lightness=0.6204, chroma=0.195, hue=253.83`, OKLCH), сконвертированная в HSL. HeroUI
отдаёт токены в OKLCH — более перцептивно равномерном пространстве, чем HSL, но чтобы
не переписывать весь пайплайн (`hsl(var(--scarna-primary))` используется по всей
библиотеке), значения один раз конвертированы скриптом `tools/oklch-convert.mjs`
(формулы OKLab↔sRGB по спецификации Björn Ottosson) и захардкожены как обычные HSL-строки.
Чтобы перенести другую палитру с того же Theme Builder — поменяйте параметры в URL,
откройте панель экспорта (иконка `</>`), возьмите OKLCH-значения из `globals.css` и
прогоните через тот же скрипт: `node tools/oklch-convert.mjs` (отредактировав входные
значения в начале файла).

В новой токен-системе HeroUI нет отдельного Secondary — он подобран отдельно по той же
методологии (тот же приглушённый нейтральный фон, один яркий акцентный оттенок, сдвинутый
по hue от Primary). `Overlay` тоже не взят из HeroUI напрямую: в их токенах `overlay` —
это, по сути, синоним `surface` (фон всплывающих поверхностей), а в ScarnaUI `Overlay` —
цвет затемняющего скрима под модалками (`bg-overlay/50` в `ScarnaDialogHost`), поэтому он
остался нейтральным чёрным в обеих темах.

## Как это работает "под капотом"

CSS-переменные `--scarna-primary` и т.д. определены в `variables.css` (нужны для
первого кадра, до того как отработает JS — иначе будет "моргание" темы). Файл
`Styles/input.css` мапит их на Tailwind-токены:

```css
@theme inline {
  --color-primary: hsl(var(--scarna-primary));
  --radius-scarna-md: var(--scarna-radius-md);
}
```

Дальше `ScarnaThemeProvider` при инициализации и при каждой смене режима вызывает
`theme.js: applyTheme(vars, isDark)`, который делает `document.documentElement.style
.setProperty("--scarna-primary", "...")` и переключает `data-theme="dark"/"light"`.
Поскольку Tailwind-утилиты (`bg-primary`, `text-primary`, `rounded-scarna-md`) уже
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
(см. `ScarnaColorVariantClasses.cs`, `ScarnaItem.razor`), а не через интерполяцию.

## Пересборка CSS

Нужна только при разработке самой библиотеки (добавили новый компонент/класс) —
не в проектах, которые ScarnaUI подключают. **Запускать вручную не нужно** —
`ScarnaUI.csproj` сам вызывает `build-css.ps1` перед каждой своей сборкой
(`Target Name="BuildTailwindCss" BeforeTargets="CoreCompile"`, см. конец файла):
любой `dotnet build`/`dotnet run`/F5 в Visual Studio для `ScarnaUI` или для
`ScarnaUI.Demo` (он ссылается на `ScarnaUI`, значит сначала собирается и он)
сам пересобирает `scarna.css`. Этот Target объявлен только в `ScarnaUI.csproj`,
поэтому на проекты-потребители не переносится — они его никогда не запускают.

Ручной запуск по-прежнему доступен, если нужен (например, чтобы увидеть
изменение CSS в уже запущенном `dotnet watch`, не дожидаясь его собственного
цикла пересборки):

```bash
./src/ScarnaUI/ScarnaUI/build-css.ps1          # собрать один раз
./src/ScarnaUI/ScarnaUI/build-css.ps1 -Watch   # пересобирать при изменениях
```

Скрипт (что при автозапуске из MSBuild, что вручную) сам скачивает
`tailwindcss-windows-x64.exe` (standalone-бинарник, без Node.js и npm) при первом
запуске, кладёт его в `tools/` (в git не коммитится, см. `.gitignore`).
