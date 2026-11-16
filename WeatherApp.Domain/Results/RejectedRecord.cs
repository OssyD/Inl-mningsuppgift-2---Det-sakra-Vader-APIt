namespace WeatherApp.Domain.Results;

public sealed record RejectedRecord(int LineNumber, string RawData, string Reason);
