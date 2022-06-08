namespace GRPCExceptionBenchmark.Server.Services;

using GRPCExceptionBenchmark.Server.Dtos;

public class WeatherService : IWeatherService
{
    public async Task<WeatherForecast> GetWeather(string cityName)
    {
        await Task.Yield();
        throw new ValidationException($"City {cityName} not found");
    }

    public async Task<WeatherForecast?> GetWeatherBetter(string cityName)
    {
        await Task.Yield();
        return null;
    }
}