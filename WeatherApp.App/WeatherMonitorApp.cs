using CsvHelper;
using Spectre.Console;
using WeatherApp.App.Helpers;
using WeatherApp.App.Menus;
using WeatherApp.App.Views;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.App;

public class WeatherMonitorApp(StatisticsView statisticsView, CsvView csvView, ApiView apiView)
{
    public async Task RunAsync()
    {
        var running = true;
        while (running)
        {
            AnsiConsole.Clear();
            ConsoleHelper.ShowHeader();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[bold]Choose an option[/]")
                    .AddChoices(
                        MenuOptions.ImportFromFile,
                        MenuOptions.FetchFromApi,
                        MenuOptions.ShowStatistics,
                        MenuOptions.Exit));

            AnsiConsole.Clear();
            try
            {
                switch (choice)
                {
                    case MenuOptions.ImportFromFile: await csvView.ShowAsync(); break;
                    case MenuOptions.FetchFromApi: await apiView.ShowAsync(); break;
                    case MenuOptions.ShowStatistics: await statisticsView.ShowAsync(); break;
                    case MenuOptions.Exit: running = false; break;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.ShowError($"Something went wrong: {ex.Message}");
            }

            if (running)
                ConsoleHelper.WaitForKey();
        }
    }
}
