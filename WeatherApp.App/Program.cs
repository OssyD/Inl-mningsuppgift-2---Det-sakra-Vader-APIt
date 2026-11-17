using WeatherApp.Infrastructure.Data;
using WeatherApp.Infrastructure.Repositories;

await using var context = new WeatherDbContext();
var repository = new WeatherDataRepository(context);
