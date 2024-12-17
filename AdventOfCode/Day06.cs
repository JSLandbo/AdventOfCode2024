using System.Numerics;

namespace AdventOfCode;

public sealed class Day06 : BaseDay
{
    private readonly string[] _input;

    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;
        var map = new char[_input.Length, _input[0].Length, 2];
        var currentPosition = (X: 0, Y: 0);
        for (var i = 0; i < _input.Length; i++)
        for (var j = 0; j < _input[i].Length; j++)
            if ((map[i, j, 0] = _input[i][j]) == '^') currentPosition = (j, i);
        var direction = (X: 0, Y: -1);
        while (true)
        {   // Loop until trailing off map, and turn right if there's a hindrance
            map[currentPosition.Y, currentPosition.X, 1] = 'X'; // Mark step on map
            var nextPosition = (X: currentPosition.X + direction.X, Y: currentPosition.Y + direction.Y);
            if (TrailingOffMap3D(map, nextPosition))
            {   // Trailing off map? Then end the loop and count tiles stepped on. 
                result = GetCountOfUniqueTilesSteppedOn(map);
                break;
            }
            if (map[nextPosition.Y, nextPosition.X, 0] == '#')
            {   // Oops, hindrance hit! Turn right... 
                direction = RotateRight(direction);
                continue;
            }
            currentPosition = nextPosition;
        }

        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var infiniteLoops = 0;
        for (var i = 0; i < _input.Length; i++) // Width * Height: Check all possible blockades
            for (var j = 0; j < _input[i].Length; j++) 
                infiniteLoops += DoesBlockadeCauseInfiniteLoop(_input, (i, j));
        
        return new ValueTask<string>($"{infiniteLoops}");
    }

    // Helper methods
    
    private static bool TrailingOffMap2D(char[,] map, (int X, int Y) next) => next.X >= map.GetLength(1) || next.Y >= map.GetLength(0) || next.X < 0 || next.Y < 0;
    
    private static bool TrailingOffMap3D(char[,,] map, (int X, int Y) next) => next.X >= map.GetLength(1) || next.Y >= map.GetLength(0) || next.X < 0 || next.Y < 0;
    
    private static int GetCountOfUniqueTilesSteppedOn(char[,,] map) => Enumerable.Range(0, map.GetLength(0)).SelectMany(i => Enumerable.Range(0, map.GetLength(1)).Where(j => map[i, j, 1] == 'X')).Count();
    
    private static (int X, int Y) RotateRight((int X, int Y) direction) => direction switch
    {
        (0, -1) => (1, 0), // Up -> Right
        (1, 0) => (0, 1), // Right -> Down
        (0, 1) => (-1, 0), // Down -> Left
        (-1, 0) => (0, -1), // Left -> Up
        _ => direction
    };
    
    private static int DoesBlockadeCauseInfiniteLoop(string[] file, (int X,int Y) blockade)
    {
        if (file[blockade.X][blockade.Y] != '.') return 0; // Blockade must be on empty space
        var map = new char[file.Length, file[0].Length]; // Map of the area
        var currentPosition = (X:0, Y:0);
        for (var i = 0; i < file.Length; i++) // Fill map and find starting position
        for (var j = 0; j < file[i].Length; j++) 
            if ((map[i, j] = file[i][j]) == '^') currentPosition = (X:j, Y:i);
        var steps = new Dictionary<(int X, int Y), List<(int X, int Y)>>(); // Steps taken
        var direction = (X:0, Y:-1); // Move up initially
        map[blockade.X, blockade.Y] = '#'; // Insert blockade into map
        while (true)
        {
            if (steps.TryGetValue(currentPosition, out var step))
            {   // If we've been here before with the same direction then it's an infinite loop.
                if (step.Any(dir => dir == direction)) return 1;
                step.Add(direction); // Add direction to steps. 
            }
            else steps[currentPosition] = [direction]; // First time here, add direction.
            var nextPosition = (X:currentPosition.X + direction.X, Y:currentPosition.Y + direction.Y);
            if (TrailingOffMap2D(map, nextPosition)) return 0; // Trailing off map, no infinite loop.
            if (map[nextPosition.Y, nextPosition.X] == '#')
            {   // We will hit a blockade if we continue, turn right.
                direction = RotateRight(direction);
                continue;
            }
            // Move to next position
            currentPosition = nextPosition;
        }
    }
}