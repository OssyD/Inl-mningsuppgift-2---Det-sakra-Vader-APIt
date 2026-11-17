using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Models;

namespace WeatherApp.Domain.Interfaces;

public interface IWeatherDataRepository
{
    Task<int> AddNewAsync(IReadOnlyCollection<WeatherDataPoint> points);

    Task<IReadOnlyList<string>> GetStationNamesAsync();

    Task<IReadOnlyList<WeatherDataPoint>> GetByStationAsync(string stationName);

    Task<StationStatistics?> GetStatisticsAsync(string stationName);
}
