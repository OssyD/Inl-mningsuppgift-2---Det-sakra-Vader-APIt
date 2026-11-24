using CsvHelper;
using Microsoft.Extensions.Configuration;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.Models;
using WeatherApp.Domain.Results;
using WeatherApp.Services.Helpers;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services.CSV;

internal class CSVService(IBadDataLogger badDataLogger, IWeatherDataRepository dataRepository, IConfiguration configuration) : ICSVService
{
    public async Task<Result<ImportSummary>> ImportCSVAsync()
    {
        var data = await ReadCSVAsync();
        if (!data.IsSuccess)
            return Result<ImportSummary>.Failure(data.Error!);

        var (points, rejectedCount) = data.Value;

        var saved = await DatabaseCall.ExecuteAsync(() => dataRepository.AddNewAsync(points));
        if (!saved.IsSuccess)
            return Result<ImportSummary>.Failure(saved.Error!);

        return Result<ImportSummary>.Success(new ImportSummary(saved.Value, rejectedCount));
    }

    private async Task<Result<(List<WeatherDataPoint> Points, int RejectedCount)>> ReadCSVAsync()
    {
        var weatherDataPoints = new List<WeatherDataPoint>();
        var rejected = new List<string>();
        var filePath = Path.Combine(AppContext.BaseDirectory, configuration["FileImport:FilePath"]!);

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
            }

            await badDataLogger.BadLogAsync(rejected);
        }
        catch (Exception ex)
        {
            await badDataLogger.ErrorLogAsync($"Error reading CSV file: {ex}");
            return Result<(List<WeatherDataPoint>, int)>.Failure(
                new Error(ErrorType.FileAccess, "An error occurred while reading the CSV file.", ex));
        }

        return Result<(List<WeatherDataPoint>, int)>.Success((weatherDataPoints, rejected.Count));
    }
}
