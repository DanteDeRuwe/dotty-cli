using Cocona.Application;
using Cocona.Help;
using Cocona.Help.DocumentModel;
using Spectre.Console;
using Spectre.Console.Rendering;

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
            })
            .OptionLikeCommand(x =>
            {
                x.Add("tree", () => Tree("Dotty CLI", t => t
                    .WithChild("greet")
                    .WithChild("convert", c => c
                        .WithChild("units")
                        .WithChild("tobase64")
                        .WithChild("frombase64")
                    )
                    .WithChild("generate", g => g
                        .WithChild("random", r => r
                            .WithChild("guid")
                            .WithChild("number")
                            .WithChild("fromtemplate")
                        )
                        .WithChild("timestamp")
                    )
                ));
            });
    }
}