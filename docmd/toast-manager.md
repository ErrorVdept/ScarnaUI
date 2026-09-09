# Тосты: IToastManager, NorthSnackbarProvider

## Файлы
- `src/NorthUI/NorthUI/Services/IToastManager.cs`
- `src/NorthUI/NorthUI/Services/ToastManager.cs` — реализация (internal), чистый C#, без JS
- `src/NorthUI/NorthUI/Services/NorthToast.cs` — модель `NorthToast`, `NorthToastSeverity`
- `src/NorthUI/NorthUI/Components/Overlays/NorthSnackbarProvider.razor` — портал/очередь

## Принцип работы

Императивный сервис показа тостов (снэкбаров) — аналог `ISnackbar` в MudBlazor.
Реализация полностью на C#, без единой строчки JS: авто-скрытие тоста через заданное
время сделано на `Task.Delay` внутри `ToastManager.Add`.

1. Разместите `<NorthSnackbarProvider />` один раз в layout.
2. Заинжектьте `IToastManager` в любой компонент и вызовите `Add(...)`.
3. `ToastManager` держит очередь (`List<NorthToast>`) и поднимает событие `OnChange`
   при любом изменении; `NorthSnackbarProvider` подписан на это событие и перерисовывает
   стопку уведомлений в правом нижнем углу экрана.
4. Каждый добавленный тост сам себя удаляет из очереди через `Duration` (по умолчанию
   4 секунды) — если передать `TimeSpan.Zero`, тост останется, пока пользователь не
   закроет его крестиком вручную.

## Пример использования

```razor
@inject IToastManager ToastManager

<NorthButton OnClick="Save">Сохранить</NorthButton>

@code {
    private void Save()
    {
        // ... логика сохранения ...
        ToastManager.Add("Изменения сохранены", NorthToastSeverity.Success);
    }
}
```

Уведомление без авто-скрытия (пользователь закрывает сам):

```csharp
ToastManager.Add("Соединение потеряно, повторное подключение...",
    NorthToastSeverity.Error, duration: TimeSpan.Zero);
```

## IToastManager

| Метод/член | Назначение |
|---|---|
| `Toasts` | Текущая очередь показанных уведомлений |
| `OnChange` | Событие при добавлении/удалении любого тоста |
| `Add(message, severity, duration?)` | Добавить уведомление. `severity` по умолчанию `Info` |
| `Remove(id)` | Убрать уведомление раньше срока |

## NorthToastSeverity

`Info` (нейтральный, цвет `primary`) / `Success` / `Warning` / `Error` (цвет `danger`) —
определяют цвет левой полосы и иконки уведомления.
