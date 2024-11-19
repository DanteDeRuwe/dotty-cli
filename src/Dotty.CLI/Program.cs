using Cocona.Application;
using Dotty.CLI.Commands;
using Dotty.CLI.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<ICoconaApplicationMetadataProvider, CustomMetadataProvider>();

// Restrict logging for  System.Net.Http.HttpClient to warning level
builder.Services.AddLogging(logging =>
{
    logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
});

builder.Services.AddHttpClient("WeatherClient", client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("CityClient", client =>
{
    client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddSingleton<IWeatherService, WeatherService>();


var app = builder.Build();

app.AddCommandsFromAssemblies(typeof(Program).Assembly);

app.Run();