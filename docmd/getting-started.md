# Установка и базовые классы

## Файлы
- `src/NorthUI/NorthUI/NorthUI.csproj` — сама библиотека (Razor Class Library)
- `src/NorthUI/NorthUI/Components/Base/NorthComponentBase.cs`
- `src/NorthUI/NorthUI/Components/Base/NorthFormComponentBase.cs`
- `src/NorthUI/NorthUI/Components/Base/CssBuilder.cs`
- `src/NorthUI/NorthUI/Components/Base/NorthColorVariantClasses.cs`
- `src/NorthUI/NorthUI/Components/Base/NorthEnums.cs` (`NorthColor`, `NorthSize`, `NorthRadius`, `NorthVariant`)
- `src/NorthUI/NorthUI/Services/NorthUiServiceCollectionExtensions.cs`
- `src/NorthUI/NorthUI/_Imports.razor`

## Подключение к проекту

1. Добавить ссылку на проект NorthUI (`dotnet add reference ...NorthUI.csproj`).
2. В `Program.cs` зарегистрировать сервисы:

```csharp
builder.Services.AddNorthUI(); // диалоги + тосты
```

3. В `App.razor` подключить собранный CSS (лежит в самой библиотеке, приезжает как
   статический ресурс RCL под `_content/NorthUI/...`):

```html
<link rel="stylesheet" href="_content/NorthUI/css/north/north.css" />
```

3.1. **Обязательно** — в `App.razor`, в `<head>`, до `<ResourcePreloader />`/содержимого,
   добавить синхронный скрипт против "вспышки светлым" (FOUC). Без него страница на
   первом кадре всегда рисуется светлой (CSS-переменные по умолчанию — светлая тема),
   и только после того, как Blazor Server подключит circuit и у `NorthThemeProvider`
   отработает первый `OnAfterRenderAsync` (JS round-trip), тёмная тема реально
   применяется — если у пользователя выбрана тёмная, будет заметная вспышка светлым
   перед переключением:

```html
<script>
    (function () {
        try {
            var stored = localStorage.getItem("north-theme-mode");
            var isDark = stored === "dark"
                || (stored !== "light" && window.matchMedia("(prefers-color-scheme: dark)").matches);
            document.documentElement.setAttribute("data-theme", isDark ? "dark" : "light");
            document.documentElement.style.colorScheme = isDark ? "dark" : "light";
        } catch (e) {}
    })();
</script>
```

   Ключ `localStorage` (`"north-theme-mode"`) — тот же, что использует `theme.js`,
   поэтому дальнейшее применение темы в `NorthThemeProvider` просто подтверждает уже
   выставленное значение, а не меняет его. Готовый пример — в
   `src/NorthUI/NorthUI.Demo/Components/App.razor`.

4. В `MainLayout.razor` обернуть содержимое провайдерами (порядок важен — Popover должен
   оборачивать `@Body`, Dialog/Snackbar можно разместить рядом):

```razor
<NorthThemeProvider>
    <NorthPopoverProvider>
        @Body
    </NorthPopoverProvider>

    <NorthDialogProvider />
    <NorthSnackbarProvider />
</NorthThemeProvider>
```

5. В `_Imports.razor` своего проекта добавить `@using` на нужные неймспейсы
   (`NorthUI.Components.Base`, `.Theme`, `.Layout`, `.Actions`, `.Forms`, `.Navigation`,
   `.DataDisplay`, `.Overlays`, `NorthUI.Services`).

Пересборка CSS самой библиотеки (нужна только при разработке NorthUI, не в проектах-
потребителях) — см. `src/NorthUI/NorthUI/build-css.ps1` и раздел про сборку в
[theme-provider.md](theme-provider.md).

## NorthComponentBase

Базовый класс **всех** компонентов библиотеки. Даёт три параметра, которые Blazor-
разработчик ожидает от любого UI-компонента (как в MudBlazor):

| Параметр | Тип | Назначение |
|---|---|---|
| `Class` | `string?` | Дополнительные CSS-классы, добавляются к классам компонента |
| `Style` | `string?` | Инлайн-стили на корневой элемент |
| `UserAttributes` | `IReadOnlyDictionary<string, object>?` | Любые непойманные атрибуты (`data-*`, `id`, `onclick`...) прозрачно летят на корневой HTML-элемент |

Компонент-наследник собирает свои классы через `CssBuilder`:

```csharp
private string Classes => CssBuilder.Default("north-card flex flex-col")
    .AddClass("border border-divider", Bordered)   // класс только если Bordered == true
    .AddClass(Class)                                 // пользовательские классы — последними
    .ToString();
```

## NorthFormComponentBase\<T\>

Базовый класс для полей ввода (`NorthInput`, `NorthCheckbox`, `NorthSwitch` и т.д.).
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
| `animate-north-fade-in` | подложка диалога, поповер/тултип | плавное появление (opacity) |
| `animate-north-scale-in` | панель диалога | появление с лёгким увеличением (0.95 → 1) |
| `animate-north-slide-up-in` | тост | появление со сдвигом снизу вверх |

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
Так уже было сломано в `NorthStack`/`NorthGrid` (расчёт `gap` из `Spacing`) —
исправлено на `(Spacing * 0.25).ToString(CultureInfo.InvariantCulture)`. Класс-строки
(`"bg-primary"` и т.п.) эта проблема не касается — они не содержат чисел.

## NorthColorVariantClasses

Общая таблица "цвет × вариант заливки" (`NorthColor` × `NorthVariant`), которой
пользуются `NorthButton`, `NorthChip`, `NorthBadge`, `NorthAvatar`. Классы в ней
записаны **буквально** (не через `$"bg-{color}"`) — так их видит статический
анализатор Tailwind при сборке CSS. Подробнее про этот нюанс — в
[theme-provider.md](theme-provider.md#почему-классы-написаны-буквально).
