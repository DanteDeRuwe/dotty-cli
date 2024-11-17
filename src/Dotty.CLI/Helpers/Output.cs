using Spectre.Console;
using Spectre.Console.Rendering;

namespace Dotty.CLI.Helpers;

internal static class Output
{
    private static IAnsiConsole ErrorConsole => AnsiConsole.Create(new AnsiConsoleSettings
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

    public static void Tree(string root, Action<TreeBuilder> configure)
    {
        var builder = new TreeBuilder(root);
        configure(builder);
        AnsiConsole.Write(builder.Build());
    }
}

internal class TreeNodeBuilder(string name)
{
    private readonly TreeNode _node = new(new Text(name));

    public TreeNodeBuilder WithChild(string child, Action<TreeNodeBuilder>? configure = null)
    {
        var builder = new TreeNodeBuilder(child);
        configure?.Invoke(builder);
        return WithChild(builder.Build());
    }

    public TreeNodeBuilder WithChild(TreeNode node)
    {
        _node.AddNode(node);
        return this;
    }

    public TreeNode Build() => _node;
}

internal class TreeBuilder(string rootName)
{
    private readonly Tree _tree = new(rootName);

    public TreeBuilder WithChild(string child, Action<TreeNodeBuilder>? configure = null)
    {
        var builder = new TreeNodeBuilder(child);
        configure?.Invoke(builder);
        return WithChild(builder.Build());
    }

    public TreeBuilder WithChild(TreeNode node)
    {
        _tree.AddNode(node);
        return this;
    }

    public Tree Build() => _tree;
}