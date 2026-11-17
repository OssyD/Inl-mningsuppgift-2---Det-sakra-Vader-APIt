using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Models;
using WeatherApp.Domain.Results;

namespace WeatherApp.Services.Interfaces;

public interface IStatisticsService
{
    Task<Result<IReadOnlyList<string>>> GetStationNamesAsync();

    Task<Result<IReadOnlyList<WeatherDataPoint>>> GetMeasurementsAsync(string stationName);

    Task<Result<StationStatistics?>> GetStatisticsAsync(string stationName);
}
