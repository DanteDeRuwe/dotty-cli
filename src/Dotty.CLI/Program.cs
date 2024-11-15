using System.Text;
using Bogus;
using CliWrap;
using Cocona;
using Cocona.Application;
using Dotty.CLI;
using Microsoft.Extensions.DependencyInjection;
using UnitsNet;
using static System.StringComparison;
using static Dotty.CLI.Output;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<ICoconaApplicationMetadataProvider>(_ =>
    new CustomMetadataProvider(new CoconaApplicationMetadataProvider()));

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
    group.AddCommand("yourself", () =>
    {
        Panel("""
              Hi, I'm Dotty, a CLI tool written in C# with dotnet. 
              I'm an assistant here to help you with various tasks!

              PS: My name is a play on words: Dotnet + Clippy. Remember Clippy? 
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


app.AddSubCommand("convert", group =>
{
    group.AddCommand("units", ([Argument] double value, [Argument] string unit, [Option] string? to) =>
    {
        // get quantity from
        if (!Quantity.TryFromUnitAbbreviation(value, unit, out var quantity))
        {
            var byName = Quantity.Infos.SelectMany(q => q.UnitInfos).FirstOrDefault(u =>
                u.Name.Equals(unit, OrdinalIgnoreCase) || u.PluralName.Equals(unit, OrdinalIgnoreCase));

            if (byName is null || !Quantity.TryFrom(value, byName.Value, out quantity))
            {
                Error($"{value} {unit} is not a valid input");
                return;
            }
        }

        // Get unit to
        to ??= Select("Select the unit to convert to...", quantity.QuantityInfo.UnitInfos.Select(u => u.Name));

        var unitToInfo = quantity.QuantityInfo.UnitInfos.FirstOrDefault(
            u => u.Name.Equals(to, OrdinalIgnoreCase)
                 || u.PluralName.Equals(to, OrdinalIgnoreCase)
                 || (UnitsNetSetup.Default.UnitParser.TryParse(to, quantity.QuantityInfo.UnitType, out var toEnum) &&
                     u.Value.Equals(toEnum)));

        if (unitToInfo is null)
        {
            Error($"{to} is not a valid unit for {quantity.QuantityInfo.Name}");
            return;
        }


        // Convert & print
        var convertedValue = quantity.ToUnit(unitToInfo.Value);
        Panel($"{value} {quantity.Unit} is equal to {convertedValue.Value} {unitToInfo.PluralName}");
    });

    group.AddCommand("tobase64",
        ([Argument] string input) => Panel(Convert.ToBase64String(Encoding.UTF8.GetBytes(input))));

    group.AddCommand("frombase64",
        ([Argument] string input) => Panel(Encoding.UTF8.GetString(Convert.FromBase64String(input))));
});

app.AddSubCommand("generate", group =>
{
    group.AddSubCommand("random", subgroup =>
    {
        subgroup.AddCommand("guid", () => Panel(Guid.NewGuid().ToString()));
        subgroup.AddCommand("number",
            ([Option('m')] int min = 0, [Option('M')] int max = 100) => Panel(Random.Shared.Next(min, max).ToString()));

        subgroup
            .AddCommand("fromtemplate", ([Argument] string template) => Panel(new Faker().Parse(template)))
            .WithDescription("""
                             Can generate random data from a template using the Bogus library. 
                             For example, `generate random fromtemplate '{{name.firstName(Male)}} {{name.lastName}}'`
                             """);
    });

    group.AddCommand("timestamp", (string format = "o") => { Panel(DateTimeOffset.UtcNow.ToString(format)); });
});

app.Run();
