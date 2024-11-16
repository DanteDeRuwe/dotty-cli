using System.Diagnostics;

namespace Dotty.CLI.Commands;

public class PresentCommands : ICommandDefinition
{
    public void Register(ICoconaAppBuilder app)
    {
        app.AddSubCommand("present", group =>
        {
            group.AddCommand("slides", () =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://docs.google.com/presentation/d/1WABgifl2J70RuZjVP8MJkAf4k5TWAohhuNUtBViE5Fs/present",
                    UseShellExecute = true
                });
            });
        });
    }
}