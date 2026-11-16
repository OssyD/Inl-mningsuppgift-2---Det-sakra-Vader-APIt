using WeatherApp.Domain.Entities;

namespace WeatherApp.Domain.Results;

public sealed record SourceReadResult(
    IReadOnlyList<WeatherDataPoint> ValidPoints,
    IReadOnlyList<RejectedRecord> Rejected);
