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
                    FileName = "https://docs.google.com/presentation/d/e/2PACX-1vQsfvb5kOIzYOGehsM9y88kjP_Xl8_T1PsD6lPosESupl8EwK9hVxmLmjomTfnjlQ3lwiXop2noJBEe/pub?start=true&loop=false&delayms=3000",
                    UseShellExecute = true
                });
            });
        })
        .WithDescription("Opens the presentation slides for the talk");
    }
}