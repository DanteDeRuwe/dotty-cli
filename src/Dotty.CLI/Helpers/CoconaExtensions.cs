using System.Reflection;
using Dotty.CLI.Commands;

namespace Dotty.CLI.Helpers;

public static class CoconaExtensions
{
    public static void AddCommandsFromAssemblies(this ICoconaAppBuilder app, params IEnumerable<Assembly> assemblies)
    {
        var commands = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t is { IsInterface: false, IsAbstract: false } && t.IsAssignableTo(typeof(ICommandDefinition)))
            .Select(t => Activator.CreateInstance(t) as ICommandDefinition);
        
        foreach (var command in commands) command?.Register(app);
    }
}