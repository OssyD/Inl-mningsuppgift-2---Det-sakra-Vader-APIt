using Microsoft.Extensions.Configuration;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services.Logging;

public class BadDataLogger(IConfiguration configuration) : IBadDataLogger
{
    private readonly string badDataPath = Path.Combine(AppContext.BaseDirectory, configuration["FileImport:BadDataLogPath"]!);
    private readonly string errorPath = Path.Combine(AppContext.BaseDirectory, configuration["ErrorLog:Path"]!);

    public async Task BadLogAsync(IEnumerable<string> lines)
    {
        try
        {
            await File.WriteAllLinesAsync(badDataPath, lines);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
        }
    }

    public async Task ErrorLogAsync(string message)
    {
        try
        {
            await File.AppendAllTextAsync(errorPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  {message}{Environment.NewLine}");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
        }
    }
}
