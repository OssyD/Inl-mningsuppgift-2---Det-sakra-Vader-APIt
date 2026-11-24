using Spectre.Console;
using WeatherApp.App.Helpers;
using WeatherApp.Domain.Entities;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.App.Views;

public class ApiView(IOpenMeteoService service)
{
    private const int UpcomingHours = 12;

    public async Task ShowAsync()
    {
        var result = await AnsiConsole.Status()
            .StartAsync("Fetching live data from Open-Meteo...", _ => service.GetWeatherFromOpenMeteo());

        if (!result.IsSuccess)
        {
            ConsoleHelper.ShowError(result.Error!.Message);
            return;
        }

        var (summary, points) = result.Value!;
        if (points.Count == 0)
        {
            ConsoleHelper.ShowWarning("Open-Meteo returned no measurements.");
            return;
        }

        var ordered = points.OrderBy(p => p.Timestamp).ToList();
        var currentHour = DateTime.Now.Date.AddHours(DateTime.Now.Hour);
        var current = ordered.LastOrDefault(p => p.Timestamp <= currentHour) ?? ordered[0];

        ShowCurrent(current);
        ShowUpcoming(ordered.Where(p => p.Timestamp > current.Timestamp).Take(UpcomingHours).ToList());

        AnsiConsole.MarkupLine(
            $"\n[green]{summary.Imported} rows saved to the database[/], " +
            $"[grey]{points.Count - summary.Imported} already stored[/], " +
            $"[red]{summary.Rejected} rejected due to errors[/].");
    }

    private static void ShowCurrent(WeatherDataPoint point)
    {
        var table = new Table().RoundedBorder().BorderColor(Color.DeepSkyBlue1).Title("[bold]Current weather[/]");
        table.AddColumn("Station");
        table.AddColumn("Timestamp");
        table.AddColumn(new TableColumn("Temperature (°C)").RightAligned());
        table.AddColumn(new TableColumn("Humidity (%)").RightAligned());
        table.AddColumn("Source");

        table.AddRow(
            Markup.Escape(point.StationName),
            point.Timestamp.ToString("yyyy-MM-dd HH:mm"),
            $"[{ColorHelper.TemperatureColor(point.Temperature)}]{point.Temperature:0.0}[/]",
            $"{point.Humidity:0}",
            Markup.Escape(point.DataSource));

        AnsiConsole.Write(table);
    }

    private static void ShowUpcoming(IReadOnlyList<WeatherDataPoint> points)
    {
        if (points.Count == 0)
            return;

        var table = new Table().RoundedBorder().Title($"[bold]Next {points.Count} hours[/]");
        table.AddColumn("Timestamp");
        table.AddColumn(new TableColumn("Temperature (°C)").RightAligned());
        table.AddColumn(new TableColumn("Humidity (%)").RightAligned());

        foreach (var point in points)
        {
            table.AddRow(
                point.Timestamp.ToString("yyyy-MM-dd HH:mm"),
                $"[{ColorHelper.TemperatureColor(point.Temperature)}]{point.Temperature:0.0}[/]",
                $"{point.Humidity:0}");
        }

        AnsiConsole.Write(table);
    }
}
