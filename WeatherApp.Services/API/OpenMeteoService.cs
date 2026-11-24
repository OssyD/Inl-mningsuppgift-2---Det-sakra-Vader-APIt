using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using WeatherApp.Domain.Constants;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Models;
using WeatherApp.Domain.Results;
using WeatherApp.Services.API.models;
using WeatherApp.Services.Helpers;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services.API;

internal class OpenMeteoService(IConfiguration configuration, IBadDataLogger dataLogger, IWeatherDataRepository weatherDataRepository) : IOpenMeteoService
{
    private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(15) };

    public async Task<Result<ImportSummary>> GetWeatherFromOpenMeteo()
    {
        OpenMeteoResponse? response;
        try
        {
            response = await Client.GetFromJsonAsync<OpenMeteoResponse>(BuildUrl());
        }
        catch (Exception ex)
        {
            await dataLogger.ErrorLogAsync($"Error fetching weather data: {ex}");
            return Result<ImportSummary>.Failure(
                new Error(ErrorType.Network, $"Could not reach Open-Meteo: {ex.Message}", ex));
        }

        if (response?.Hourly is null)
            return Result<ImportSummary>.Failure(
                new Error(ErrorType.NotFound, "Open-Meteo did not return any hourly data."));

        var (points, rejected) = MapToWeatherDataPoints(response.Hourly);

        var saved = await DatabaseCall.ExecuteAsync(() => weatherDataRepository.AddNewAsync(points));
        if (!saved.IsSuccess)
            return Result<ImportSummary>.Failure(saved.Error!);

        return Result<ImportSummary>.Success(new ImportSummary(saved.Value, rejected));
    }

    private string BuildUrl()
    {
        var baseUrl = configuration["OpenMeteo:BaseUrl"];
        var timezone = Uri.EscapeDataString(configuration["OpenMeteo:Timezone"]!);
        var latitude = configuration["OpenMeteo:Locations:0:Latitude"];
        var longitude = configuration["OpenMeteo:Locations:0:Longitude"];

        return $"{baseUrl}?latitude={latitude}&longitude={longitude}" +
               $"&hourly=temperature_2m,relative_humidity_2m&timezone={timezone}";
    }

    private (List<WeatherDataPoint> Points, int Rejected) MapToWeatherDataPoints(Hourly hourly)
    {
        var points = new List<WeatherDataPoint>();
        var rejected = 0;

        for (var i = 0; i < hourly.Time.Count; i++)
        {
            var temperature = hourly.Temperature.ElementAtOrDefault(i);
            var humidity = hourly.Humidity.ElementAtOrDefault(i);

            if (temperature is null || humidity is null)
            {
                rejected++;
                continue;
            }

            points.Add(new WeatherDataPoint
            {
                StationName = configuration["OpenMeteo:Locations:0:StationName"]!,
                Timestamp = hourly.Time[i],
                Temperature = temperature.Value,
                Humidity = humidity.Value,
                DataSource = DataSourceNames.Api
            });
        }

        return (points, rejected);
    }
}
