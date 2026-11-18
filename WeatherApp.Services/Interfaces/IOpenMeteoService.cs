using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Results;

namespace WeatherApp.Services.Interfaces;

public interface IOpenMeteoService
{
    Task<Result<List<WeatherDataPoint>>> GetWeatherFromOpenMeteo();
}
