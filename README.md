<div align="center">

# ScarnaUI

**Blazor-компоненты с удобством MudBlazor и дизайном HeroUI**

Светлая и тёмная темы · тема меняется в рантайме · Tailwind CSS без Node.js · 50+ компонентов

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![Blazor](https://img.shields.io/badge/Blazor-Server-5C2D91?logo=blazor&logoColor=white)
![Tailwind](https://img.shields.io/badge/Tailwind-v4-06B6D4?logo=tailwindcss&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

</div>

---

## Зачем это нужно

CSS-оверрайды поверх MudBlazor упираются в потолок: разметка чужая, и часть эффектов
(например, pill-фон под активной вкладкой вместо 2px-подчёркивания) физически недостижима
без переписывания DOM. ScarnaUI — библиотека с нуля, где совмещено лучшее из двух миров:

| От MudBlazor — **удобство** | От HeroUI — **дизайн** |
|---|---|
| `@bind-Value` и интеграция с `EditForm` | Палитра, радиусы, тени, шрифт Inter |
| Императивные сервисы: `ShowAsync<T>()`, `Add("...")` | Мягкие поверхности, pill-вкладки, аккуратные состояния |
| `Class` / `Style` / любые атрибуты на каждом компоненте | Токены темы, которые легко менять |
| Знакомые имена: `Stack`, `Grid`, `Drawer`, `NavMenu`… | Светлая и тёмная темы «из коробки» |

## Возможности

- **50+ компонентов** — кнопки, формы, списки, таблицы, диалоги, меню, навигация, layout.
- **Тема в рантайме** — цвета и радиусы живут в CSS-переменных; смена применяется мгновенно, без пересборки. Выбор light / dark / system запоминается в `localStorage`.
- **Единый язык параметров** — везде одни и те же `Color`, `Variant`, `Size`, `Radius`.
- **Tailwind v4, собранный standalone-бинарником** — готовый `scarna.css` приезжает вместе с пакетом, Node.js у потребителя не нужен.
- **Spacing-утилиты в стиле MudBlazor** — `pa-4`, `mx-n2`, `px-0!`, а также `scrollable`, `scarna-width-full`.
- **Доступность** — `aria-*`, фокус-ловушка в диалогах, `inert` у закрытых панелей.

## Быстрый старт

**1. Установка.** Пока пакет собирается локально:

```powershell
./pack-local.ps1
dotnet add package ScarnaUI --version 1.0.0-local --source ScarnaUILocal
```

Либо подключите проект как `ProjectReference`.

**2. Сервисы** — `Program.cs`:

```csharp
builder.Services.AddScarnaUI(); // диалоги, тосты, навигация
```

**3. Стили** — `App.razor`, в `<head>`:

```html
<link rel="stylesheet" href="_content/ScarnaUI/css/scarna/scarna.css" />
```

> Чтобы страница не мигала светлой темой на первом кадре, добавьте ещё маленький
> синхронный скрипт — он описан в [getting-started.md](docmd/getting-started.md).

**4. Провайдеры** — `MainLayout.razor`:

```razor
@inherits LayoutComponentBase

<ScarnaThemeProvider>
    <ScarnaPopoverProvider>
        <ScarnaLayout>
            <ScarnaNavBar>
                <span class="font-semibold">Моё приложение</span>
                <ScarnaThemeSwitch />
            </ScarnaNavBar>

            <main class="p-6">@Body</main>
        </ScarnaLayout>
    </ScarnaPopoverProvider>

    <ScarnaDialogProvider />
    <ScarnaSnackbarProvider />
</ScarnaThemeProvider>
```

**5. `_Imports.razor`** — добавьте `@using` на нужные неймспейсы:
`ScarnaUI.Components.Base`, `.Theme`, `.Layout`, `.Actions`, `.Forms`, `.Navigation`,
`.DataDisplay`, `.Overlays` и `ScarnaUI.Services`.

## Примеры

### Форма

```razor
<EditForm Model="model" OnValidSubmit="Save">
    <DataAnnotationsValidator />
    <ScarnaStack Spacing="4">
        <ScarnaInput Label="Email" @bind-Value="model.Email" ErrorText="Введите корректный email" />
        <ScarnaPasswordBox Label="Пароль" @bind-Value="model.Password" />
        <ScarnaNumericField Label="Возраст" @bind-Value="model.Age" Min="18" Max="99" />
        <ScarnaSwitch @bind-Value="model.Notify">Присылать уведомления</ScarnaSwitch>
        <ScarnaButton Color="Color.Primary" Type="submit">Сохранить</ScarnaButton>
    </ScarnaStack>
</EditForm>
```

### Диалог подтверждения

```razor
@inject IScarnaDialogService DialogService

<ScarnaButton Color="Color.Danger" OnClick="DeleteAsync">Удалить</ScarnaButton>

@code {
    private async Task DeleteAsync()
    {
        var result = await DialogService.ShowAsync<ConfirmDeleteDialog>(
            new ScarnaDialogOptions { Size = ScarnaDialogSize.Small });

        if (!result.Cancelled) { /* подтверждено */ }
    }
}
```

### Уведомления

```razor
@inject IToastManager Toasts

@code {
    void Saved() => Toasts.Add("Изменения сохранены", ScarnaToastSeverity.Success);
}
```

### Боковое меню

```razor
<ScarnaLayout Flex="false" Class="flex">
    <ScarnaDrawer Open="true" Variant="ScarnaDrawerVariant.Persistent" Width="16rem">
        <ScarnaNavMenu Class="p-2">
            <ScarnaNavLink Href="/">Главная</ScarnaNavLink>
            <ScarnaNavLink Href="/orders">Заказы</ScarnaNavLink>
        </ScarnaNavMenu>
    </ScarnaDrawer>
    <main class="flex-1 p-6">@Body</main>
</ScarnaLayout>
```

Активный пункт `ScarnaNavLink` подсвечивается сам — по текущему URL.

### Своя тема

```razor
<ScarnaThemeProvider Theme="_theme">@Body</ScarnaThemeProvider>

@code {
    private readonly ScarnaTheme _theme = new();

    protected override void OnInitialized()
    {
        _theme.Light.Primary = "142 71% 45%"; // HSL-триплет — зелёный вместо синего
        _theme.Dark.Primary  = "142 71% 55%";
    }
}
```

## Компоненты

| Категория | Компоненты |
|---|---|
| **Тема** | `ScarnaThemeProvider`, `ScarnaThemeSwitch` |
| **Layout** | `ScarnaLayout`, `ScarnaContainer`, `ScarnaStack`, `ScarnaGrid` / `ScarnaItem`, `ScarnaNavBar`, `ScarnaDrawer`, `ScarnaBottomMenu`, `ScarnaNavMenu` / `ScarnaNavLink` |
| **Actions** | `ScarnaButton`, `ScarnaToggleButton` |
| **Forms** | `ScarnaInput`, `ScarnaTextArea`, `ScarnaPasswordBox`, `ScarnaNumericField`, `ScarnaSlider`, `ScarnaTimeField`, `ScarnaDateTimePicker`, `ScarnaCheckbox`, `ScarnaSwitch`, `ScarnaSelect`, `ScarnaSelectGroup` |
| **Data display** | `ScarnaText`, `ScarnaCard`, `ScarnaPaper`, `ScarnaList`, `ScarnaTable`, `ScarnaExpansionPanel`, `ScarnaAvatar`, `ScarnaImage`, `ScarnaBadge`, `ScarnaChip`, `ScarnaTag`, `ScarnaProgress`, `ScarnaSpinner`, `ScarnaDivider`, `ScarnaSeparator`, `ScarnaCodeBlock` |
| **Navigation** | `ScarnaTabs`, `ScarnaMenu` |
| **Overlays** | `ScarnaPopover`, `ScarnaTooltip`, `ScarnaDialog` + `IScarnaDialogService`, `IToastManager` |

Документация по каждому компоненту — в [`docmd/`](docmd/index.md).

## Живая документация

В репозитории есть демо-проект с интерактивной документацией в духе mudblazor.com:
живой пример на каждый компонент, кнопка «Показать код» и таблица параметров.

```powershell
dotnet run --project src/ScarnaUI/ScarnaUI.Demo
```

| Страница | Что там |
|---|---|
| `/componentlib` | Документация по компонентам, боковое меню на `ScarnaDrawer` |
| `/components` | Витрина компонентов на одной странице |
| `/showcase/main-layout` | Layout wireframes: AppBar + Drawer (Temporary / Persistent / Mini) |
| `/showcase/stackoverflow`, `/showcase/tasks` | Примеры «настоящих» страниц на ScarnaUI |

### Готовая сборка демо

```powershell
./pack-local.ps1
```

Скрипт собирает NuGet-пакет и кладёт демо в `buildDemo/` одним self-contained exe —
.NET на целевой машине не нужен. Рядом с exe остаются `wwwroot` и манифест статики
(так устроен ASP.NET Core), поэтому переносите папку целиком.
Флаги: `-SkipDemo`, `-DemoRuntime linux-x64`, `-Version 1.2.3`.

## Структура репозитория

```text
src/ScarnaUI/
├── ScarnaUI/          # сама библиотека (Razor Class Library)
│   ├── Components/    #   Base · Theme · Layout · Actions · Forms · DataDisplay · Navigation · Overlays
│   ├── Services/      #   диалоги, тосты, навигация
│   ├── Styles/        #   input.css — вход Tailwind и кастомные утилиты
│   └── wwwroot/       #   готовый scarna.css и JS-модули (popover, dialog, theme…)
└── ScarnaUI.Demo/     # полигон и живая документация
docmd/                 # markdown-документация по каждому компоненту
pack-local.ps1         # сборка NuGet-пакета и демо
```

## Как устроено под капотом

- **База** — `ScarnaComponentBase` даёт `Class`, `Style`, размеры (`Width` / `Height` / `Min*` / `Max*`) и catch-all для произвольных HTML-атрибутов; `ScarnaFormComponentBase<T>` добавляет `Value` / `ValueChanged` / `ValueExpression` и работу с `EditContext`.
- **Порталы** — `ScarnaPopoverProvider` рендерит выпадающие списки, меню и тултипы в одном месте поверх страницы; `popover.js` считает позицию (flip/shift) без внешних зависимостей.
- **Тема** — токены из `ScarnaTheme` переносятся в CSS-переменные на `<html>`, поэтому переключение мгновенное.
- **Стили** — Tailwind v4 собирается standalone-бинарником при сборке библиотеки; потребителю достаточно подключить один CSS.

Подробнее — [установка](docmd/getting-started.md), [тема](docmd/theme-provider.md),
[кастомные CSS-классы](docmd/custom-classes.md).

## Лицензия

[MIT](LICENSE) © 2026 Ilya Belousov
