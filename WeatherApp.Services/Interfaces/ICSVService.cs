using System;
using System.Collections.Generic;
using System.Text;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Results;

namespace WeatherApp.Services.Interfaces
{
    public interface ICSVService
    {
        Task<Result<int>> ImportCSVAsync();
    }
}
