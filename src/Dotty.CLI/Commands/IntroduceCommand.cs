namespace Dotty.CLI.Commands;

public class IntroduceCommand : ICommandDefinition
{
    public void Register(ICoconaAppBuilder app)
    {
        app.AddSubCommand("introduce", group =>
        {
            group.AddCommand("talk", () => Panel("Welcome to this talk on crafting modern CLI tools using .NET"))
                .WithDescription("Introduces the talk");

            group.AddCommand("speaker", IntroduceSpeaker)
                .WithDescription("Introduces the speaker");

            group.AddCommand("yourself", () =>
                {
                    Panel("""
                          Hi, I'm Dotty, a CLI tool written in C# with dotnet. 
                          I'm an assistant here to help you with various tasks!

                          PS: My name is a play on words: Dotnet + Clippy. Remember Clippy? 
                          """);
                })
                .WithDescription("Introduces Dotty");
        })
        .WithDescription("Contains commands to introduce various elements");
    }

    private static void IntroduceSpeaker([Option('n')] string? name = null)
    {
        name ??= Select("Select a speaker", "Dante De Ruwe", "Some other guy...");

        if (!name.Equals("Dante De Ruwe"))
        {
            Error("Speaker not found.");
            return;
        }

        Panel($"""
               This talk is presented to you by {name}.

               [gray]Technical Consultant | Software Developer | Public Speaker[/]

               Passionate about .NET and fascinated by software craftsmanship and architecture.

               :rocket: [bold][cyan][link=https://dantederuwe.com]dantederuwe.com[/][/][/]
               """);
    }
}