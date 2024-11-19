using Cocona;
using Cocona.Filters;

var builder = CoconaApp.CreateBuilder();

// Define services here

var app = builder.Build();

// Commands without arguments
app.AddCommand("ping", () => Console.WriteLine("pong"))
    .WithDescription("Send a ping to the CLI")
    .WithAliases("p")
    .WithFilter(new DelegateCommandFilter(async (ctx, next) =>
    {
        Console.WriteLine("Before command execution");
        await next(ctx);
        Console.WriteLine("After command execution");
        return 0;
    }));

// SHOW HELP!

// Subcommands without arguments
app.AddSubCommand("generate", group =>
{
    group.AddCommand("guid", () => Console.WriteLine(Guid.NewGuid()));
    group.AddCommand("number", () => Console.WriteLine(Random.Shared.Next(0, 100)));
});


// Commands with arguments

// option by default, switch to argument
// app.AddCommand("echo", (string message) => Console.WriteLine(message)); 
app.AddCommand("echo", ([Argument] string message) => Console.WriteLine(message));

// Mix and match
app.AddCommand("repeat", (string message, int times) =>
{
    for (var i = 0; i < times; i++) Console.WriteLine(message);
});

app.Run();