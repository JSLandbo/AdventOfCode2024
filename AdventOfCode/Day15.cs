namespace AdventOfCode;

public sealed class Day15 : BaseDay
{
    private readonly string[] _input;

    public Day15()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var info = GetInfo(_input);
        foreach (var step in info.steps)
        {
            AttemptStep(step, ref info.map, ref info.position);
        }
        var sum = 0;
        for (var y = 0; y < info.map.GetLength(0); y++)
        {
            for (var x = 0; x < info.map.GetLength(1); x++)
            {
                if (info.map[y, x] == 'O')
                {
                    sum += 100 * y + x;
                }
            }
        }
        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2()
    {
        return default;
    }
    
    // Helper methods

    private static void AttemptStep((int x, int y) step, ref char[,] map, ref (int x, int y) position)
    {
        var next = map[position.y + step.y, position.x + step.x];
        if (next == '#') return; // Cannot move in that direction
        if (next == '.')
        {   // Move player only, free space
            MovePlayer(step, ref map, ref position);
            return;
        }
        var info = GetBlocksToMove(step, ref map, ref position);
        if (!info.canMove) return; // No space to move, next...
        MoveBlocks(step, ref map, info.blocks);
        MovePlayer(step, ref map, ref position);
    }

    private static (bool canMove, List<(int x, int y)> blocks) GetBlocksToMove((int x, int y) step, ref char[,] map, ref (int x, int y) position)
    {
        var blocks = new List<(int x, int y)>();
        var width = map.GetLength(1);
        var height = map.GetLength(0);
        var canMove = false;
        for (var i = 0; i > -1; i++)
        {
            var nextx = position.x + step.x * i;
            var nexty = position.y + step.y * i;
            if (nextx < 0 || nextx >= width || nexty < 0 || nexty >= height) break;
            if (map[nexty, nextx] == '#') break;
            if (map[nexty, nextx] == '.')
            {
                canMove = true;
                break;
            }
            blocks.Add((nextx, nexty));
        }
        if (!canMove) blocks.Clear();
        return (canMove, blocks);
    }
    
    private static void MoveBlocks((int x, int y) step, ref char[,] map, List<(int x, int y)> blocks)
    {
        foreach (var block in blocks)
        {
            map[block.y + step.y, block.x + step.x] = 'O';
        }
    }
    
    private static void MovePlayer((int x, int y) step, ref char[,] map, ref (int x, int y) position)
    {
        map[position.y, position.x] = '.';
        position.x += step.x;
        position.y += step.y;
        map[position.y, position.x] = '@';
    }
    
    private static (char[,] map, (int x, int y)[] steps, (int x, int y) position) GetInfo(string[] input)
    {
        var height = Array.IndexOf(input, string.Empty);
        var width = input[0].Length;
        var position = (x: -1, y: -1);
        var map = new char[height, width];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if ((map[y, x] = input[y][x]) == '@')
                {
                    position = (x, y);
                }
            }
        }
        var steps = new List<(int x, int y)>();
        for (var y = height + 1; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                steps.Add(GetStep(input[y][x]));
            }
        }
        return (map, steps.ToArray(), position);
    }

    private static (int x, int y) GetStep(char ch) => ch switch
    {
        '>' => (1, 0), '<' => (-1, 0), '^' => (0, -1), 'v' => (0, 1), _ => (0, 0),
    };
}