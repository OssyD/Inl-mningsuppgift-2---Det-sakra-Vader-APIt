namespace WeatherApp.Services.Interfaces;

public interface IBadDataLogger
{
    Task BadLogAsync(IEnumerable<string> lines);

    Task ErrorLogAsync(string message);
}
