using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherApp.App;
using WeatherApp.Services;

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
    // Run against LocalDB unless DOTNET_ENVIRONMENT says otherwise (e.g. "Production" for Azure).
    EnvironmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? Environments.Development
});

builder.Services
    .AddServices(builder.Configuration)
    .AddViews();

using var host = builder.Build();
await using var scope = host.Services.CreateAsyncScope();

await scope.ServiceProvider.GetRequiredService<WeatherMonitorApp>().RunAsync();
