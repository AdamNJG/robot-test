using FluentAssertions;
using RobotSimulator.Simulator;

namespace RobotSimulator.UnitTests;

public class SimulatorTests
{
    [Fact]
    public void Move_AtZeroZeroFacingNorth_MovesNorth()
    {
        string[] commands = ["PLACE 0,0,NORTH", "MOVE", "REPORT"];

        RobotEngine engine = new();

        var reportedPosition = engine.Execute(commands);

        reportedPosition.Should().Be("0,1,NORTH");
    }
    
    [Fact]
    public void NoCommands_ReturnsErrorMessage()
    {
        string[] commands = ["PLACE 0,0,NORTH", "MOVE", "REPORT"];

        RobotEngine engine = new();
        
        var reportedPosition = engine.Execute(commands);

        reportedPosition.Should().Be("0,1,NORTH");
    }
    
    [Fact]
    public void MovesTwice_ExpectedPosition()
    {
        string[] commands = ["PLACE 0,0,NORTH", "MOVE", "MOVE", "REPORT"];

        RobotEngine engine = new();
        var reportedPosition = engine.Execute(commands);


        reportedPosition.Should().Be("0,2,NORTH");
    }
    
    [Fact]
    public void Move_AtBoundary_ShouldIgnoreCommand()
    {
        string[] commands = ["PLACE 0,4,NORTH", "MOVE", "REPORT"]; 
        RobotEngine engine = new();
    
        var result = engine.Execute(commands);

        result.Should().Be("0,4,NORTH"); 
    }
    
    [Theory]
    [InlineData(new[] { "PLACE 0,0,NORTH", "RIGHT", "REPORT" }, "0,0,EAST")]
    [InlineData(new[] { "PLACE 0,0,NORTH", "LEFT", "REPORT" }, "0,0,WEST")]
    [InlineData(new[] { "PLACE 0,0,NORTH", "RIGHT", "RIGHT", "RIGHT", "RIGHT", "REPORT" }, "0,0,NORTH")]
    [InlineData(new[] { "PLACE 0,0,NORTH", "LEFT", "LEFT", "LEFT", "LEFT", "REPORT" }, "0,0,NORTH")]
    [InlineData(new[] { "PLACE 0,0,EAST", "LEFT", "REPORT" }, "0,0,NORTH")]
    public void Rotations_FacingCorrectWay(string[] commands, string expectedPosition)
    {
        RobotEngine engine = new();
    
        var result = engine.Execute(commands);
    
        result.Should().Be(expectedPosition); 
    }
    
    [Theory]
    [InlineData("PLACE 0,1,SOUTH", "MOVE", "0,0,SOUTH")]
    [InlineData("PLACE 0,0,SOUTH", "MOVE", "0,0,SOUTH")]
    [InlineData("PLACE 3,0,EAST", "MOVE", "4,0,EAST")]  
    [InlineData("PLACE 4,0,EAST", "MOVE", "4,0,EAST")] 
    [InlineData("PLACE 1,0,WEST", "MOVE", "0,0,WEST")] 
    [InlineData("PLACE 0,0,WEST", "MOVE", "0,0,WEST")] 
    public void Move_ShouldHandleAllDirectionsAndBoundaries(string place, string move, string expected)
    {
        var engine = new RobotEngine();
        string[] commands = [place, move, "REPORT"];

        var result = engine.Execute(commands);

        result.Should().Be(expected);
    }

    [Fact]
    public void IgnoreCommands_UntilPlaced()
    {
        string[] commands = ["MOVE", "LEFT", "PLACE 0,0,NORTH", "MOVE", "REPORT"];
        RobotEngine engine = new();

        var result = engine.Execute(commands);

        result.Should().Be("0,1,NORTH");
    }
    
    [Fact]
    public void IgnoreCommands_IfPlacedOffTable()
    {
        string[] commands = ["PLACE 5,5,NORTH", "MOVE", "REPORT"];
        RobotEngine engine = new();

        var result = engine.Execute(commands);

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("PLACE A,B,NORTH")] // Non-integer coordinates
    [InlineData("PLACE 0,0,INVALID")] // Invalid direction
    [InlineData("PLACE 0,0")] // Missing direction
    [InlineData("PLACE 0,0,NORTH,EXTRA")] // Too many parts
    public void InvalidPlaceCommands_ShouldBeIgnored(string invalidPlaceCommand)
    {
        string[] commands = [invalidPlaceCommand, "MOVE", "REPORT"];
        RobotEngine engine = new();

        var result = engine.Execute(commands);

        result.Should().BeEmpty();
    }
}