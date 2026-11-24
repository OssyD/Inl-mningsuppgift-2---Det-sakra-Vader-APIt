using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Infrastructure.Data;
using WeatherApp.Infrastructure.Repositories;

namespace WeatherApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WeatherDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("WeatherDb"),
                sql => sql.EnableRetryOnFailure()));
        services.AddScoped<IWeatherDataRepository, WeatherDataRepository>();

        return services;
    }
}
