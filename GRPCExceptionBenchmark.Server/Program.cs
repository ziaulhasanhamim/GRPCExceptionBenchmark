using GRPCExceptionBenchmark.Server.Services;
using GRPCExceptionBenchmark.Server.GrpcServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddScoped<IWeatherService, WeatherService>();

var app = builder.Build();

app.MapGrpcService<WeatherForecaster>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
