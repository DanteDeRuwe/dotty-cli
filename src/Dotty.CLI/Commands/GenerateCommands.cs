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
                subgroup.AddCommand("guid", () => Panel(Guid.NewGuid().ToString()));
                
                subgroup.AddCommand("number", ([Option('m')] int min = 0, [Option('M')] int max = 100)
                    => Panel(Random.Shared.Next(min, max).ToString()));

                subgroup
                    .AddCommand("fromtemplate", ([Argument] string template) => Panel(new Faker().Parse(template)))
                    .WithDescription("""
                                     Can generate random data from a template using the Bogus library. 
                                     For example, `generate random fromtemplate '{{name.firstName(Male)}} {{name.lastName}}'`
                                     """);
            });

            group.AddCommand("timestamp", (string format = "o") => Panel(DateTimeOffset.UtcNow.ToString(format)));
        });
    }
}