using Cocona;
using Spectre.Console;

var builder = CoconaApp.CreateBuilder();
var app = builder.Build();

app.AddCommand("greet", ([Argument] string subject) => Panel($"Hello, {subject}!"));

app.AddSubCommand("introduce", group =>
{
    group.AddCommand("talk", () => Panel("Welcome to this talk on crafting modern CLI tools using .NET"));
    group.AddCommand("speaker", ([Option] string? name = null) =>
    {
        name ??= Select("Select a speaker", "Dante De Ruwe", "Some other guy...");

        if (!name.Equals("Dante De Ruwe"))
        {
            return Error("Speaker not found.");
        }

        Panel($"""
               This talk is presented to you by {name}, a technical consultant, 
               software developer and public speaker passionate about .NET and fascinated 
               by software craftsmanship and architecture.
               """);
        return 0;
    });
});


app.Run();
return;

int Panel(string message)
{
    AnsiConsole.Write(new Panel(message) { Border = BoxBorder.Rounded });
    return 0;
}

int Error(string message)
{
    AnsiConsole.MarkupLine($"?? [bold red]Error:[/] {message}");
    return 1;
}

T Select<T>(string title, params IEnumerable<T> choices) where T : notnull
{
    return AnsiConsole.Prompt(new SelectionPrompt<T>().Title(title).AddChoices(choices));
}
