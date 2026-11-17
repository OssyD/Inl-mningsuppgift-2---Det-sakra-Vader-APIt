using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Infrastructure.Data;
using WeatherApp.Infrastructure.Repositories;

namespace WeatherApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<WeatherDbContext>();
        services.AddScoped<IWeatherDataRepository, WeatherDataRepository>();

        return services;
    }
}
