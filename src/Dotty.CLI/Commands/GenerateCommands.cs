using Bogus;

namespace Dotty.CLI.Commands;

public class GenerateCommands : ICommandDefinition
{
    public void Register(ICoconaAppBuilder app)
    {
        app.AddSubCommand("generate", group =>
            {
                group.AddSubCommand("random", subgroup =>
                {
                    subgroup.AddCommand("guid", () => Panel(Guid.NewGuid().ToString()))
                        .WithDescription("Generates a random GUID");

                    subgroup.AddCommand("number", ([Option('m')] int min = 0, [Option('M')] int max = 100)
                            => Panel(Random.Shared.Next(min, max).ToString()))
                        .WithDescription("Generates a random number within a range");

                    subgroup
                        .AddCommand("fromtemplate", ([Argument] string template) => Panel(new Faker().Parse(template)))
                        .WithDescription("""
                                         Can generate random data from a template using the Bogus library. 
                                         For example, `generate random fromtemplate '{{name.firstName(Male)}} {{name.lastName}}'`
                                         """);
                })
                .WithDescription("Contains commands to generate certain random data");

                group.AddCommand("timestamp", (string format = "o") => Panel(DateTimeOffset.UtcNow.ToString(format)))
                    .WithDescription("Generates a timestamp for the current datetime with an optional format string");
            })
            .WithDescription("Contains commands to generate certain data");
    }
}