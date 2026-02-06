using FluentAssertions;
using RobotSimulator.Simulator;

namespace RobotSimulator.UnitTests;

public class CommandCleanerTests
{
    [Fact]
    public void ValidCommands_ReturnsArrayOfCommands()
    {
        string[] inputCommands = ["PLACE", "0,0,NORTH", "MOVE", "REPORT"];
        string[] expectedCommands = ["PLACE 0,0,NORTH", "MOVE", "REPORT"];

        string[] outputCommands = CommandCleaner.CleanCommands(inputCommands);

        outputCommands.Should().BeEquivalentTo(expectedCommands);
    }        
    
    [Fact]
    public void ValidCommands_PlaceAlreadyCombined_ReturnsArrayOfCommands()
    {
        string[] inputCommands = ["PLACE 0,0,NORTH", "MOVE", "REPORT"];
        string[] expectedCommands = ["PLACE 0,0,NORTH", "MOVE", "REPORT"];

        string[] outputCommands = CommandCleaner.CleanCommands(inputCommands);

        outputCommands.Should().BeEquivalentTo(expectedCommands);
    }    
    
    [Fact]
    public void NoPlaceCommand_ReturnsAllCommands()
    {
        string[] inputCommands = ["MOVE", "LEFT", "RIGHT" ,"REPORT"];
        string[] expectedCommands = ["MOVE", "LEFT", "RIGHT" ,"REPORT"];

        string[] outputCommands = CommandCleaner.CleanCommands(inputCommands);

        outputCommands.Should().BeEquivalentTo(expectedCommands);
    }    
    
    [Fact]
    public void NoPlaceCommand_NoCommands_Empty()
    {
        string[] inputCommands = [];
        string[] expectedCommands = [];

        string[] outputCommands = CommandCleaner.CleanCommands(inputCommands);

        outputCommands.Should().BeEquivalentTo(expectedCommands);
    }    
    
    [Fact]
    public void PlaceCommand_WithoutPlaceCoordinates_Empty()
    {
        string[] inputCommands = ["PLACE"];
        string[] expectedCommands = [];

        string[] outputCommands = CommandCleaner.CleanCommands(inputCommands);

        Assert.Equal(expectedCommands, outputCommands);
    }
    
    [Fact]
    public void ImproperlyFormattedCommands_ReturnsArrayOfCommands()
    {
        string[] inputCommands = ["  PLACE   1,1,NORTH  ", " move ", "  REPORT  "];
        string[] expectedCommands = ["PLACE 1,1,NORTH", "MOVE", "REPORT"];

        string[] outputCommands = CommandCleaner.CleanCommands(inputCommands);

        outputCommands.Should().BeEquivalentTo(expectedCommands);
    }
}