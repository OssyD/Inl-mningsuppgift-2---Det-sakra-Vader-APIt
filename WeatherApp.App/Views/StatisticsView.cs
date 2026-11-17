using Spectre.Console;
using WeatherApp.App.Helpers;
using WeatherApp.App.Menus;
using WeatherApp.Domain.Interfaces;

namespace WeatherApp.App.Views;

public class StatisticsView(IWeatherDataRepository repository)
{
    public async Task ShowAsync()
    {
        var stations = await repository.GetStationNamesAsync();
        if (stations.Count == 0)
        {
            ConsoleHelper.ShowWarning("No measurements stored yet. Import data first.");
            return;
        }

        ShowStations(stations);

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[bold]Choose a station[/]")
                .AddChoices(stations)
                .AddChoices(MenuOptions.Back));

        if (selected == MenuOptions.Back)
            return;

        AnsiConsole.Clear();
        await ShowMeasurementsAsync(selected);
        await ShowStationStatisticsAsync(selected);
    }

    private static void ShowStations(IReadOnlyList<string> stations)
    {
        var table = new Table().RoundedBorder().Title("[bold]Stations[/]");
        table.AddColumn("Station");

        foreach (var station in stations)
        {
            table.AddRow(Markup.Escape(station));
        }

        AnsiConsole.Write(table);
    }

    private async Task ShowMeasurementsAsync(string stationName)
    {
        var measurements = await repository.GetByStationAsync(stationName);

        var table = new Table().RoundedBorder().Title($"[bold]Measurements for {Markup.Escape(stationName)}[/]");
        table.AddColumn("Timestamp");
        table.AddColumn(new TableColumn("Temperature (°C)").RightAligned());
        table.AddColumn(new TableColumn("Humidity (%)").RightAligned());
        table.AddColumn("Source");

        foreach (var point in measurements)
        {
            table.AddRow(
                point.Timestamp.ToString("yyyy-MM-dd HH:mm"),
                $"[{ColorHelper.TemperatureColor(point.Temperature)}]{point.Temperature:0.0}[/]",
                $"{point.Humidity:0.0}",
                Markup.Escape(point.DataSource));
        }

        AnsiConsole.Write(table);
    }

    private async Task ShowStationStatisticsAsync(string stationName)
    {
        var statistics = await repository.GetStatisticsAsync(stationName);
        if (statistics is null)
            return;

        var table = new Table().RoundedBorder().Title("[bold]Statistics[/]");
        table.AddColumn("Measurements");
        table.AddColumn(new TableColumn("Max (°C)").RightAligned());
        table.AddColumn(new TableColumn("Min (°C)").RightAligned());
        table.AddColumn(new TableColumn("Average (°C)").RightAligned());

        table.AddRow(
            statistics.MeasurementCount.ToString(),
            $"[red]{statistics.MaxTemperature:0.0}[/]",
            $"[blue]{statistics.MinTemperature:0.0}[/]",
            $"{statistics.AverageTemperature:0.0}");

        AnsiConsole.Write(table);
    }
}
