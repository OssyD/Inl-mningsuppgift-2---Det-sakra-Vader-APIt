using CsvHelper;

[CsvRecord]
public partial class WeatherCSV
{
    [CsvColumn]  
    public string StationName { get; set; } = null!;

    [CsvColumn]
    public string Timestamp { get; set; } = null!;

    [CsvColumn]
    public string Temperature { get; set; } = null!;

    [CsvColumn]
    public string Humidity { get; set; } = null!;
}