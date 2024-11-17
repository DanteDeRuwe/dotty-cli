using Dotty.CLI.IntegrationTest.Helpers;
using FluentAssertions;

namespace Dotty.CLI.IntegrationTest;

public class ConvertCommandsTests(DockerContainerFixture fixture) 
    : IClassFixture<DockerContainerFixture>
{
    [Fact]
    public async Task ConvertUnits_ShouldConvertInchesToCentimeters()
    {
        var container = fixture.GetContainerForCommand("convert units 1 in --to cm");
        await container.StartAsync();
        var (stdout, stderr) = await container.GetLogsAsync();
        stderr.Should().BeEmpty();
        stdout.Should().Contain("2.54 Centimeters");
    }
    
    [Fact]
    public async Task ConvertUnits_ShouldNotConvertInchesToKilograms()
    {
        var container = fixture.GetContainerForCommand("convert units 1 cm --to kg");
        await container.StartAsync();
        var (stdout, stderr) = await container.GetLogsAsync();
        stderr.Should().Contain("kg is not a valid unit for Length");
        stdout.Should().BeEmpty();
    }
}