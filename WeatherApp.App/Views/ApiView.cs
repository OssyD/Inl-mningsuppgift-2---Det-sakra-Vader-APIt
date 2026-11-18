using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using WeatherApp.App.Helpers;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.App.Views
{
    public class ApiView(IOpenMeteoService service)
    {
        public async Task ShowAsync()
        {
            var result = await service.GetWeatherFromOpenMeteo();

            if (!result.IsSuccess)
            {
                ConsoleHelper.ShowError(result.Error!.Message);
                return;
            }

            AnsiConsole.MarkupLine($"[green]{result.Value} new measurements saved.[/]");
        }
    }
}
