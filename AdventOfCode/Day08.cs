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
        var width = map[0].Length;
        var height = map.Length;
        var nodes = GetNodes(map);
        var antinodes = CreateSimpleAntinodes(width, height, nodes);
        var result = antinodes.Cast<bool>().Count(b => b);
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var map = _input.Select(e => e.ToArray()).ToArray();
        var width = map[0].Length;
        var height = map.Length;
        var nodes = GetNodes(map);
        var antinodes = CreateResonatingAntinodes(width, height, nodes);
        var result = antinodes.Cast<bool>().Count(b => b);
        return new ValueTask<string>($"{result}");
    }

    // Helper methods
    
    private static Dictionary<char, HashSet<(int x, int y)>> GetNodes(char[][] map)
    {
        Dictionary<char, HashSet<(int x, int y)>> nodes = [];
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[0].Length; x++)
            {
                var ch = map[y][x];
                if (ch == '.') continue;
                nodes.TryAdd(ch, []);
                nodes[ch].Add((x,y));
            }
        }
        return nodes;
    }
    
    private static bool[,] CreateSimpleAntinodes(int width, int height, Dictionary<char, HashSet<(int x, int y)>> nodes)
    {
        var antinodes = new bool[height, width];
        
        foreach (var type in nodes)
        {
            if (type.Value.Count == 1) continue;
            foreach (var mainnode in type.Value)
            {
                foreach (var subnode in type.Value)
                {
                    if (mainnode == subnode) continue;
                    var dx = subnode.x - mainnode.x;
                    var dy = subnode.y - mainnode.y;
                    var antinodeX = mainnode.x + dx * 2;
                    var antinodeY = mainnode.y + dy * 2;
                    if (antinodeX >= 0 && antinodeX < width && antinodeY >= 0 && antinodeY < height)
                    {
                        antinodes[antinodeY, antinodeX] = true;
                    }
                }
            }
        }
        return antinodes;
    }
    
    private static bool[,] CreateResonatingAntinodes(int width, int height, Dictionary<char, HashSet<(int x, int y)>> nodes)
    {
        var antinodes = new bool[height, width];
        
        foreach (var type in nodes)
        {
            if (type.Value.Count == 1) continue;
            foreach (var mainnode in type.Value)
            {
                antinodes[mainnode.y, mainnode.x] = true;
                foreach (var subnode in type.Value)
                {
                    if (mainnode == subnode) continue;
                    
                    var dx = subnode.x - mainnode.x;
                    var dy = subnode.y - mainnode.y;

                    var factor = 2;
                    while (true)
                    {
                        var antinodeX = mainnode.x + dx * factor;
                        var antinodeY = mainnode.y + dy * factor;
                        if (antinodeX >= 0 && antinodeX < width && antinodeY >= 0 && antinodeY < height)
                        {
                            antinodes[antinodeY, antinodeX] = true;
                            factor++;
                            continue;
                        }
                        break;
                    }
                }
            }
        }
        return antinodes;
    }
}