using WeatherApp.App;
using WeatherApp.App.Views;
using WeatherApp.Infrastructure.Data;
using WeatherApp.Infrastructure.Repositories;

await using var context = new WeatherDbContext();
var repository = new WeatherDataRepository(context);

var app = new WeatherMonitorApp(new StatisticsView(repository));
await app.RunAsync();
