namespace WeatherApp.Domain.Results;

public enum ErrorType
{
    Validation,
    NotFound,
    FileAccess,
    Network,
    Database,
    Unexpected
}
