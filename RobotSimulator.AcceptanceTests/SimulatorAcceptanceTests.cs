using FluentAssertions;

namespace RobotSimulator.AcceptanceTests;

public class SimulatorAcceptanceTests
{
    [Fact]
    public void ValidCommands_CorrectPosition()
    {
        string[] args = ["PLACE", "0,0,NORTH", "MOVE", "REPORT"];
        using var context = new ConsoleContext();
        
        Program.Main(args);
        
        context.GetOutput().Should().EndWith("0,1,NORTH");
    }    
    
    [Fact]
    public void ValidCommands_HandlingPlaceUsingPiping_CorrectPosition()
    {
        string[] args = ["PLACE 0,0,NORTH", "MOVE", "REPORT"];
        using var context = new ConsoleContext();
        
        Program.Main(args);
        
        context.GetOutput().Should().EndWith("0,1,NORTH");
    }    

    [Fact]
    public void NoCommands_ReturnsErrorMessage()
    {
        using var context = new ConsoleContext();
        
        Program.Main([]);
        
        context.GetOutput().Should().BeEmpty();
    }
    
    [Fact]
    public void MovingTwice_ShouldMove_WhenTwoMoveCommandsProvided()
    {
        string[] args = ["PLACE", "0,0,NORTH", "MOVE", "MOVE", "REPORT"];
        using var context = new ConsoleContext();
    
        Program.Main(args);
    
        context.GetOutput().Should().EndWith("0,2,NORTH");
    }    
    
    [Fact]
    public void Moving_AtBoundary_DoesNotMove()
    {
        string[] args = ["PLACE", "0,4,NORTH", "MOVE", "REPORT"];
        using var context = new ConsoleContext();
    
        Program.Main(args);
    
        context.GetOutput().Should().EndWith("0,4,NORTH");
    }
    
    [Fact]
    public void Commands_BeforePlace_ShouldBeIgnored()
    {
        string[] args = ["MOVE", "REPORT", "PLACE 1,1,NORTH", "REPORT"];
        using var context = new ConsoleContext();
    
        Program.Main(args);
    
        context.GetOutput().Should().EndWith("1,1,NORTH");
    }
    
    [Theory]
    [InlineData(new[] { "PLACE 0,0,NORTH", "RIGHT", "REPORT" }, "0,0,EAST")]
    [InlineData(new[] { "PLACE 0,0,NORTH", "LEFT", "REPORT" }, "0,0,WEST")]
    [InlineData(new[] { "PLACE 0,0,NORTH", "RIGHT", "RIGHT", "RIGHT", "RIGHT", "REPORT" }, "0,0,NORTH")]
    [InlineData(new[] { "PLACE 0,0,NORTH", "LEFT", "LEFT", "LEFT", "LEFT", "REPORT" }, "0,0,NORTH")]
    [InlineData(new[] { "PLACE 0,0,EAST", "LEFT", "REPORT" }, "0,0,NORTH")]
    public void Rotations_FacingCorrectWay(string[] args, string expectedPosition)
    {
        using var context = new ConsoleContext();
    
        Program.Main(args);
    
        context.GetOutput().Should().EndWith(expectedPosition);
    }
    
    [Fact]
    public void ValidCommands_WithExtraWhitespace_ShouldStillWork()
    {
        string[] args = ["  PLACE   1,1,NORTH  ", " move ", "  REPORT  "];
        const string expectedPosition = "1,2,NORTH";
        
        using var context = new ConsoleContext();
    
        Program.Main(args);
    
        context.GetOutput().Should().EndWith(expectedPosition);
    }
    
    [Theory]
    [InlineData("PLACE 0,1,SOUTH", "MOVE", "0,0,SOUTH")] 
    [InlineData("PLACE 0,0,SOUTH", "MOVE", "0,0,SOUTH")] 
    [InlineData("PLACE 3,0,EAST", "MOVE", "4,0,EAST")]  
    [InlineData("PLACE 4,0,EAST", "MOVE", "4,0,EAST")] 
    [InlineData("PLACE 1,0,WEST", "MOVE", "0,0,WEST")]  
    [InlineData("PLACE 0,0,WEST", "MOVE", "0,0,WEST")]  
    public void ValidCommands_ShouldHandleAllDirectionsAndBoundaries(string place, string move, string expected)
    {
        string[] args = [place, move, "REPORT"];
        using var context = new ConsoleContext();
    
        Program.Main(args);
    
        context.GetOutput().Should().EndWith(expected);
    }

    [Fact]
    public void Streaming_ValidCommands_CorrectPosition()
    {
        var input = "PLACE 0,0,NORTH\nMOVE\nREPORT\nEXIT";
        using var context = new ConsoleContext(input);

        Program.Main([]);

        context.GetOutput().Should().EndWith("0,1,NORTH");
    }

    [Fact]
    public void Streaming_MultipleCommandsOnOneLine_CorrectPosition()
    {
        var input = "PLACE 0,0,NORTH MOVE REPORT\nEXIT";
        using var context = new ConsoleContext(input);

        Program.Main([]);

        context.GetOutput().Should().EndWith("0,1,NORTH");
    }

    [Fact]
    public void Streaming_Interactive_MultipleReports()
    {
        var input = "PLACE 0,0,NORTH\nREPORT\nMOVE\nREPORT\nEXIT";
        using var context = new ConsoleContext(input);

        Program.Main([]);

        var output = context.GetOutput();
        output.Should().Contain("0,0,NORTH");
        output.Should().EndWith("0,1,NORTH");
    }
}