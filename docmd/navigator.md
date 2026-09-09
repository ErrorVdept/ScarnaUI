# Navigator (INavigator)

## Файлы
- `src/NorthUI/NorthUI/Services/INavigator.cs`
- `src/NorthUI/NorthUI/Services/Navigator.cs`

## Описание

Императивный сервис навигации — обёртка над встроенным `NavigationManager`, по духу
как в MudBlazor: компоненты навигации (`NavLink`, `NavMenu`) не работают с голым
`NavigationManager` напрямую, а используют готовые хелперы поверх него. `Navigator`
даёт то же самое для NorthUI — в первую очередь для подсветки активного пункта в
[NorthBottomMenu](bottom-menu.md)/[NorthNavBar](navbar.md) и будущем `NorthNavMenu`,
без необходимости писать сравнение текущего URI руками в каждом таком компоненте.

Регистрируется автоматически вместе с остальными сервисами NorthUI —
`builder.Services.AddNorthUI()` (см. [getting-started.md](getting-started.md)).
Инжектируется как обычный Scoped-сервис.

## Пример использования

```razor
@inject INavigator Navigator

<NorthButton OnClick='() => Navigator.NavigateTo("/orders/5")'>К заказу</NorthButton>

<NorthBottomMenuItem Text="Заказы" OnClick='() => Navigator.NavigateTo("/orders")'>
    <span class="@(Navigator.IsActive("/orders") ? "text-primary" : "")">...</span>
</NorthBottomMenuItem>
```

```csharp
@code {
    protected override void OnInitialized()
    {
        Navigator.LocationChanged += (_, e) => Console.WriteLine(e.Location);
    }
}
```

## Члены INavigator

| Член | Назначение |
|---|---|
| `Uri` | Текущий абсолютный URI |
| `BaseUri` | Базовый URI приложения |
| `LocationChanged` | Событие смены адреса (в т.ч. кнопки браузера "назад"/"вперёд") |
| `NavigateTo(uri, forceLoad, replace)` | Переход на другой адрес |
| `Refresh(replace)` | Полная перезагрузка текущей страницы (аналог F5, всегда `forceLoad`) |
| `ToBaseRelativePath(uri?)` | Адрес относительно `BaseUri`, без домена и ведущего "/" |
| `IsActive(uri, exact)` | True, если `uri` совпадает с текущим адресом — для подсветки активного пункта меню. `exact=false` (по умолчанию) считает активным и любой дочерний путь |
