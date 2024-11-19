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

                        subgroup.AddCommand("number", ([Option('f')] int from = 0, [Option('t')] int to = 100) =>
                            {
                                if (from >= to)
                                {
                                    Console.WriteLine("Error: The 'from' value cannot be greater than the 'to' value");
                                    return 1;
                                }

                                var number = Random.Shared.Next(from, to);
                                Panel($"Generated number: {number}");
                                return 0;
                            })
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