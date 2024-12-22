namespace AdventOfCode;

public sealed class Day08 : BaseDay
{
    private readonly string[] _input;

    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var map = _input.Select(e => e.ToArray()).ToArray();
        var nodes = GetNodes(map);
        
        return default;
    }

    public override ValueTask<string> Solve_2()
    {
        var map = _input.Select(e => e.ToArray()).ToArray();
        var nodes = GetNodes(map);
        
        return default;
    }

    private static Dictionary<char, HashSet<(int x, int y)>> GetNodes(char[][] map)
    {
        Dictionary<char, HashSet<(int x, int y)>> nodes = [];
        for (var y = 0; y < map[0].Length - 1; y++)
        {
            for (var x = 0; x < map.Length - 1; x++)
            {
                var ch = map[y][x];
                if (ch == '.') continue;
                if (!nodes.TryAdd(ch, [])) nodes[ch].Add((x,y));
            }
        }
        
        return nodes;
    }
}