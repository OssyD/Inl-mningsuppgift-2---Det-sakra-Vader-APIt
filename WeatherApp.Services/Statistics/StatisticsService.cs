using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Models;
using WeatherApp.Domain.Results;
using WeatherApp.Services.Helpers;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services.Statistics;

public class StatisticsService(IWeatherDataRepository repository) : IStatisticsService
{
    public Task<Result<IReadOnlyList<string>>> GetStationNamesAsync() =>
        DatabaseCall.ExecuteAsync(() => repository.GetStationNamesAsync());

    public Task<Result<IReadOnlyList<WeatherDataPoint>>> GetMeasurementsAsync(string stationName) =>
        DatabaseCall.ExecuteAsync(() => repository.GetByStationAsync(stationName));

    public Task<Result<StationStatistics?>> GetStatisticsAsync(string stationName) =>
        DatabaseCall.ExecuteAsync(() => repository.GetStatisticsAsync(stationName));
}
