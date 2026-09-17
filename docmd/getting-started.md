# Установка и базовые классы

## Файлы
- `src/ScarnaUI/ScarnaUI/ScarnaUI.csproj` — сама библиотека (Razor Class Library)
- `src/ScarnaUI/ScarnaUI/Components/Base/ScarnaComponentBase.cs`
- `src/ScarnaUI/ScarnaUI/Components/Base/ScarnaFormComponentBase.cs`
- `src/ScarnaUI/ScarnaUI/Components/Base/CssBuilder.cs`
- `src/ScarnaUI/ScarnaUI/Components/Base/ScarnaColorVariantClasses.cs`
- `src/ScarnaUI/ScarnaUI/Components/Base/ScarnaEnums.cs` (`Color`, `Size`, `Radius`, `Variant`)
- `src/ScarnaUI/ScarnaUI/Services/ScarnaUiServiceCollectionExtensions.cs`
- `src/ScarnaUI/ScarnaUI/_Imports.razor`

## Подключение к проекту

Два способа: `ProjectReference` (ниже, обычный способ для разработки внутри
монорепозитория) или локальный NuGet-пакет (`dotnet add package` — удобно, если
проект-потребитель лежит отдельно и не должен видеть исходники ScarnaUI). Для
второго — `./pack-local.ps1` в корне репозитория собирает `.nupkg`, кладёт его
в `./nupkgs` и регистрирует эту папку как локальный источник NuGet (один раз):

```bash
./pack-local.ps1                     # версия по умолчанию 1.0.0-local
./pack-local.ps1 -Version 1.2.3      # конкретная версия
```

Версия по умолчанию **фиксированная** (не новая на каждый запуск) — специально:
NuGet кэширует пакеты по номеру версии в `~/.nuget/packages`, и с плавающей
версией пришлось бы вручную переставлять её у потребителя после каждой
пересборки. Вместо этого скрипт сам чистит в этом кэше именно версию
`scarnaui/<version>` при каждом запуске — так `dotnet restore` у потребителя
после `./pack-local.ps1` всегда подтягивает свежую сборку под тем же номером:

```bash
dotnet add package ScarnaUI --version 1.0.0-local --source ScarnaUILocal
```

1. Добавить ссылку на проект ScarnaUI (`dotnet add reference ...ScarnaUI.csproj`).
2. В `Program.cs` зарегистрировать сервисы:

```csharp
builder.Services.AddScarnaUI(); // диалоги + тосты
```

3. В `App.razor` подключить собранный CSS (лежит в самой библиотеке, приезжает как
   статический ресурс RCL под `_content/ScarnaUI/...`):

```html
<link rel="stylesheet" href="_content/ScarnaUI/css/scarna/scarna.css" />
```

3.1. **Обязательно** — в `App.razor`, в `<head>`, до `<ResourcePreloader />`/содержимого,
   добавить синхронный скрипт против "вспышки светлым" (FOUC). Без него страница на
   первом кадре всегда рисуется светлой (CSS-переменные по умолчанию — светлая тема),
   и только после того, как Blazor Server подключит circuit и у `ScarnaThemeProvider`
   отработает первый `OnAfterRenderAsync` (JS round-trip), тёмная тема реально
   применяется — если у пользователя выбрана тёмная, будет заметная вспышка светлым
   перед переключением:

```html
<script>
    (function () {
        try {
            var stored = localStorage.getItem("scarna-theme-mode");
            var isDark = stored === "dark"
                || (stored !== "light" && window.matchMedia("(prefers-color-scheme: dark)").matches);
            document.documentElement.setAttribute("data-theme", isDark ? "dark" : "light");
            document.documentElement.style.colorScheme = isDark ? "dark" : "light";
        } catch (e) {}
    })();
</script>
```

   Ключ `localStorage` (`"scarna-theme-mode"`) — тот же, что использует `theme.js`,
   поэтому дальнейшее применение темы в `ScarnaThemeProvider` просто подтверждает уже
   выставленное значение, а не меняет его. Готовый пример — в
   `src/ScarnaUI/ScarnaUI.Demo/Components/App.razor`.

4. В `MainLayout.razor` обернуть содержимое провайдерами (порядок важен — Popover должен
   оборачивать `@Body`, Dialog/Snackbar можно разместить рядом):

```razor
<ScarnaThemeProvider>
    <ScarnaPopoverProvider>
        @Body
    </ScarnaPopoverProvider>

    <ScarnaDialogProvider />
    <ScarnaSnackbarProvider />
</ScarnaThemeProvider>
```

5. В `_Imports.razor` своего проекта добавить `@using` на нужные неймспейсы
   (`ScarnaUI.Components.Base`, `.Theme`, `.Layout`, `.Actions`, `.Forms`, `.Navigation`,
   `.DataDisplay`, `.Overlays`, `ScarnaUI.Services`).

Пересборка CSS самой библиотеки (нужна только при разработке ScarnaUI, не в проектах-
потребителях) происходит автоматически при каждой сборке `ScarnaUI.csproj` — см.
`src/ScarnaUI/ScarnaUI/build-css.ps1`, MSBuild-таргет `BuildTailwindCss` в конце
`ScarnaUI.csproj` и раздел про сборку в [theme-provider.md](theme-provider.md).

## ScarnaComponentBase

Базовый класс **всех** компонентов библиотеки. Даёт параметры, которые Blazor-
разработчик ожидает от любого UI-компонента (как в MudBlazor):

| Параметр | Тип | Назначение |
|---|---|---|
| `Class` | `string?` | Дополнительные CSS-классы, добавляются к классам компонента |
| `Style` | `string?` | Инлайн-стили на корневой элемент (добавляются последними — переопределяют всё остальное из этой таблицы) |
| `Width` / `Height` | `string?` | CSS width/height корневого элемента ("240px", "50%", "20rem", просто "240"...) |
| `MinWidth` / `MinHeight` | `string?` | CSS min-width/min-height |
| `MaxWidth` / `MaxHeight` | `string?` | CSS max-width/max-height |
| `UserAttributes` | `IReadOnlyDictionary<string, object>?` | Любые непойманные атрибуты (`data-*`, `id`, `onclick`...) прозрачно летят на корневой HTML-элемент |

Отступов (`Padding`/`Margin`) среди этих параметров намеренно нет — регулируются
CSS-классами через `Class`, а не inline style: `px-*`/`py-*`/`mx-*`/`my-*` — обычные
Tailwind-утилиты, плюс фирменные `pa-*`/`ma-*` (и отрицательные `ma-nN`/`mx-nN`/`my-nN`)
в стиле MudBlazor — см. [custom-classes.md](custom-classes.md#отступы-pa--ma--mud-стиль).
Классы, в отличие от inline style, можно комбинировать с вариантами (`sm:pa-6`, `hover:ma-2`).

Голое число без единицы (`Width="240"`) автоматически получает `px` — это не
мелочь: `width: 240;` (без единицы) невалиден как CSS, и браузер **тихо** отбрасывает
всю декларацию целиком, а не только её. Значения уже с единицей (`"50%"`, `"20rem"`)
или CSS-функцией (`calc(...)`) не трогаются.

Наследник рендерит `style="@RootStyle"` (не `@Style` напрямую) — `RootStyle` уже
собирает Width/Height/Min\*/Max\*/Style в одну строку:

```razor
<div class="@Classes" style="@RootStyle" @attributes="UserAttributes">...</div>
```

Если у компонента уже есть свой вычисляемый inline style (градиент заливки у
`ScarnaSlider`, анимируемая ширина у `ScarnaDrawer` и т.п.), используйте `BuildStyle(ownStyle)`
вместо `RootStyle` — он подмешает Width/Height/Min\*/Max\*/Style вокруг переданной строки:

```csharp
private string? CombinedStyle => BuildStyle($"background: {myGradient};");
```

Если наследник вдобавок **override'ит** сами Width/Height под свою логику (как
`ScarnaDrawer` — там это размер раскрытой панели, а не "просто ширина/высота
корня", и относится то к width, то к height в зависимости от `Anchor`) — передайте
`includeWidthHeight: false`, иначе `BuildStyle` добавит ещё и "сырые" Width/Height
по обеим осям поверх уже посчитанного `ownStyle`, задвоив их и наложив лишнее
ограничение на ось, которую компонент не использует:

```csharp
private string? CombinedStyle => BuildStyle(sizeStyle, includeWidthHeight: false);
```

> Blazor не поддерживает скрытие параметров через `new` (в отличие от обычных
> C#-членов) — рантайм требует ровно один параметр с данным именем на весь
> наследственный путь компонента, иначе падает с ошибкой
> `declares more than one parameter matching the name ...`. Поэтому у пары
> компонентов с более узким/другим смыслом того же названия параметр не скрыт,
> а по-настоящему **переопределён** (`override`, тот же тип, другое значение по
> умолчанию) — `ScarnaDrawer.Width`/`Height` (размер раскрытой панели, участвует
> в анимации) и `ScarnaLayout.MinHeight` (по умолчанию `"100vh"`, а не `null`).
> Там, где тип другой и override физически невозможен — `ScarnaContainer` — общий
> параметр называется иначе: пресет брейкпоинтов называется `Breakpoint`, а не
> `MaxWidth` (тот остался за унаследованным произвольным CSS-значением).

Компонент-наследник собирает свои классы через `CssBuilder`:

```csharp
private string Classes => CssBuilder.Default("scarna-card flex flex-col")
    .AddClass("border border-divider", Bordered)   // класс только если Bordered == true
    .AddClass(Class)                                 // пользовательские классы — последними
    .ToString();
```

## ScarnaFormComponentBase\<T\>

Базовый класс для полей ввода (`ScarnaInput`, `ScarnaCheckbox`, `ScarnaSwitch` и т.д.).
Добавляет то, что нужно любому полю формы:

- `Value` / `ValueChanged` / `ValueExpression` — механика `@bind-Value` "из коробки";
- каскадный `EditContext?` — если поле стоит внутри `<EditForm>`, оно регистрируется
  в форме и участвует в валидации;
- `ErrorText` / `Error` — ручная подсветка ошибки (даже без `EditForm`);
- `IsInvalid` — true, если есть ручная ошибка ИЛИ `EditContext` считает поле невалидным.

Наследник вызывает `await SetValueAsync(newValue)` при изменении значения пользователем —
это одновременно обновит `Value`, поднимет `ValueChanged` и уведомит `EditContext`.

## Анимации

В `Styles/input.css` определены три готовые анимации входа (только "enter" —
исчезновение в Blazor происходит мгновенно вместе с `@if`/удалением из DOM,
полноценный exit-transition потребовал бы отдельной JS-хореографии):

| Класс | Где используется | Эффект |
|---|---|---|
| `animate-scarna-fade-in` | подложка диалога, поповер/тултип | плавное появление (opacity) |
| `animate-scarna-scale-in` | панель диалога | появление с лёгким увеличением (0.95 → 1) |
| `animate-scarna-slide-up-in` | тост | появление со сдвигом снизу вверх |

Уважают `prefers-reduced-motion: reduce` (анимация схлопывается до 0.01мс). Для
поповера/тултипа класс добавляется не в разметке, а из `popover.js` в момент,
когда элемент реально становится видимым (`popoverEl.classList.add(...)`) —
если добавить класс прямо в `.razor`, анимация начнёт отсчёт времени сразу при
создании DOM-узла, а не в момент показа, и на Blazor Server (с задержкой
SignalR round-trip) анимация может "проиграться" ещё до того, как элемент
станет видимым.

## ⚠️ Локаль сервера и числа в CSS

При сборке `style="..."` из чисел (`double`/`decimal`) **обязательно** форматируйте
их через `CultureInfo.InvariantCulture`. ASP.NET Core по умолчанию использует
культуру ОС — на сервере с русской локалью `$"{0.5}rem"` даст `"0,5rem"`
(запятая вместо точки), это невалидный CSS, и браузer **молча** отбрасывает всё
объявление целиком — визуально это выглядит как "пропавшие" отступы/паддинги.
Так уже было сломано в `ScarnaStack`/`ScarnaGrid` (расчёт `gap` из `Spacing`) —
исправлено на `(Spacing * 0.25).ToString(CultureInfo.InvariantCulture)`. Класс-строки
(`"bg-primary"` и т.п.) эта проблема не касается — они не содержат чисел.

## ScarnaColorVariantClasses

Общая таблица "цвет × вариант заливки" (`Color` × `Variant`) — `Get(color, variant)` —
которой пользуются `ScarnaButton`, `ScarnaChip`, `ScarnaBadge`, `ScarnaAvatar`. Классы в
ней записаны **буквально** (не через `$"bg-{color}"`) — так их видит статический
анализатор Tailwind при сборке CSS. Подробнее про этот нюанс — в
[theme-provider.md](theme-provider.md#почему-классы-написаны-буквально).

Для компонентов-"поверхностей" без варианта заливки (`ScarnaList`, `ScarnaCard`,
`ScarnaPaper`, `ScarnaBottomMenu`, `ScarnaTable`, `ScarnaExpansionPanel`) — отдельный
`Surface(color, defaultClass = "bg-content1")`: `Color.Default` даёт `defaultClass`
(нейтральную поверхность темы — обычно `bg-content1`, а не `bg-default`, как у `Get`),
`Color.Transparent` — `bg-transparent`, остальные — сплошную заливку цветом
(`bg-primary text-primary-foreground` и т.п.).

`Color.Transparent` существует только для `Surface` — в таблице `Get` (Button/Chip/
Badge/...) он не поддерживается: там для "невидимого" фона уже есть `Variant.Light`/
`Variant.Bordered`.
