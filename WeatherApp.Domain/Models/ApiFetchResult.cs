using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Models;

public sealed record ApiFetchResult(ImportSummary Summary, IReadOnlyList<WeatherDataPoint> Points);
