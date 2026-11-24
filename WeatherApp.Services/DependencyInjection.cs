using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Infrastructure;
using WeatherApp.Services.API;
using WeatherApp.Services.CSV;
using WeatherApp.Services.Interfaces;
using WeatherApp.Services.Logging;
using WeatherApp.Services.Statistics;

namespace WeatherApp.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddScoped<IStatisticsService, StatisticsService>();
        services.AddScoped<ICSVService, CSVService>();
        services.AddScoped<IOpenMeteoService, OpenMeteoService>();
        services.AddSingleton<IBadDataLogger, BadDataLogger>();

        return services;
    }
}
