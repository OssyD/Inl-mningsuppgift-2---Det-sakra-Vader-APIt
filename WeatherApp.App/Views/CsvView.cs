using System;
using System.Collections.Generic;
using System.Text;
using WeatherApp.App.Helpers;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.App.Views
{
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

            Console.WriteLine($"CSV data imported successfully. Rows: {result.Value}");
        }
    }
}
