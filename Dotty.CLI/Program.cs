using CliWrap;
using Cocona;
using Cocona.Application;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<ICoconaApplicationMetadataProvider>(_ => new CustomMetadataProvider(new CoconaApplicationMetadataProvider()));

var app = builder.Build();

app.AddCommand("greet", ([Argument] string subject) => Panel($"Hello, {subject}!"));

app.AddSubCommand("introduce", group =>
{
    group.AddCommand("talk", () => Panel("Welcome to this talk on crafting modern CLI tools using .NET"));
    group.AddCommand("speaker", ([Option('n')] string? name = null) =>
    {
        name ??= Select("Select a speaker", "Dante De Ruwe", "Some other guy...");

        if (!name.Equals("Dante De Ruwe"))
        {
            Error("Speaker not found.");
            return 1;
        }

        Panel($"""
               This talk is presented to you by {name}, a technical consultant, 
               software developer and public speaker passionate about .NET and fascinated 
               by software craftsmanship and architecture.
               """);
        return 0;
    });
});

app.AddSubCommand("present", group =>
{
    group.AddCommand("slides", async () =>
    {
        //open google slides and put in slideshow mode
        var edgePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "Microsoft", "Edge", "Application", "msedge.exe"
        );

        await Cli
            .Wrap(edgePath)
            .WithArguments([
                "https://docs.google.com/presentation/d/1WABgifl2J70RuZjVP8MJkAf4k5TWAohhuNUtBViE5Fs/present",
                "--new-window",
                "--start-fullscreen"
            ])
            .ExecuteAsync();
    });
});


app.Run();
return;

void Panel(string message) => AnsiConsole.Write(new Panel(message) { Border = BoxBorder.Rounded });

void Error(string message) => AnsiConsole.MarkupLine($":police_car_light: [bold red]Error:[/] {message}");

T Select<T>(string title, params IEnumerable<T> choices) where T : notnull
{
    return AnsiConsole.Prompt(new SelectionPrompt<T>().Title(title).AddChoices(choices));
}

public class CustomMetadataProvider(ICoconaApplicationMetadataProvider inner) : ICoconaApplicationMetadataProvider
{
    public string GetProductName() => "dotty";
    public string GetExecutableName() => "dotty";
    public string GetVersion() => $"v{inner.GetVersion()}";
    public string GetDescription() => inner.GetDescription();
}