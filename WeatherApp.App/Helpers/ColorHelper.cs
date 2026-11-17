namespace WeatherApp.App.Helpers;

public static class ColorHelper
{
    public static string TemperatureColor(decimal temperature) => temperature switch
    {
        < 0 => "blue",
        >= 10 => "red",
        _ => "default"
    };
}
