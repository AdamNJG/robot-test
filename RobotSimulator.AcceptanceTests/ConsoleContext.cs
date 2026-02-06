namespace RobotSimulator.AcceptanceTests;

public class ConsoleContext: IDisposable
{
    private readonly StringWriter _out = new();
    private readonly TextWriter _originalOut = Console.Out;
    private readonly TextReader _originalIn = Console.In;

    public ConsoleContext(string input = "")
    {
        Console.SetOut(_out);
        Console.SetIn(new StringReader(input));
    }

    public string GetOutput() => _out.ToString().Trim();

    public void Dispose()
    {
        Console.SetOut(_originalOut);
        Console.SetIn(_originalIn);
    }
}