using Dotty.CLI.IntegrationTest.Helpers;
using FluentAssertions;

namespace Dotty.CLI.IntegrationTest;

public class GreetCommandsTests(DockerContainerFixture fixture) : IClassFixture<DockerContainerFixture>
{
    [Fact]
    public async Task GreetCommand_ShouldShowCorrectGreeting()
    {
        var container = fixture.GetContainerForCommand("greet tests");
        await container.StartAsync();
        var (stdout, stderr) = await container.GetLogsAsync();
        stdout.Should().Contain("Hello, tests!");
    }
}

