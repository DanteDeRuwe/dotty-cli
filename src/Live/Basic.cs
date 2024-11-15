// switch (args)
// {
//     case []:
//         Console.WriteLine("No command provided. Possible commands: 'ping', 'generate'.");
//         return 1;
//     case ["ping"]:
//         Console.WriteLine("pong");
//         return 0;
//     case ["generate", "guid"]:
//     {
//         // Generate a GUID
//         Guid guid = Guid.NewGuid();
//         Console.WriteLine($"Generated GUID: {guid}");
//         return 0;
//     }
//     case ["generate", "number", .. var options]:
//     {
//         int from = 0, to = 100;
//         bool hasFrom = false, hasTo = false;
//
//         // Parse '--from' argument
//         if (options.Contains("--from"))
//         {
//             var index = Array.IndexOf(options, "--from");
//             hasFrom = index + 1 < options.Length && int.TryParse(options[index + 1], out from);
//             if(!hasFrom)
//             {
//                 Console.WriteLine("Invalid value for --from.");
//                 return 1;
//             }
//         }
//
//         // Parse '--to' argument
//         if (options.Contains("--to"))
//         {
//             var index = Array.IndexOf(options, "--to");
//             hasTo = index + 1 < options.Length && int.TryParse(options[index + 1], out to);
//             if(!hasTo)
//             {
//                 Console.WriteLine("Invalid value for --to.");
//                 return 1;
//             }
//         }
//
//         // Validate if 'from' is greater than 'to'
//         if (hasFrom && hasTo && from > to)
//         {
//             Console.WriteLine("The 'from' value cannot be greater than the 'to' value.");
//             return 1;
//         }
//
//         // Generate a random number in the specified range
//         Random rand = new Random();
//         var number = rand.Next(from, to + 1); // The upper bound is exclusive, so add +1 to include 'to'
//         Console.WriteLine($"Generated number: {number}");
//         return 0;
//     }
//     case ["generate", ..]:
//         Console.WriteLine("Invalid command. Use 'guid' or 'number'.");
//         return 1;
//     default:
//         Console.WriteLine("Unknown command.");
//         return 1;
// }
