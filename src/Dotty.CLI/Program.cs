using CliWrap;
using Cocona;
using Cocona.Application;
using Dotty.CLI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using static Dotty.CLI.Output;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<ICoconaApplicationMetadataProvider>(_ =>
    new CustomMetadataProvider(new CoconaApplicationMetadataProvider()));

var options = builder.Configuration.Get<DottyOptions>();
builder.Services.Configure<DottyOptions>(builder.Configuration);

var aiOptions = options.Groq; // or options.OpenApi

#pragma warning disable SKEXP0010 //EXPERIMENTAL FEATURE!
builder.Services.AddOpenAIChatCompletion(aiOptions.Model, endpoint: new Uri(aiOptions.Endpoint), apiKey: aiOptions.ApiKey);
#pragma warning restore SKEXP0010


var app = builder.Build();

app.AddCommand("greet", ([Argument] string subject) => Panel($"Hello, {subject}!"));

app.AddCommand("ask", async ([FromService] IChatCompletionService chat, [Option('q')] string question, [Option('c')] string? context = null) =>
{
    var history = new ChatHistory();
    
    if (!string.IsNullOrEmpty(context))
    {
        history.AddSystemMessage(context);
    }
    
    history.AddUserMessage(question);
    
    await foreach (var response in chat.GetStreamingChatMessageContentsAsync(history))
    {
        WriteItalic(response);
        await Task.Delay(50); // simulate typing
    }
});

app.AddSubCommand("introduce", group =>
{
    group.AddCommand("talk", () => Panel("Welcome to this talk on crafting modern CLI tools using .NET"));
    group.AddCommand("speaker", ([Option('n')] string? name = null) =>
    {
        name ??= Select("Select a speaker", "Dante De Ruwe", "Some other guy...");

        if (!name.Equals("Dante De Ruwe"))
        {
            Error("Speaker not found.");
            return;
        }

        Panel($"""
               This talk is presented to you by {name}, a technical consultant, 
               software developer and public speaker passionate about .NET and fascinated 
               by software craftsmanship and architecture.
               """);
    });
});

app.AddSubCommand("present", group =>
{
    group.AddCommand("slides", async () =>
    {
        //open google slides and put in slideshow mode
        var edgePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "Microsoft", "Edge", "Application", "msedge.exe"
        );

        await Cli
            .Wrap(edgePath)
            .WithArguments([
                "https://docs.google.com/presentation/d/1WABgifl2J70RuZjVP8MJkAf4k5TWAohhuNUtBViE5Fs/present",
                "--new-window",
                "--start-fullscreen"
            ])
            .ExecuteAsync();
    });
});

app.Run();


file record DottyOptions
{
    public AIServiceOptions OpenApi { get; init; }
    public AIServiceOptions Groq { get; init; }
}

file record AIServiceOptions
{
    public required string ApiKey { get; init; }
    public required string Endpoint { get; init; }
    public required string Model { get; init; }
}
