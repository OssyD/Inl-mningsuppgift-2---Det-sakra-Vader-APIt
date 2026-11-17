using Microsoft.EntityFrameworkCore;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Models;
using WeatherApp.Infrastructure.Data;

namespace WeatherApp.Infrastructure.Repositories;

public class WeatherDataRepository(WeatherDbContext context) : IWeatherDataRepository
{
    public async Task<int> AddNewAsync(IReadOnlyCollection<WeatherDataPoint> points)
    {
        var existing = await GetExistingKeysAsync(points);

        var newPoints = points
            .Where(p => !existing.Contains((p.StationName, p.Timestamp)))
            .DistinctBy(p => (p.StationName, p.Timestamp))
            .ToList();

        context.WeatherDataPoints.AddRange(newPoints);
        await context.SaveChangesAsync();

        return newPoints.Count;
    }

    public async Task<IReadOnlyList<string>> GetStationNamesAsync() =>
        await context.WeatherDataPoints
            .AsNoTracking()
            .Select(p => p.StationName)
            .Distinct()
            .OrderBy(name => name)
            .ToListAsync();

    public async Task<IReadOnlyList<WeatherDataPoint>> GetByStationAsync(string stationName) =>
        await context.WeatherDataPoints
            .AsNoTracking()
            .Where(p => p.StationName == stationName)
            .OrderBy(p => p.Timestamp)
            .ToListAsync();

    public async Task<StationStatistics?> GetStatisticsAsync(string stationName) =>
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
            .FirstOrDefaultAsync();

    private async Task<HashSet<(string StationName, DateTime Timestamp)>> GetExistingKeysAsync(IReadOnlyCollection<WeatherDataPoint> candidates)
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
            .ToListAsync();

        return existing.Select(p => (p.StationName, p.Timestamp)).ToHashSet();
    }
}
