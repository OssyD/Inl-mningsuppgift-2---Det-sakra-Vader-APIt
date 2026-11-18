using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherApp.Domain.Constants;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Results;
using WeatherApp.Services.API.models;
using WeatherApp.Services.Helpers;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services.API
{
    internal class OpenMeteoService(IConfiguration configuration, IBadDataLogger dataLogger, IWeatherDataRepository weatherDataRepository) : IOpenMeteoService
    {
        public async Task<Result<List<WeatherDataPoint>>> GetWeatherFromOpenMeteo()
        {
            try
            {
                using var client = new HttpClient();
                var baseUrl = configuration["OpenMeteo:BaseUrl"];
                var timezone = configuration["OpenMeteo:Timezone"];
                var latitude = configuration["OpenMeteo:Locations:0:Latitude"];
                var longitude = configuration["OpenMeteo:Locations:0:Longitude"];
                var url = $"{baseUrl}?latitude={latitude}&longitude={longitude}&hourly=temperature_2m,relative_humidity_2m&timezone={timezone}";

                var response = await client.GetFromJsonAsync<OpenMeteoResponse>(url);

                if (response == null)
                {
                    await dataLogger.ErrorLogAsync("Received null response from OpenMeteo API.");
                    return Result<List<WeatherDataPoint>>.Failure(new Error(ErrorType.NotFound, "Received null response from OpenMeteo API."));
                }

                var points = MapToWeatherDataPoints(response);

                await DatabaseCall.ExecuteAsync(() => weatherDataRepository.AddNewAsync(points));
                return Result<List<WeatherDataPoint>>.Success(MapToWeatherDataPoints(response));
            }
            catch (Exception ex)
            {
                await dataLogger.ErrorLogAsync($"Error fetching weather data: {ex}");
                return Result<List<WeatherDataPoint>>.Failure(new Error(ErrorType.Network, $"Error fetching weather data: {ex.Message}"));
            }
           


        }
        private List<WeatherDataPoint> MapToWeatherDataPoints(OpenMeteoResponse response)
        {
            var weatherDataPoints = new List<WeatherDataPoint>();

            if (response.Hourly == null || response.Hourly.Time.Count == 0)
                return weatherDataPoints;

            for (int i = 0; i < response.Hourly.Time.Count; i++)
            {
                var time = response.Hourly.Time[i];
                var temperature = response.Hourly.Temperature[i];
                var humidity = response.Hourly.Humidity[i];
                if (temperature.HasValue && humidity.HasValue)
                {
                    weatherDataPoints.Add(new WeatherDataPoint
                    {
                        StationName = configuration["OpenMeteo:Locations:0:StationName"]!,
                        Timestamp = time,
                        Temperature = temperature.Value,
                        Humidity = humidity.Value,
                        DataSource = DataSourceNames.Api
                    });
                }
            }
            return weatherDataPoints;
        }
    }
}
