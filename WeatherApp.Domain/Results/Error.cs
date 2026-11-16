namespace WeatherApp.Domain.Results;

public sealed record Error(ErrorType Type, string Message, Exception? Exception = null);
