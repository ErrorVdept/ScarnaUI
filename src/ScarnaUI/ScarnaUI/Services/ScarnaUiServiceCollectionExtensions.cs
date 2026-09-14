using Microsoft.Extensions.DependencyInjection;

namespace ScarnaUI.Services;

/// <summary>
/// Точка входа для регистрации сервисов ScarnaUI в DI-контейнере хост-приложения.
/// Вызывается один раз в Program.cs: <c>builder.Services.AddScarnaUI();</c>
/// Как и в MudBlazor, дальше сервисы просто инжектируются: <c>@inject IScarnaDialogService DialogService</c>.
/// </summary>
public static class ScarnaUiServiceCollectionExtensions
{
    public static IServiceCollection AddScarnaUI(this IServiceCollection services)
    {
        // Регистрируем конкретный тип как Scoped и переиспользуем тот же экземпляр
        // для публичного интерфейса — так провайдеры (в этой же сборке) получают
        // доступ к внутреннему состоянию (Dialogs/Toasts + OnChange), а код приложения
        // работает только с публичным контрактом.
        services.AddScoped<ScarnaDialogService>();
        services.AddScoped<IScarnaDialogService>(sp => sp.GetRequiredService<ScarnaDialogService>());

        services.AddScoped<ToastManager>();
        services.AddScoped<IToastManager>(sp => sp.GetRequiredService<ToastManager>());

        services.AddScoped<Navigator>();
        services.AddScoped<INavigator>(sp => sp.GetRequiredService<Navigator>());

        return services;
    }
}
