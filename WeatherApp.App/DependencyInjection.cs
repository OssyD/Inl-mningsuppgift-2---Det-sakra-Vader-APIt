using Microsoft.Extensions.DependencyInjection;
using WeatherApp.App.Views;

namespace WeatherApp.App;

public static class DependencyInjection
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddTransient<StatisticsView>();
        services.AddTransient<WeatherMonitorApp>();

        return services;
    }
}
