using System.Text.Json.Serialization;

namespace WeatherApp.Services.API.models;

internal class Hourly
{
    [JsonPropertyName("time")]
    public List<DateTime> Time { get; set; } = [];

    [JsonPropertyName("temperature_2m")]
    public List<decimal?> Temperature { get; set; } = [];

    [JsonPropertyName("relative_humidity_2m")]
    public List<decimal?> Humidity { get; set; } = [];
}
