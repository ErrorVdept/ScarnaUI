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

Базовый класс **всех** компонентов библиотеки. Даёт три параметра, которые Blazor-
разработчик ожидает от любого UI-компонента (как в MudBlazor):

| Параметр | Тип | Назначение |
|---|---|---|
| `Class` | `string?` | Дополнительные CSS-классы, добавляются к классам компонента |
| `Style` | `string?` | Инлайн-стили на корневой элемент |
| `UserAttributes` | `IReadOnlyDictionary<string, object>?` | Любые непойманные атрибуты (`data-*`, `id`, `onclick`...) прозрачно летят на корневой HTML-элемент |

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

Общая таблица "цвет × вариант заливки" (`Color` × `Variant`), которой
пользуются `ScarnaButton`, `ScarnaChip`, `ScarnaBadge`, `ScarnaAvatar`. Классы в ней
записаны **буквально** (не через `$"bg-{color}"`) — так их видит статический
анализатор Tailwind при сборке CSS. Подробнее про этот нюанс — в
[theme-provider.md](theme-provider.md#почему-классы-написаны-буквально).
