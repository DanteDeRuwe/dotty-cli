using Spectre.Console;

namespace Dotty.CLI.Helpers;

public static class Output
{
    public static void Panel(string message) =>
        AnsiConsole.Write(new Panel(message) { Border = BoxBorder.Rounded });

    public static void Error(string message) =>
        AnsiConsole.MarkupLine($":police_car_light: [bold red]Error:[/] {message}");

    public static T Select<T>(string title, params IEnumerable<T> choices) where T : notnull =>
        AnsiConsole.Prompt(new SelectionPrompt<T>().Title(title).AddChoices(choices).EnableSearch());
    
    public static void WriteItalic<T>(T value) => AnsiConsole.Markup($"[italic]{value}[/]");
}
