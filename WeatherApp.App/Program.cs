using Microsoft.Extensions.DependencyInjection;
using WeatherApp.App;
using WeatherApp.Services;
using WeatherApp.Services.Interfaces;

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
var csv = scope.ServiceProvider.GetRequiredService<ICSVService>();

var results = await csv.ReadCSVAsync();

foreach (var result in results.Value!)
{
    Console.WriteLine($"Station: {result.StationName}, Timestamp: {result.Timestamp}, Temperature: {result.Temperature}, Humidity: {result.Humidity}, DataSource: {result.DataSource}");
}

//await app.RunAsync();
