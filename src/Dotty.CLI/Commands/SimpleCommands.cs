using Cocona.Application;
using Cocona.Help;
using Spectre.Console;

namespace Dotty.CLI.Commands;

public class SimpleCommands : ICommandDefinition
{
    public void Register(ICoconaAppBuilder app)
    {
        app.AddCommand("greet", ([Argument] string subject) => Panel($"Hello, {subject}!"));

        app.AddCommand(([FromService] ICoconaApplicationMetadataProvider metadataProvider, [FromService] ICoconaHelpMessageBuilder helpProvider) =>
        {
            Panel(new Rows(
                new FigletText("dotty").Color(Color.Aqua).Centered(),
                new Markup($"[bold][aqua]{metadataProvider.GetVersion()}[/][/]").Centered(),
                new Rule { Style = Style.Parse("gray") },
                new Text("\n"),
                new Text(helpProvider.BuildAndRenderForCurrentCommand())
            ));
        });
    }
}