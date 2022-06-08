namespace GRPCExceptionBenchmark.Server.GrpcServices;

using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GRPCExceptionBenchmark.Server.Protos;
using GRPCExceptionBenchmark.Server.Services;
using WeatherForecasterBase = GRPCExceptionBenchmark.Server.Protos.WeatherForecaster.WeatherForecasterBase;

public class WeatherForecaster : WeatherForecasterBase
{
    private readonly IWeatherService _weatherService;

    public WeatherForecaster(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public override async Task<WeatherResponse> GetWeather(WeatherRequest request, Grpc.Core.ServerCallContext context)
    {
        try
        {
            var weatherForcest = await _weatherService.GetWeather(request.CityName);
            return new()
            {
                DateTime = Timestamp.FromDateTime(weatherForcest.DateTime),
                Temperature = weatherForcest.Temperature,
                Summary = weatherForcest.Summary
            };
        }
        catch (ValidationException ex)
        {
            throw new RpcException(new(StatusCode.InvalidArgument, ex.Message));
        }
    }

    public override async Task<WeatherResponse> GetWeatherBetter(WeatherRequest request, Grpc.Core.ServerCallContext context)
    {
        var weatherForcest = await _weatherService.GetWeatherBetter(request.CityName);
        if (weatherForcest is null)
        {
            context.Status = new(StatusCode.InvalidArgument, $"City {request.CityName} not found");
            return new();
        }
        return new()
        {
            DateTime = Timestamp.FromDateTime(weatherForcest.DateTime),
            Temperature = weatherForcest.Temperature,
            Summary = weatherForcest.Summary
        };
    }
}
