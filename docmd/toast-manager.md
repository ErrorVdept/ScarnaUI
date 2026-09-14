# Тосты: IToastManager, ScarnaSnackbarProvider

## Файлы
- `src/ScarnaUI/ScarnaUI/Services/IToastManager.cs`
- `src/ScarnaUI/ScarnaUI/Services/ToastManager.cs` — реализация (internal), чистый C#, без JS
- `src/ScarnaUI/ScarnaUI/Services/ScarnaToast.cs` — модель `ScarnaToast`, `ScarnaToastSeverity`
- `src/ScarnaUI/ScarnaUI/Components/Overlays/ScarnaSnackbarProvider.razor` — портал/очередь

## Принцип работы

Императивный сервис показа тостов (снэкбаров) — аналог `ISnackbar` в MudBlazor.
Реализация полностью на C#, без единой строчки JS: авто-скрытие тоста через заданное
время сделано на `Task.Delay` внутри `ToastManager.Add`.

1. Разместите `<ScarnaSnackbarProvider />` один раз в layout.
2. Заинжектьте `IToastManager` в любой компонент и вызовите `Add(...)`.
3. `ToastManager` держит очередь (`List<ScarnaToast>`) и поднимает событие `OnChange`
   при любом изменении; `ScarnaSnackbarProvider` подписан на это событие и перерисовывает
   стопку уведомлений в правом нижнем углу экрана.
4. Каждый добавленный тост сам себя удаляет из очереди через `Duration` (по умолчанию
   4 секунды) — если передать `TimeSpan.Zero`, тост останется, пока пользователь не
   закроет его крестиком вручную.

## Пример использования

```razor
@inject IToastManager ToastManager

<ScarnaButton OnClick="Save">Сохранить</ScarnaButton>

@code {
    private void Save()
    {
        // ... логика сохранения ...
        ToastManager.Add("Изменения сохранены", ScarnaToastSeverity.Success);
    }
}
```

Уведомление без авто-скрытия (пользователь закрывает сам):

```csharp
ToastManager.Add("Соединение потеряно, повторное подключение...",
    ScarnaToastSeverity.Error, duration: TimeSpan.Zero);
```

## IToastManager

| Метод/член | Назначение |
|---|---|
| `Toasts` | Текущая очередь показанных уведомлений |
| `OnChange` | Событие при добавлении/удалении любого тоста |
| `Add(message, severity, duration?)` | Добавить уведомление. `severity` по умолчанию `Info` |
| `Remove(id)` | Убрать уведомление раньше срока |

## ScarnaToastSeverity

`Info` (нейтральный, цвет `primary`) / `Success` / `Warning` / `Error` (цвет `danger`) —
определяют цвет левой полосы и иконки уведомления.
