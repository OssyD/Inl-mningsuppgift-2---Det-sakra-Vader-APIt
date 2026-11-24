using WeatherApp.Domain.Models;
using WeatherApp.Domain.Results;

namespace WeatherApp.Services.Interfaces;

public interface IOpenMeteoService
{
    Task<Result<ImportSummary>> GetWeatherFromOpenMeteo();
}
