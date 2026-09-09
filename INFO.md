# План: своя UI-библиотека Blazor-компонентов (North) — DX от MudBlazor, дизайн от HeroUI
ПОСТРОИТЬ НА ОСНОВЕ Tailwind css
## Контекст

Направление изменилось: вместо CSS-скина поверх MudBlazor — своя библиотека компонентов
с нуля, в отдельном проекте. Причина: CSS-оверрайды упёрлись в потолок — разметка чужая,
поэтому часть эффектов физически недостижима без переписывания DOM (как было с табами:
`.mud-tab-slider` — это 2px подчёркивание, и как его ни красить, оно не станет pill-фоном
без изменения самой структуры компонента).

Что переносим 1:1 из MudBlazor — не код, а **паттерны взаимодействия**:
- `@bind-Value` / `EventCallback<T>` / `ValueExpression` для интеграции с `EditForm` и другие параметры.
- Провайдеры-сервисы для императивных вызовов (`IDialogService.ShowAsync<T>()`,
  `ISnackbar.Add(...)`) вместо ручного управления видимостью назвать ToastManager
- `Class` / `Style` / произвольные HTML-атрибуты, прозрачно прокидываемые на корневой элемент
- Единый `CascadingParameter`-провайдер темы (тёмная/светлая, токены)

Что переносим по дизайну — HeroUI: уже есть готовые, сверенные с живым heroui.com токены (радиусы, тени, палитра,
шрифт Inter) — это готовый фундамент дизайн-системы, его переиспользуем с доработками. Чтобы было настраиваемо, но не выглядело как под копирку.(важно!).

!!! ВАЖНО!
Темы(радиусы, все цвета) должны настраиваться просто. И легко менятьтся в рантайме.

- Библиотека — **отдельный проект** (`NorthUI`, Razor Class Library), `ThemeCollection`
  ссылается на него как на обычный project reference.
- Префикс компонентов — **`North`** (`NorthButton`, `NorthInput`, `NorthDialog`, ...).



## Структура решения
NorthUI.Demo - blazor проект для тестирования отрисовки и работы. Полигон.
NorthUI - (новая библиотека, Microsoft.NET.Sdk.Razor)
```
    Components/
      Base/
        NorthComponentBase.cs        // Class/Style/UserAttributes, CssBuilder-хелпер
        NorthFormComponentBase.cs    // + Value/ValueChanged/ValueExpression, EditContext
      Theme/
        NorthThemeProvider.razor      // каскадная тема, тёмная/светлая (замена MainLayout-логики)
        NorthTheme.cs                  // C#-модель токенов (радиусы/цвета/шрифт)
      Layout/
        NorthContainer.razor, NorthGrid.razor, NorthItem.razor, NorthStack.razor
        NorthAppBar.razor, NorthDrawer.razor, NorthNavMenu.razor, NorthNavLink.razor
      Navigation/
        NorthTabs.razor, NorthTabPanel.razor, NorthMenu.razor, NorthMenuItem.razor
      Overlays/
        NorthPopoverProvider.razor + NorthPopover.razor   // портал + позиционирование
        NorthDialogProvider.razor + NorthDialog.razor
        NorthSnackbarProvider.razor
        NorthDatePicker.razor, NorthTimePicker.razor, NorthColorPicker.razor

        ТУТ ДРУГИЕ КОМПОНЕНТЫ ПО КАТЕГОРИЯМ. ВСЕ ОСНОВНЫЕ КОНТРОЛЫ ИЗ heroui.
    Services/
      INorthDialogService.cs / NorthDialogService.cs
      INorthSnackbarService.cs / NorthSnackbarService.cs
    wwwroot/
      css/north/               // variables.css + colors.css 
      css/north/components/category/controlname.css - тут css для компонента
      js/north/
        popover.js                // расчёт позиции (flip/shift), без внешних зависимостей
        dialog.js                  // focus trap, Escape, блокировка скролла
        theme.js                   // перенос текущей wwwroot/js/theme.js как есть
```


## Архитектурные примитивы (строятся один раз, переиспользуются везде)

1. **`NorthComponentBase`** — базовый класс: `[Parameter] Class`, `[Parameter] Style`,
   `[Parameter(CaptureUnmatchedValues=true)] UserAttributes`, маленький `CssBuilder`
   (аналог внутреннего хелпера MudBlazor для сборки `class="North-btn North-btn--primary ..."`).
2. **`NorthFormComponentBase<T>`** — добавляет `Value`/`ValueChanged`/`ValueExpression` +
   `[CascadingParameter] EditContext?`, регистрацию поля в форме и подсветку ошибок валидации.
   Даёт `@bind-Value` "из коробки" на всех формах-компонентах, как в MudBlazor.
3. **`NorthPopoverProvider`** — один корневой компонент (кладётся в layout один раз, как
   `MudPopoverProvider`), рендерит портал-контейнер в конце `<body>`. `NorthPopover.razor` —
   переиспользуемый внутренний блок с JS-модулем `popover.js` (позиционирование относительно
   якоря, flip при нехватке места). На нём строятся: `NorthSelect`, `NorthAutocomplete`,
   `NorthMenu`, `NorthTooltip`, `NorthDatePicker`/`NorthTimePicker`/`NorthColorPicker`.
4. **`NorthDialogProvider` + `INorthDialogService`** — сервис с `ShowAsync<TComponent>(params)`,
   как `IDialogService` в MudBlazor: инжектится в любой компонент, открывает модалку поверх
   всего приложения без ручного `bool _open` в каждом месте использования.
5. **`ToastManager` + `IToastManager`** — `Add(message, severity)`, очередь
   тостов с авто-скрытием по таймеру (чистый C#, JS не нужен).
6. **`NorthThemeProvider`** 


Важно продумать механизм подключения. Мне нравится как в MudBlazor.