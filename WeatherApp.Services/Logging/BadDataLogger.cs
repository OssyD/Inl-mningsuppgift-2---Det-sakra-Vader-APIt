using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services.Logging;

public class BadDataLogger : IBadDataLogger
{
    private readonly string path = Path.Combine(AppContext.BaseDirectory, "bad_data.log");

    public async Task WriteAsync(IEnumerable<string> lines)
    {
        try
        {
            await File.WriteAllLinesAsync(path, lines);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
        }
    }
}
