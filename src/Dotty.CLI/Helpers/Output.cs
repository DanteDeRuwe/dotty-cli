using Spectre.Console;
using Spectre.Console.Rendering;

namespace Dotty.CLI.Helpers;

internal static class Output
{
    private static IAnsiConsole ErrorConsole { get; } = AnsiConsole.Create(new AnsiConsoleSettings
    {
        Ansi = AnsiSupport.Detect,
        ColorSystem = ColorSystemSupport.Detect,
        Out = new AnsiConsoleOutput(Console.Error),
    });

    public static void Panel(IRenderable renderable, bool expand = true) =>
        AnsiConsole.Write(new Panel(renderable) { Border = BoxBorder.Rounded, Expand = expand });

    public static void Panel(string message, bool expand = true) =>
        Panel(new Text(message), expand);

    public static void Error(string message) =>
        ErrorConsole.MarkupLine($":police_car_light: [bold red]Error:[/] {message}");

    public static T Select<T>(string title, params IEnumerable<T> choices) where T : notnull =>
        AnsiConsole.Prompt(new SelectionPrompt<T>().Title(title).AddChoices(choices).EnableSearch());

    public static void WriteItalic<T>(T value) => AnsiConsole.Markup($"[italic]{value}[/]");

    public static void WriteBold<T>(T value) => AnsiConsole.Markup($"[bold]{value}[/]");
}