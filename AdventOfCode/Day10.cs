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
        var trails = trailhead.Select(e => FindCountOfTrails([], (e.x, e.y), map, true)).Sum();
        return new ValueTask<string>($"{trails}");
    }

    public override ValueTask<string> Solve_2()
    {
        var (trailhead, map) = GetMap(_input);
        var trails = trailhead.Select(e => FindCountOfTrails([], (e.x, e.y), map, false)).Sum();
        return new ValueTask<string>($"{trails}");
    }
    
    // Helper methods
    
    private static int FindCountOfTrails(HashSet<(int x, int y)> taken, (int x,  int y) pos, int[][] map, bool distinct)
    {
        if (distinct && !taken.Add(pos)) return 0;
        var value = map[pos.y][pos.x];
        if (value == 9) return 1;
        var sum = 0;
        if (LeftIsNext()) sum += FindCountOfTrails(taken, (pos.x - 1, pos.y), map, distinct);
        if (RightIsNext()) sum += FindCountOfTrails(taken, (pos.x + 1, pos.y), map, distinct);
        if (UpIsNext()) sum += FindCountOfTrails(taken, (pos.x, pos.y - 1), map, distinct);
        if (DownIsNext()) sum += FindCountOfTrails(taken, (pos.x, pos.y + 1), map, distinct);
        return sum;
        bool LeftIsNext() => pos.x != 0 && map[pos.y][pos.x - 1] == value + 1;
        bool RightIsNext() => pos.x != map[0].Length - 1 && map[pos.y][pos.x + 1] == value + 1;
        bool UpIsNext() => pos.y != 0 && map[pos.y - 1][pos.x] == value + 1;
        bool DownIsNext() => pos.y != map.Length - 1 && map[pos.y + 1][pos.x] == value + 1;
    }
    
    private static (List<(int x, int y)> trailheads, int[][] map) GetMap(string[] input)
    {
        List<(int x, int y)> trailheads = [];
        var map = new int[input.Length][];
        for (var y = 0; y < input.Length; y++) 
        {
            map[y] = new int[input[y].Length];
            for (var x = 0; x < input[y].Length; x++) 
            {
                var n = input[y][x] - '0';
                if (n == 0) trailheads.Add((x, y));
                map[y][x] = n;
            }
        }
        return (trailheads, map);
    }
}