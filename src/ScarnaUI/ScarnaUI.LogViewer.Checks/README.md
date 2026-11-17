# Проверки LogViewer

Запуск из корня репозитория:

```powershell
dotnet run --project src/ScarnaUI/ScarnaUI.LogViewer.Checks
node src/ScarnaUI/ScarnaUI.LogViewer.Checks/selection.test.mjs
node src/ScarnaUI/ScarnaUI.LogViewer.Checks/virtualization.test.mjs
```

Первая команда проверяет дисковый снимок и сравнивает многострочные регулярные
выражения с обычным `Regex` .NET. Вторая проверяет логику выделения и событий
копирования с имитацией браузера; это не замена проверке реального буфера обмена.

Необязательный локальный замер создаёт временный синтетический журнал 200 МиБ и
UTF-16 копию, затем удаляет их:

```powershell
dotnet run --project src/ScarnaUI/ScarnaUI.LogViewer.Checks -- --benchmark
```

Время и пиковая память зависят от машины и состояния файлового кеша. Замер не
включает работу браузера или Blazor Server.
