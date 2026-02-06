namespace RobotSimulator.Simulator;

public static class CommandCleaner
{
    public static string[] CleanCommands(string[] input)
    {
        List<string> cleaned = [];

        for (var i = 0; i < input.Length; i++)
        {
            var trimmedToken = input[i].Trim();
            
            if (trimmedToken.StartsWith("PLACE ", StringComparison.OrdinalIgnoreCase))
            {
                var coordsPart = trimmedToken[5..].Trim(); 
                cleaned.Add($"PLACE {coordsPart}");
                continue;
            }
            
            if (input[i] == "PLACE" && i + 1 < input.Length)
            {
                cleaned.Add($"{input[i]} {input[i + 1]}");
                i++; 
                continue;
            }
            
            if (TryGetCommand(input[i], out var command))
            {
                cleaned.Add(command);
            }
        }

        return cleaned.ToArray();
    } 
    
    private static bool TryGetCommand(string input, out string command)
    {
        command = string.Empty;
        if (string.IsNullOrWhiteSpace(input)) return false;
        
        var normalized = input.Trim().ToUpper();
        
        string[] validVerbs = ["MOVE", "LEFT", "RIGHT", "REPORT"];

        if (!validVerbs.Contains(normalized)) return false;
        
        command = normalized;
        return true;
    }
}