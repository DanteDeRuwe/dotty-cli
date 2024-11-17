using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

namespace Dotty.CLI.IntegrationTest.Helpers;

public class DockerContainerFixture : IAsyncLifetime
{
    public IFutureDockerImage Image { get; } = new ImageFromDockerfileBuilder()
        .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
        .WithDockerfile("Dockerfile")
        .Build();
    
    public IContainer GetContainerForCommand(string[] command) => new ContainerBuilder()
        .WithImage(Image)
        .WithCommand(command)
        .WithWaitStrategy(Wait.ForUnixContainer().AddCustomWaitStrategy(new UntilExitCode()))
        .Build();
    
    public IContainer GetContainerForCommand(string command) => 
        GetContainerForCommand(command.Split(' '));

    public Task InitializeAsync() => Image.CreateAsync();
    public Task DisposeAsync() => Image.DisposeAsync().AsTask();
}