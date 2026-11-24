using WeatherApp.Domain.Models;
using WeatherApp.Domain.Results;

namespace WeatherApp.Services.Interfaces;

public interface ICSVService
{
    Task<Result<ImportSummary>> ImportCSVAsync();
}
