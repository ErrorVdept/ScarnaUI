namespace ScarnaUI.Demo.Components.Pages.ComponentLib.Shared;

/// <summary>Одна строка таблицы API в интерактивной документации (см. DocApiTable).</summary>
public sealed record DocApiRow(string Name, string Type, string Default, string Description);
