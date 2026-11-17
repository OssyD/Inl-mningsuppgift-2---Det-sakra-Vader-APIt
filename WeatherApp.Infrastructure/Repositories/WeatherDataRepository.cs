using Microsoft.EntityFrameworkCore;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Models;
using WeatherApp.Infrastructure.Data;

namespace WeatherApp.Infrastructure.Repositories;

public class WeatherDataRepository(WeatherDbContext context) : IWeatherDataRepository
{
    public async Task<HashSet<(string StationName, DateTime Timestamp)>> GetExistingKeysAsync(
        IReadOnlyCollection<WeatherDataPoint> candidates,
        CancellationToken cancellationToken = default)
    {
        if (candidates.Count == 0)
            return [];

        var stationNames = candidates.Select(p => p.StationName).Distinct().ToList();
        var from = candidates.Min(p => p.Timestamp);
        var to = candidates.Max(p => p.Timestamp);

        var existing = await context.WeatherDataPoints
            .AsNoTracking()
            .Where(p => stationNames.Contains(p.StationName) && p.Timestamp >= from && p.Timestamp <= to)
            .Select(p => new { p.StationName, p.Timestamp })
            .ToListAsync(cancellationToken);

        return existing.Select(p => (p.StationName, p.Timestamp)).ToHashSet();
    }

    public async Task AddRangeAsync(IEnumerable<WeatherDataPoint> points, CancellationToken cancellationToken = default)
    {
        await context.WeatherDataPoints.AddRangeAsync(points, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetStationNamesAsync(CancellationToken cancellationToken = default) =>
        await context.WeatherDataPoints
            .AsNoTracking()
            .Select(p => p.StationName)
            .Distinct()
            .OrderBy(name => name)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<WeatherDataPoint>> GetByStationAsync(
        string stationName,
        CancellationToken cancellationToken = default) =>
        await context.WeatherDataPoints
            .AsNoTracking()
            .Where(p => p.StationName == stationName)
            .OrderBy(p => p.Timestamp)
            .ToListAsync(cancellationToken);

    public async Task<StationStatistics?> GetStatisticsAsync(
        string stationName,
        CancellationToken cancellationToken = default) =>
        await context.WeatherDataPoints
            .AsNoTracking()
            .Where(p => p.StationName == stationName)
            .GroupBy(p => p.StationName)
            .Select(g => new StationStatistics(
                g.Key,
                g.Count(),
                g.Max(p => p.Temperature),
                g.Min(p => p.Temperature),
                g.Average(p => p.Temperature),
                g.Min(p => p.Timestamp),
                g.Max(p => p.Timestamp)))
            .FirstOrDefaultAsync(cancellationToken);
}
