using CsvHelper;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Results;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services.CSV
{
    internal class CSVService(IBadDataLogger badDataLogger) : ICSVService
    {
        public async Task<Result<List<WeatherDataPoint>>> ReadCSVAsync()
        {
            var weatherDataPoints = new List<WeatherDataPoint>();
            var rejected = new List<string>();
            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "weather_data.csv");

            var options = CsvReaderOptions.For<WeatherCSV>() with
            {
                Separator = '\t',
                TrackLineNumbers = true,
                FieldCountMismatch = CsvFieldCountHandling.Allow,
                MissingField = CsvMissingFieldHandling.ReturnEmpty
            };
            try
            {
                using var reader = CsvReader.FromFile(filePath, options);

                while (await reader.ReadAsync())
                {
                    var row = reader.GetRecord<WeatherCSV>();
                    var rawLine = reader.Current.ToString();

                    var validationResult = WeatherCSVValidator.Validate(row);

                    if (validationResult.IsSuccess)
                        weatherDataPoints.Add(validationResult.Value!);
                    else
                        rejected.Add($"Line {reader.LineNumber}: {rawLine} | {validationResult.Error!.Message}");

                    await badDataLogger.WriteAsync(rejected);
                }
            }
            catch (Exception ex)
            {
                return Result<List<WeatherDataPoint>>.Failure(new Error(ErrorType.CSVReadError, "An error occurred while reading the CSV file.", ex));
            }

            return Result<List<WeatherDataPoint>>.Success(weatherDataPoints);
        }
    }
}