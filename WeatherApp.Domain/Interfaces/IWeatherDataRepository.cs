using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Models;

namespace WeatherApp.Domain.Interfaces;

public interface IWeatherDataRepository
{
    Task<HashSet<(string StationName, DateTime Timestamp)>> GetExistingKeysAsync(
        IReadOnlyCollection<WeatherDataPoint> candidates,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<WeatherDataPoint> points, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetStationNamesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WeatherDataPoint>> GetByStationAsync(
        string stationName,
        CancellationToken cancellationToken = default);

    Task<StationStatistics?> GetStatisticsAsync(string stationName, CancellationToken cancellationToken = default);
}
