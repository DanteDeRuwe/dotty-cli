namespace Dotty.CLI.Commands;

public class SimpleCommands : ICommandDefinition
{
    public void Register(ICoconaAppBuilder app)
    {
        app.AddCommand("greet", ([Argument] string subject) => Panel($"Hello, {subject}!"));
    }
}