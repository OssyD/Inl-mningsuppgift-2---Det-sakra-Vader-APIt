using System.Text.Json.Serialization;

namespace WeatherApp.Services.API.models;

internal class HourlyUnits
{
    [JsonPropertyName("time")]
    public string Time { get; set; } = string.Empty;

    [JsonPropertyName("temperature_2m")]
    public string Temperature { get; set; } = string.Empty;

    [JsonPropertyName("relative_humidity_2m")]
    public string Humidity { get; set; } = string.Empty;
}
