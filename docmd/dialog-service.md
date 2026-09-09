# Диалоги: INorthDialogService, NorthDialogProvider, NorthDialog

## Файлы
- `src/NorthUI/NorthUI/Services/INorthDialogService.cs`
- `src/NorthUI/NorthUI/Services/NorthDialogService.cs` — реализация (internal)
- `src/NorthUI/NorthUI/Services/NorthDialogModels.cs` — `NorthDialogOptions`, `NorthDialogResult`, `NorthDialogReference`, `NorthDialogSize`
- `src/NorthUI/NorthUI/Components/Overlays/NorthDialogProvider.razor` — портал
- `src/NorthUI/NorthUI/Components/Overlays/NorthDialogHost.razor` + `.razor.cs` — подложка + панель одного диалога (внутренний, напрямую не используется)
- `src/NorthUI/NorthUI/Components/Overlays/NorthDialog.razor` — "скелет" содержимого диалога (заголовок/тело/действия)
- `src/NorthUI/NorthUI/wwwroot/js/north/dialog.js` — focus trap, Escape, блокировка скролла страницы

## Принцип работы

Императивный сервис показа модальных диалогов — аналог `IDialogService` в MudBlazor.
Открывает модалку поверх всего приложения без ручного `bool _isOpen` в каждом месте
использования.

1. Разместите `<NorthDialogProvider />` один раз в layout (см.
   [getting-started.md](getting-started.md)).
2. Заинжектьте `INorthDialogService` в любой компонент и вызовите
   `ShowAsync<TComponent>(...)`, передав тип **своего** компонента с содержимым диалога.
3. `NorthDialogService` держит список открытых диалогов (`NorthDialogReference`) и
   уведомляет `NorthDialogProvider` о любом изменении; провайдер рендерит по одному
   `NorthDialogHost` на каждый открытый диалог — тот рисует подложку, панель нужного
   размера и монтирует внутрь ваш компонент через `<DynamicComponent>`, попутно
   включая JS-focus-trap (`dialog.js`) и блокировку скролла страницы.
4. Внутри своего компонента-содержимого получите `NorthDialogReference` через
   `[CascadingParameter]` и вызовите `.Close(result)`, когда пользователь подтвердил
   или отменил действие. `ShowAsync` вернёт `Task<NorthDialogResult>`, который
   завершается именно в этот момент — так вызывающий код может дождаться результата.

## Пример: свой диалог

```razor
@* Components/Dialogs/ConfirmDeleteDialog.razor *@
<NorthDialog Title="Удалить файл?">
    <DialogContent>Это действие необратимо.</DialogContent>
    <DialogActions>
        <NorthButton Variant="NorthVariant.Light" OnClick="Cancel">Отмена</NorthButton>
        <NorthButton Color="NorthColor.Danger" OnClick="Confirm">Удалить</NorthButton>
    </DialogActions>
</NorthDialog>

@code {
    [CascadingParameter] private NorthDialogReference DialogReference { get; set; } = default!;
    private void Cancel() => DialogReference.Close(NorthDialogResult.Cancel());
    private void Confirm() => DialogReference.Close(NorthDialogResult.Ok());
}
```

## Пример: вызов из кода

```razor
@inject INorthDialogService DialogService

<NorthButton Color="NorthColor.Danger" OnClick="DeleteAsync">Удалить</NorthButton>

@code {
    private async Task DeleteAsync()
    {
        var result = await DialogService.ShowAsync<ConfirmDeleteDialog>(
            new NorthDialogOptions { Size = NorthDialogSize.Small });

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

## INorthDialogService

| Метод | Назначение |
|---|---|
| `ShowAsync<TComponent>(NorthDialogOptions?)` | Показать диалог без параметров |
| `ShowAsync<TComponent>(IReadOnlyDictionary<string, object?>, NorthDialogOptions?)` | Показать диалог с параметрами компонента |
| `ShowAsync(Type, ...)` | Нетипизированная версия (тип компонента известен в рантайме) |
| `Close(NorthDialogReference, NorthDialogResult?)` | Принудительно закрыть диалог снаружи |

## NorthDialogOptions

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Title` | `string?` | — | Используется, только если ваш компонент не рисует собственный заголовок |
| `Size` | `NorthDialogSize` | `Medium` | `Small`/`Medium`/`Large`/`FullScreen` |
| `CloseOnEscapeKey` | `bool` | `true` | Закрывать по Escape (результат — `Cancel()`) |
| `CloseOnBackdropClick` | `bool` | `true` | Закрывать по клику на подложку |

## NorthDialogResult

`NorthDialogResult.Ok(data?)` / `NorthDialogResult.Cancel()`. `Cancelled` — булево
поле ("закрыт без подтверждения"), `Data`/`DataAs<T>()` — произвольные данные,
переданные из диалога.

## NorthDialog (готовый "скелет" содержимого)

| Параметр | Тип | Назначение |
|---|---|---|
| `Title` | `string?` | Текстовый заголовок |
| `TitleContent` | `RenderFragment?` | Заголовок сложной разметкой (вместо `Title`) |
| `ShowCloseButton` | `bool` (default `true`) | Крестик закрытия в заголовке |
| `DialogContent` | `RenderFragment?` | Тело диалога |
| `DialogActions` | `RenderFragment?` | Панель кнопок внизу |
