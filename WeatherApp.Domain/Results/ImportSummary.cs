namespace WeatherApp.Domain.Results;

public sealed record ImportSummary(
    int Imported,
    int Duplicates,
    IReadOnlyList<RejectedRecord> Rejected);
