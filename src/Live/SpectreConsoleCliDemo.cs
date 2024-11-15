using System.ComponentModel;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddBranch("generate", group =>
    {
        group.AddCommand<GenerateGuidCommand>("guid");
        group.AddCommand<GenerateNumberCommand>("number");
    });
});

return app.Run(args);

public class GenerateGuidCommand : Command
{
    public override int Execute(CommandContext context)
    {
        Console.WriteLine($"Generated GUID: {Guid.NewGuid()}");
        return 0;
    }
}

public class GenerateNumberCommand : Command<GenerateNumberCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--from")]
        [DefaultValue(0)]
        public int From { get; init; }

        [CommandOption("--to")]
        [DefaultValue(100)]
        public int To { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (settings.From > settings.To)
        {
            Console.WriteLine("Error: The 'from' value cannot be greater than the 'to' value");
            return 1;
        }

        var number = Random.Shared.Next(settings.From, settings.To);
        Console.WriteLine($"Generated number: {number}");
        return 0;
    }
}
