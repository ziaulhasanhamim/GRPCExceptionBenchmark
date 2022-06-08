using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GRPCExceptionBenchmark.Server.Protos;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;

Console.Write("Server Addr: ");
var addr = Console.ReadLine();


var clientFactoryBetter = HttpClientFactory.Create();
var stepBetter = Step.Create("GetWeatherBetter", clientFactoryBetter, async context =>
{
    using var channel = GrpcChannel.ForAddress(addr!, new GrpcChannelOptions()
    {
        HttpClient = context.Client,
        DisposeHttpClient = false
    });
    var client = new WeatherForecaster.WeatherForecasterClient(channel);
    try
    {
        await client.GetWeatherBetterAsync(new() { CityName = "Dhaka" });
        return Response.Fail();
    }
    catch (RpcException ex)
    {
        return ex.StatusCode == StatusCode.InvalidArgument ? Response.Ok() : Response.Fail();
    }
});

var scenarioBetter = ScenarioBuilder.CreateScenario("scenarioBetter", stepBetter)
    .WithWarmUpDuration(TimeSpan.FromSeconds(50))
    .WithLoadSimulations(Simulation.KeepConstant(32, TimeSpan.FromSeconds(200)));

NBomberRunner
    .RegisterScenarios(scenarioBetter)
    .Run();

var clientFactory = HttpClientFactory.Create();
var step = Step.Create("GetWeather", clientFactory, async context =>
{
    using var channel = GrpcChannel.ForAddress(addr!, new GrpcChannelOptions()
    {
        HttpClient = context.Client,
        DisposeHttpClient = false
    });
    var client = new WeatherForecaster.WeatherForecasterClient(channel);
    try
    {
        await client.GetWeatherAsync(new() { CityName = "Dhaka" });
        return Response.Fail();
    }
    catch (RpcException ex)
    {
        return ex.StatusCode == StatusCode.InvalidArgument ? Response.Ok() : Response.Fail();
    }
});

var scenario = ScenarioBuilder.CreateScenario("scenario", step)
    .WithWarmUpDuration(TimeSpan.FromSeconds(50))
    .WithLoadSimulations(Simulation.KeepConstant(32, TimeSpan.FromSeconds(200)));


NBomberRunner
    .RegisterScenarios(scenario)
    .Run();
