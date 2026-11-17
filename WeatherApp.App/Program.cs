using Microsoft.Extensions.DependencyInjection;
using WeatherApp.App;
using WeatherApp.Services;

var services = new ServiceCollection()
    .AddServices()
    .AddViews();

await using var provider = services.BuildServiceProvider(new ServiceProviderOptions
{
    ValidateOnBuild = true,
    ValidateScopes = true
});
await using var scope = provider.CreateAsyncScope();

var app = scope.ServiceProvider.GetRequiredService<WeatherMonitorApp>();
await app.RunAsync();
