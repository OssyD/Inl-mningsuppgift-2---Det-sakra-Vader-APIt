using CsvHelper;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Results;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services.CSV
{
    internal class CSVService : ICSVService
    {
        public Task<Result<List<WeatherDataPoint>>> ReadCSVAsync()
        {
            var weatherDataPoints = new List<WeatherDataPoint>();
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "weather_data.csv");
                using var reader = CsvReader.FromFile(filePath);

                foreach(var row in reader.GetRecords<WeatherCSV>())
                {
                    var validationResult = WeatherCSVValidator.Validate(row);
                    if (validationResult.IsSuccess)
                    {
                        weatherDataPoints.Add(validationResult.Value!);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(Result<List<WeatherDataPoint>>.Failure(new Error(ErrorType.CSVReadError, "An error occurred while reading the CSV file.", ex)));
            }

            return Task.FromResult(Result<List<WeatherDataPoint>>.Success(weatherDataPoints));
        }
    }
}