using CliWrap;
using Cocona;
using Cocona.Application;
using Dotty.CLI;
using Microsoft.Extensions.DependencyInjection;
using static Dotty.CLI.Output;

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
            return;
        }

        Panel($"""
               This talk is presented to you by {name}, a technical consultant, 
               software developer and public speaker passionate about .NET and fascinated 
               by software craftsmanship and architecture.
               """);
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