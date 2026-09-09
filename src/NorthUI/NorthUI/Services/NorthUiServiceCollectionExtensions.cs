using Microsoft.Extensions.DependencyInjection;

namespace NorthUI.Services;

/// <summary>
/// Точка входа для регистрации сервисов NorthUI в DI-контейнере хост-приложения.
/// Вызывается один раз в Program.cs: <c>builder.Services.AddNorthUI();</c>
/// Как и в MudBlazor, дальше сервисы просто инжектируются: <c>@inject INorthDialogService DialogService</c>.
/// </summary>
public static class NorthUiServiceCollectionExtensions
{
    public static IServiceCollection AddNorthUI(this IServiceCollection services)
    {
        // Регистрируем конкретный тип как Scoped и переиспользуем тот же экземпляр
        // для публичного интерфейса — так провайдеры (в этой же сборке) получают
        // доступ к внутреннему состоянию (Dialogs/Toasts + OnChange), а код приложения
        // работает только с публичным контрактом.
        services.AddScoped<NorthDialogService>();
        services.AddScoped<INorthDialogService>(sp => sp.GetRequiredService<NorthDialogService>());

        services.AddScoped<ToastManager>();
        services.AddScoped<IToastManager>(sp => sp.GetRequiredService<ToastManager>());

        services.AddScoped<Navigator>();
        services.AddScoped<INavigator>(sp => sp.GetRequiredService<Navigator>());

        return services;
    }
}
