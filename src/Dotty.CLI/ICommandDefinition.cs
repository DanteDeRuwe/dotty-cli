namespace Dotty.CLI;

public interface ICommandDefinition
{
    public void Register(ICoconaAppBuilder app);
}