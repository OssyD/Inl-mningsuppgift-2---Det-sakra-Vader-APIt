using System;
using System.Collections.Generic;
using System.Text;
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

            foreach (var dataPoint in result.Value!)
            {
                Console.WriteLine($"Station: {dataPoint.StationName}, Time: {dataPoint.Timestamp}, Temperature: {dataPoint.Temperature}, Humidity: {dataPoint.Humidity}");
            }
        }
    }
}
