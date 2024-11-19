using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Spectre.Console;

namespace Dotty.CLI.Commands;

public class WeatherCommands : ICommandDefinition
{
    public void Register(ICoconaAppBuilder app)
    {
        app.AddCommand("weather", ExecuteWeatherCommand)
            .WithDescription("Gets the weather for a specific city");
    }

    private static async Task ExecuteWeatherCommand([FromService] IWeatherService weatherService, [Argument] string city)
    {
        var weather = await weatherService.GetWeather(city);
        if (weather is null)
        {
            Error($"Could not find weather for {city}");
            return;
        }

        var dailyWeatherAtNoon = Enumerable
            .Zip(weather.Hourly.Time, weather.Hourly.Temperature, weather.Hourly.Humidity)
            .Select((values, i) => new { Time = values.Item1, Temperature = values.Item2, Humidity = values.Item3 })
            .GroupBy(x => x.Time.Hour)
            .Where(g => g.Key == 12)
            .SelectMany(g => g);

        var table = new Table().AddColumns("Time", "Temperature", "Humidity").Centered();
        AnsiConsole.Live(table).Start(ctx =>
        {
            foreach (var w in dailyWeatherAtNoon)
            {
                table.AddRow(new TableRow([
                    new Markup($"[bold]{w.Time:dd MMMM}[/]"),
                    new Text($"{w.Temperature:F1}°C"),
                    new Text($"{w.Humidity:F1}%")
                ]));
                ctx.Refresh();
                Thread.Sleep(250); // Simulate slower rendering
            }
        });
    }
}

public interface IWeatherService
{
    Task<Weather?> GetWeather(string city);
}

public class WeatherService(IHttpClientFactory clientFactory) : IWeatherService
{
    public async Task<Weather?> GetWeather(string city)
    {
        using var cityClient = clientFactory.CreateClient("CityClient");
        var cityResponse = await cityClient.GetFromJsonAsync<CityResponse>($"/v1/search?name={city}&count=1&language=en&format=json");
        if (cityResponse is not { Results: { Length: >= 0 } cityResults }) return null;

        var cities = cityResults.Select(r => (r.Latitude, r.Longitude)).ToArray();
        var (lat, lon) = cities.First();

        using var weatherClient = clientFactory.CreateClient("WeatherClient");
        var response = await weatherClient.GetFromJsonAsync<Weather>($"/v1/forecast?latitude={lat.ToString("F", CultureInfo.InvariantCulture)}&longitude={lon.ToString("F", CultureInfo.InvariantCulture)}&hourly=temperature_2m,relative_humidity_2m");

        return response;
    }
}

file record CityResponse(
    [property: JsonPropertyName("results")]
    CityResults[] Results);

file record CityResults(
    [property: JsonPropertyName("latitude")]
    double Latitude,
    [property: JsonPropertyName("longitude")]
    double Longitude);

public record Hourly(
    [property: JsonPropertyName("time")] DateTime[] Time,
    [property: JsonPropertyName("temperature_2m")]
    double[] Temperature,
    [property: JsonPropertyName("relative_humidity_2m")]
    double[] Humidity);

public record Weather(
    [property: JsonPropertyName("hourly")] Hourly Hourly);