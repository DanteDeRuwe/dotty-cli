using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

namespace Dotty.CLI.IntegrationTest.Helpers;

internal class UntilExitCode : IWaitUntil
{
    public async Task<bool> UntilAsync(IContainer container)
    {
        await container.GetExitCodeAsync();
        return true;
    }
}