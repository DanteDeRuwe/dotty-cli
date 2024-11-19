using System.Text;
using UnitsNet;
using static System.StringComparison;

namespace Dotty.CLI.Commands;

public class ConvertCommands : ICommandDefinition
{
    public void Register(ICoconaAppBuilder app)
    {
        app.AddSubCommand("convert", group =>
        {
            group.AddCommand("units", ConvertUnits)
                .WithDescription("Converts a value from one unit of measurement to another");
            group.AddCommand("tobase64", ([Argument] string input) => Panel(Convert.ToBase64String(Encoding.UTF8.GetBytes(input))))
                .WithDescription("Converts a string to a base64 encoded string");
            group.AddCommand("frombase64", ([Argument] string input) => Panel(Encoding.UTF8.GetString(Convert.FromBase64String(input))))
                .WithDescription("Converts a base64 encoded string to a regular string");
        })
        .WithDescription("Contains commands to convert values");
    }


    private static void ConvertUnits([Argument] double value, [Argument] string unit, [Option] string? to)
    {
        // get quantity from
        if (!Quantity.TryFromUnitAbbreviation(value, unit, out var quantity))
        {
            var byName = Quantity.Infos.SelectMany(q => q.UnitInfos).FirstOrDefault(u =>
                u.Name.Equals(unit, OrdinalIgnoreCase) || u.PluralName.Equals(unit, OrdinalIgnoreCase));

            if (byName is null || !Quantity.TryFrom(value, byName.Value, out quantity))
            {
                Error($"{value} {unit} is not a valid input");
                return;
            }
        }

        // Get unit to
        to ??= Select("Select the unit to convert to...", quantity.QuantityInfo.UnitInfos.Select(u => u.Name));

        var unitToInfo = quantity.QuantityInfo.UnitInfos.FirstOrDefault(
            u => u.Name.Equals(to, OrdinalIgnoreCase)
                 || u.PluralName.Equals(to, OrdinalIgnoreCase)
                 || (UnitsNetSetup.Default.UnitParser.TryParse(to, quantity.QuantityInfo.UnitType, out var toEnum) &&
                     u.Value.Equals(toEnum)));

        if (unitToInfo is null)
        {
            Error($"{to} is not a valid unit for {quantity.QuantityInfo.Name}");
            return;
        }


        // Convert & print
        var convertedValue = quantity.ToUnit(unitToInfo.Value);
        Panel($"{value} {quantity.Unit} is equal to {convertedValue.Value} {unitToInfo.PluralName}");
    }
}