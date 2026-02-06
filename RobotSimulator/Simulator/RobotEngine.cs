namespace RobotSimulator.Simulator;

public class RobotEngine
{
    private readonly string[] _directions = ["NORTH", "EAST", "SOUTH", "WEST"];
    private int _facingIndex; 
    private int _x, _y;
    private string _facing = "";
    private bool _isPlaced;

    public string Execute(string[] commands) 
    {
        var cleanedCommands = CommandCleaner.CleanCommands(commands);
        
        foreach (var cmd in cleanedCommands) 
        {
            if (cmd.StartsWith("PLACE"))
            {
                Place(cmd);
                continue;
            }

            if (!_isPlaced) continue;

            switch (cmd)
            {
                case "LEFT":
                    TurnLeft();
                    break;
                case "RIGHT":
                    TurnRight();
                    break;
                case "MOVE":
                    Move();
                    break;
                case "REPORT":
                    return Report();
            }
        }
        return "";
    }

    private void Place(string command)
    {
        var parts = command[6..].Split(',');
        
        if (parts.Length != 3) return;
        
        if (!int.TryParse(parts[0], out var x) || !int.TryParse(parts[1], out var y)) return;
        
        if (x is < 0 or > 4 || y is < 0 or > 4) return;
        
        var facing = parts[2];
        if (!Array.Exists(_directions, d => d == facing)) return;

        _x = x;
        _y = y;
        _facing = facing;
        _facingIndex = Array.IndexOf(_directions, _facing);
        _isPlaced = true;
    }

    private void TurnLeft()
    {
        _facingIndex = (_facingIndex + 3) % 4;
        _facing = _directions[_facingIndex];
    }

    private void TurnRight()
    {
        _facingIndex = (_facingIndex + 1) % 4;
        _facing = _directions[_facingIndex];
    }

    private void Move()
    {
        switch (_facing)
        {
            case "NORTH" when _y < 4:
                _y++;
                break;
            case "SOUTH" when _y > 0:
                _y--;
                break;
            case "EAST" when _x < 4:
                _x++;
                break;
            case "WEST" when _x > 0:
                _x--;
                break;
        }
    }

    private string Report()
    {
        return $"{_x},{_y},{_facing}";
    }
}