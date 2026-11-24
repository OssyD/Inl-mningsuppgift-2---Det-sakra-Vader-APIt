using System.Globalization;
using WeatherApp.Domain.Constants;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Results;

namespace WeatherApp.Services.CSV;

internal static class WeatherCSVValidator
{
    private const NumberStyles Number = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
    private const decimal MinTemperature = -60m;
    private const decimal MaxTemperature = 80m;
    private const decimal MinHumidity = 0m;
    private const decimal MaxHumidity = 100m;

    public static Result<WeatherDataPoint> Validate(WeatherCSV row)
    {

        if (string.IsNullOrWhiteSpace(row.StationName))
            return Fail("Stationsnamn saknas");

        if (!DateTime.TryParseExact(row.Timestamp, "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
            return Fail($"Ogiltig tidpunkt: '{row.Timestamp}'");

        if (!TryParseDecimal(row.Temperature, out var temperature))
            return Fail($"Ogiltig temperatur: '{row.Temperature}'");

        if (temperature < MinTemperature || temperature > MaxTemperature)
            return Fail($"Orimlig temperatur: {temperature}");

        if (!TryParseDecimal(row.Humidity, out var humidity))
            return Fail($"Ogiltig luftfuktighet: '{row.Humidity}'");

        if (humidity < MinHumidity || humidity > MaxHumidity)
            return Fail($"Orimlig luftfuktighet: {humidity}");

        return Result<WeatherDataPoint>.Success(new WeatherDataPoint
        {
            StationName = row.StationName.Trim(),
            Timestamp = timestamp,
            Temperature = temperature,
            Humidity = humidity,
            DataSource = DataSourceNames.CsvFile
        });
    }

    private static bool TryParseDecimal(string value, out decimal result) =>
        decimal.TryParse(value.Replace(',', '.'), Number, CultureInfo.InvariantCulture, out result);
    private static Result<WeatherDataPoint> Fail(string reason) =>
        Result<WeatherDataPoint>.Failure(new Error(ErrorType.Validation, reason));
}
