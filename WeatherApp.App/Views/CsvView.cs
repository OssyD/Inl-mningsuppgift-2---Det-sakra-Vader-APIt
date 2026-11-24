using Spectre.Console;
using WeatherApp.App.Helpers;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.App.Views;

public class CsvView(ICSVService service)
{
    public async Task ShowAsync()
    {
        var result = await service.ImportCSVAsync();

        if (!result.IsSuccess)
        {
            ConsoleHelper.ShowError(result.Error!.Message);
            return;
        }

        var summary = result.Value!;
        AnsiConsole.MarkupLine($"[green]{summary.Imported} rows imported, {summary.Rejected} rows rejected due to errors.[/]");
    }
}
