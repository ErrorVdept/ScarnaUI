# Диалоги: IScarnaDialogService, ScarnaDialogProvider, ScarnaDialog

## Файлы
- `src/ScarnaUI/ScarnaUI/Services/IScarnaDialogService.cs`
- `src/ScarnaUI/ScarnaUI/Services/ScarnaDialogService.cs` — реализация (internal)
- `src/ScarnaUI/ScarnaUI/Services/ScarnaDialogModels.cs` — `ScarnaDialogOptions`, `ScarnaDialogResult`, `ScarnaDialogReference`, `ScarnaDialogSize`
- `src/ScarnaUI/ScarnaUI/Components/Overlays/ScarnaDialogProvider.razor` — портал
- `src/ScarnaUI/ScarnaUI/Components/Overlays/ScarnaDialogHost.razor` + `.razor.cs` — подложка + панель одного диалога (внутренний, напрямую не используется)
- `src/ScarnaUI/ScarnaUI/Components/Overlays/ScarnaDialog.razor` — "скелет" содержимого диалога (заголовок/тело/действия)
- `src/ScarnaUI/ScarnaUI/wwwroot/js/scarna/dialog.js` — focus trap, Escape, блокировка скролла страницы

## Принцип работы

Императивный сервис показа модальных диалогов — аналог `IDialogService` в MudBlazor.
Открывает модалку поверх всего приложения без ручного `bool _isOpen` в каждом месте
использования.

1. Разместите `<ScarnaDialogProvider />` один раз в layout (см.
   [getting-started.md](getting-started.md)).
2. Заинжектьте `IScarnaDialogService` в любой компонент и вызовите
   `ShowAsync<TComponent>(...)`, передав тип **своего** компонента с содержимым диалога.
3. `ScarnaDialogService` держит список открытых диалогов (`ScarnaDialogReference`) и
   уведомляет `ScarnaDialogProvider` о любом изменении; провайдер рендерит по одному
   `ScarnaDialogHost` на каждый открытый диалог — тот рисует подложку, панель нужного
   размера и монтирует внутрь ваш компонент через `<DynamicComponent>`, попутно
   включая JS-focus-trap (`dialog.js`) и блокировку скролла страницы.
4. Внутри своего компонента-содержимого получите `ScarnaDialogReference` через
   `[CascadingParameter]` и вызовите `.Close(result)`, когда пользователь подтвердил
   или отменил действие. `ShowAsync` вернёт `Task<ScarnaDialogResult>`, который
   завершается именно в этот момент — так вызывающий код может дождаться результата.

## Пример: свой диалог

```razor
@* Components/Dialogs/ConfirmDeleteDialog.razor *@
<ScarnaDialog Title="Удалить файл?">
    <DialogContent>Это действие необратимо.</DialogContent>
    <DialogActions>
        <ScarnaButton Variant="Variant.Light" OnClick="Cancel">Отмена</ScarnaButton>
        <ScarnaButton Color="Color.Danger" OnClick="Confirm">Удалить</ScarnaButton>
    </DialogActions>
</ScarnaDialog>

@code {
    [CascadingParameter] private ScarnaDialogReference DialogReference { get; set; } = default!;
    private void Cancel() => DialogReference.Close(ScarnaDialogResult.Cancel());
    private void Confirm() => DialogReference.Close(ScarnaDialogResult.Ok());
}
```

## Пример: вызов из кода

```razor
@inject IScarnaDialogService DialogService

<ScarnaButton Color="Color.Danger" OnClick="DeleteAsync">Удалить</ScarnaButton>

@code {
    private async Task DeleteAsync()
    {
        var result = await DialogService.ShowAsync<ConfirmDeleteDialog>(
            new ScarnaDialogOptions { Size = ScarnaDialogSize.Small });

        if (!result.Cancelled)
        {
            // пользователь подтвердил
        }
    }
}
```

Передача параметров в диалог (имя параметра компонента → значение):

```csharp
await DialogService.ShowAsync<EditUserDialog>(
    new Dictionary<string, object?> { ["UserId"] = user.Id });
```

## IScarnaDialogService

| Метод | Назначение |
|---|---|
| `ShowAsync<TComponent>(ScarnaDialogOptions?)` | Показать диалог без параметров |
| `ShowAsync<TComponent>(IReadOnlyDictionary<string, object?>, ScarnaDialogOptions?)` | Показать диалог с параметрами компонента |
| `ShowAsync(Type, ...)` | Нетипизированная версия (тип компонента известен в рантайме) |
| `Close(ScarnaDialogReference, ScarnaDialogResult?)` | Принудительно закрыть диалог снаружи |

## ScarnaDialogOptions

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Title` | `string?` | — | Используется, только если ваш компонент не рисует собственный заголовок |
| `Size` | `ScarnaDialogSize` | `Medium` | `Small`/`Medium`/`Large`/`FullScreen` |
| `CloseOnEscapeKey` | `bool` | `true` | Закрывать по Escape (результат — `Cancel()`) |
| `CloseOnBackdropClick` | `bool` | `true` | Закрывать по клику на подложку |

## ScarnaDialogResult

`ScarnaDialogResult.Ok(data?)` / `ScarnaDialogResult.Cancel()`. `Cancelled` — булево
поле ("закрыт без подтверждения"), `Data`/`DataAs<T>()` — произвольные данные,
переданные из диалога.

## ScarnaDialog (готовый "скелет" содержимого)

| Параметр | Тип | Назначение |
|---|---|---|
| `Title` | `string?` | Текстовый заголовок |
| `TitleContent` | `RenderFragment?` | Заголовок сложной разметкой (вместо `Title`) |
| `ShowCloseButton` | `bool` (default `true`) | Крестик закрытия в заголовке |
| `DialogContent` | `RenderFragment?` | Тело диалога |
| `DialogActions` | `RenderFragment?` | Панель кнопок внизу |
