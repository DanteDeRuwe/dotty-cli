using System.Reflection;
using Cocona.Application;

namespace Dotty.CLI.Helpers;

public class CustomMetadataProvider : ICoconaApplicationMetadataProvider
{
    public string GetProductName() => "dotty";
    public string GetExecutableName() => "dotty";
    public string GetVersion() => $"v{GetAssemblyAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty}";
    public string GetDescription() => GetAssemblyAttribute<AssemblyDescriptionAttribute>()?.Description ?? string.Empty;

    private static T? GetAssemblyAttribute<T>() where T : Attribute => Assembly.GetEntryAssembly()!.GetCustomAttribute<T>();
}