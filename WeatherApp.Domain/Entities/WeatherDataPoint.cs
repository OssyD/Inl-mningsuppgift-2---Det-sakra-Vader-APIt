namespace WeatherApp.Domain.Entities;

public class WeatherDataPoint
{
    public int Id { get; set; }
    public required string StationName { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public required string DataSource { get; set; }
}
