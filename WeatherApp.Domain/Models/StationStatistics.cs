namespace WeatherApp.Domain.Models;

public sealed record StationStatistics(
    string StationName,
    int MeasurementCount,
    decimal MaxTemperature,
    decimal MinTemperature,
    decimal AverageTemperature,
    DateTime FirstMeasurement,
    DateTime LastMeasurement);
