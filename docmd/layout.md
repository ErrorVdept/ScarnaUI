# ScarnaLayout

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Layout/ScarnaLayout.razor`

## Описание

Корневая обёртка приложения (аналог `MudLayout`) — задаёт минимальную высоту экрана
и базовые цвета темы (`bg-background text-foreground`) для всего, что внутри
(`ScarnaNavBar` + `ScarnaDrawer` + `main`). Кладётся один раз в `MainLayout.razor`
вместо `<div class="min-h-screen bg-background text-foreground">`.

Сама по себе не содержит логики — как и `MudLayout`, просто холст для остальных
layout-компонентов.

## Пример использования

```razor
@* MainLayout.razor *@
<ScarnaThemeProvider>
    <ScarnaPopoverProvider>
        <ScarnaLayout>
            <ScarnaNavBar>...</ScarnaNavBar>
            <main class="py-8">
                @Body
            </main>
        </ScarnaLayout>
    </ScarnaPopoverProvider>

    <ScarnaDialogProvider />
    <ScarnaSnackbarProvider />
</ScarnaThemeProvider>
```

Sticky-футер (за счёт `Flex="true"` по умолчанию — `main` растягивается через
`flex-1`, `footer` прижимается к низу):

```razor
<ScarnaLayout>
    <ScarnaNavBar>...</ScarnaNavBar>
    <main class="flex-1 py-8">@Body</main>
    <footer class="border-t border-divider p-4 text-sm text-foreground/60">© 2026</footer>
</ScarnaLayout>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `MinHeight` | `string?` | `"100vh"` | CSS `min-height` корневого элемента. Пусто/`null` — без `min-height` (например, вложенный layout внутри модалки) |
| `Flex` | `bool` | `true` | Рендерить как `flex flex-col` вместо обычного block-`div` — даёт "бесплатный" sticky footer и не ломает текущую вёрстку NavBar/Drawer |
| `ChildContent` | `RenderFragment?` | — | Содержимое: `ScarnaNavBar`, `ScarnaDrawer`, `main`, футер |
| `Class`, `Style`, `UserAttributes` | — | — | см. [getting-started.md](getting-started.md) |

Цвет фона (`bg-background text-foreground`) не параметризован — это базовая пара
токенов темы для "холста" страницы. Если конкретной странице нужен другой фон —
переопределяйте через `Class` (например, `Class="bg-content2"`).
