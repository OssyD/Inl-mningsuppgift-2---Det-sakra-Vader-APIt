namespace WeatherApp.Services.Interfaces;

public interface IBadDataLogger
{
    Task WriteAsync(IEnumerable<string> lines);
}
