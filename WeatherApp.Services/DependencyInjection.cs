using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Infrastructure;
using WeatherApp.Services.CSV;
using WeatherApp.Services.Interfaces;
using WeatherApp.Services.Logging;

namespace WeatherApp.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddInfrastructure();
        services.AddScoped<IStatisticsService, StatisticsService>();
        services.AddScoped<ICSVService, CSVService>();
        services.AddSingleton<IBadDataLogger, BadDataLogger>();

        return services;
    }
}
