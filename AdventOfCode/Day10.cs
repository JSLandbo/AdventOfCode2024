namespace AdventOfCode;

public sealed class Day10 : BaseDay
{
    private readonly string[] _input;

    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var (trailhead, map) = GetMap(_input);

        return default;
    }

    public override ValueTask<string> Solve_2()
    {
        var (trailhead, map) = GetMap(_input);
        
        return default;
    }
    
    private static (List<(int x, int y)> trailheads, int[,] map) GetMap(string[] input)
    {
        List<(int x, int y)> trailheads = [];
        var map = new int[input.Length, input[0].Length];
        for (var y = 0; y < input.Length; y++) 
        {
            for (var x = 0; x < input[y].Length; x++) 
            {
                var n = input[y][x] - '0';
                if (n == 0) trailheads.Add((x, y));
                map[y, x] = n;
            }
        }
        return (trailheads, map);
    }
}