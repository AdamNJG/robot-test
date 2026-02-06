using RobotSimulator.Simulator;

namespace RobotSimulator;

public static class Program
{
    public static void Main(string[] args)
    {
        RobotEngine engine = new();

        if (args.Length > 0)
        {
            string output = engine.Execute(args);
            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine(output);
            }
            return;
        }
        
        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.Trim().Equals("EXIT", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            var tokens = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
            
            string output = engine.Execute(tokens);
            
            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine(output);
            }
        }
    }
}