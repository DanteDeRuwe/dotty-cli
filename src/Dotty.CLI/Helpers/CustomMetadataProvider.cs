using Cocona.Application;

namespace Dotty.CLI.Helpers;

public class CustomMetadataProvider(ICoconaApplicationMetadataProvider inner) : ICoconaApplicationMetadataProvider
{
    public string GetProductName() => "dotty";
    public string GetExecutableName() => "dotty";
    public string GetVersion() => $"v{inner.GetVersion()}";
    public string GetDescription() => inner.GetDescription();
}