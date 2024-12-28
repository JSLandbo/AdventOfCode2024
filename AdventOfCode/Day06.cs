using System.Collections;

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
        int result;
        var currentPosition = (X: 0, Y: 0);
        var map = new char[_input.Length, _input[0].Length, 2]; // Map of the area
        // Fill map and find starting position
        for (var y = 0; y < _input.Length; y++)
        {
            for (var x = 0; x < _input[y].Length; x++)
            {
                if ((map[y, x, 0] = _input[y][x]) == '^')
                {
                    currentPosition = (X: x, Y: y); 
                }
            }
        }
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
                direction = direction switch
                {
                    (0, -1) => (1, 0), // Up -> Right
                    (1, 0) => (0, 1), // Right -> Down
                    (0, 1) => (-1, 0), // Down -> Left
                    (-1, 0) => (0, -1), // Left -> Up
                    _ => direction
                };
                continue;
            }
            currentPosition = nextPosition;
        }
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var start = (X: 0, Y: 0); // Starting position        
        var map = new char[_input.Length, _input[0].Length]; // Map of the area
        for (var y = 0; y < _input.Length; y++)
        {
            for (var x = 0; x < _input[y].Length; x++)
            {
                if ((map[y, x] = _input[y][x]) == '^')
                {
                    start = (X: x, Y: y); 
                }
            }
        }
        var infiniteLoops = 0;
        Parallel.For(0, _input.Length, (i) =>
        {
            for (var j = 0; j < _input[i].Length; j++)
            {
                Interlocked.Add(ref infiniteLoops, DoesBlockadeCauseInfiniteLoop(map, start, (X: j, Y: i)));
            }
        });
        return new ValueTask<string>($"{infiniteLoops}");
    }

    // Helper methods
    
    private static bool TrailingOffMap2D(int width, int height, (int X, int Y) next) => next.X >= width || next.Y >= height || next.X < 0 || next.Y < 0;
    
    private static bool TrailingOffMap3D(char[,,] map, (int X, int Y) next) => next.X >= map.GetLength(1) || next.Y >= map.GetLength(0) || next.X < 0 || next.Y < 0;
    
    private static int GetCountOfUniqueTilesSteppedOn(char[,,] map) => Enumerable.Range(0, map.GetLength(0)).SelectMany(i => Enumerable.Range(0, map.GetLength(1)).Where(j => map[i, j, 1] == 'X')).Count();
    
    private static int DoesBlockadeCauseInfiniteLoop(char[,] map, (int X, int Y) position, (int X,int Y) blockade)
    {
        if (map[blockade.Y, blockade.X] != '.') return 0; // Blockade must be on empty space
        var width = map.GetLength(1);
        var height = map.GetLength(0);
        var visited = new BitArray(height * width * 4);
        var direction = (X:0, Y:-1); // Move up initially
        var directionIndex = 0; 
        while (true)
        {   // If we've been here before with the same direction then it's an infinite loop.
            var positionIndex = (position.Y * width + position.X) * 4 + directionIndex; // Unique index
            if (visited[positionIndex]) return 1;
            visited[positionIndex] = true;
            var nextPosition = (X:position.X + direction.X, Y:position.Y + direction.Y);
            if (TrailingOffMap2D(width, height, nextPosition)) return 0; // Trailing off map, no infinite loop.
            if (map[nextPosition.Y, nextPosition.X] == '#' || nextPosition == blockade)
            {   // We will hit a blockade if we continue, turn right.
                directionIndex = (directionIndex + 1) % 4;
                direction = directionIndex switch
                {   // Rotate direction clockwise (up -> right -> down -> left)
                    0 => (X: 0, Y: -1),
                    1 => (X: 1, Y: 0),
                    2 => (X: 0, Y: 1),
                    3 => (X: -1, Y: 0),
                    _ => direction
                };
                continue;
            }
            position = nextPosition;
        }
    }
}