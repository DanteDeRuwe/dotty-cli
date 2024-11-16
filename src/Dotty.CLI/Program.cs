using Cocona.Application;
using Dotty.CLI.Helpers;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<ICoconaApplicationMetadataProvider>(new CustomMetadataProvider(new CoconaApplicationMetadataProvider()));

var app = builder.Build();

app.AddCommandsFromAssemblies(typeof(Program).Assembly);

app.Run();