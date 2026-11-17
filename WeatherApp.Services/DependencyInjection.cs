using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Infrastructure;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddInfrastructure();
        services.AddScoped<IStatisticsService, StatisticsService>();

        return services;
    }
}
