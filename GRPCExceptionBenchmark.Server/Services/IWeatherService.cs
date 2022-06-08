namespace GRPCExceptionBenchmark.Server.Services;

using GRPCExceptionBenchmark.Server.Dtos;

public interface IWeatherService
{
    Task<WeatherForecast> GetWeather(string cityName);
    Task<WeatherForecast?> GetWeatherBetter(string cityName);
}
