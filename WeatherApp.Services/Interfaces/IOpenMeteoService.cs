using WeatherApp.Domain.Results;

namespace WeatherApp.Services.Interfaces;

public interface IOpenMeteoService
{
    Task<Result<int>> GetWeatherFromOpenMeteo();
}
