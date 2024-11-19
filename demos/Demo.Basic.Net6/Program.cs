if (!args.Any())
{
    Console.Error.WriteLine("No command provided. Possible commands: 'ping', 'generate'.");
    return 1;
}

switch (args[0])
{
    case "ping":
        Console.WriteLine("pong");
        return 0;
    case "generate":
        switch (args[1])
        {
            case "guid":
                Console.WriteLine($"Generated GUID: {Guid.NewGuid()}");
                return 0;
            case "number":
                return ExecuteGenerateNumberCommand(args.Skip(2).ToArray());
            default:
                Console.Error.WriteLine("Invalid command. Use 'guid' or 'number'.");
                return 1;
        }
    default:
        Console.WriteLine("Unknown command.");
        return 1;
}

int ExecuteGenerateNumberCommand(string[] options)
{
    int from = 0, to = 100;

    // Parse '--from' argument
    if (options.Contains("--from"))
    {
        var index = Array.IndexOf(options, "--from");
        var hasFrom = index + 1 < options.Length && int.TryParse(options[index + 1], out from);
        if (!hasFrom)
        {
            Console.Error.WriteLine("Invalid value for --from.");
            return 1;
        }
    }

    // Parse '--to' argument
    if (options.Contains("--to"))
    {
        var index = Array.IndexOf(options, "--to");
        var hasTo = index + 1 < options.Length && int.TryParse(options[index + 1], out to);
        if (!hasTo)
        {
            Console.Error.WriteLine("Invalid value for --to.");
            return 1;
        }
    }

    // Validate: If 'from' is greater than 'to', error!
    if (from >= to)
    {
        Console.Error.WriteLine("The 'from' value cannot be greater than the 'to' value.");
        return 1;
    }

    // Generate a random number in the specified range
    var number = Random.Shared.Next(from, to);
    Console.WriteLine($"Generated number: {number}");
    return 0;
}
