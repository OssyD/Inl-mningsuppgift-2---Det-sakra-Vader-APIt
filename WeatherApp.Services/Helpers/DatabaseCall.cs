using WeatherApp.Domain.Results;

namespace WeatherApp.Services.Helpers;

public static class DatabaseCall
{
    public static async Task<Result<T>> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return Result<T>.Success(await operation());
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(new Error(ErrorType.Database, "Could not communicate with the database.", ex));
        }
    }
}
