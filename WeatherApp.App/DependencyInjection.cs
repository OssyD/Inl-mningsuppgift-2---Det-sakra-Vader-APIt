using Microsoft.Extensions.DependencyInjection;
using WeatherApp.App.Views;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.App;

public static class DependencyInjection
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddTransient<StatisticsView>();
        services.AddTransient<CsvView>();
        services.AddTransient<ApiView>();
        services.AddTransient<WeatherMonitorApp>();

        return services;
    }
}
