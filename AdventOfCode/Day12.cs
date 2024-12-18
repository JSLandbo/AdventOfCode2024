namespace AdventOfCode;

public sealed class Day12 : BaseDay
{
    private readonly string[] _input;

    public Day12()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var map = _input.Select(e => e.ToArray()).ToArray();
        var groups = GetFigures(map);
        var results = groups.Select(group => {
            var area = GetArea(group);
            var circumference = GetCircumference(group.Select(e => (e.X, e.Y)).ToArray());
            return new
            {
                Area = area,
                Circumference = circumference,
                Group = group,
                Price = area * circumference
            };
        }).ToList();
        
        return new ValueTask<string>($"{results.Sum(e => e.Price)}");
    }

    public override ValueTask<string> Solve_2()
    {
        var map = _input.Select(e => e.ToArray()).ToArray();
        var results = GetFigures(map).Select(group => {
            var area = GetArea(group);
            var sides = GetSides(group.Select(e => (e.X, e.Y)).ToHashSet());
            return new 
            {
                Area = area,
                Sides = sides,
                Group = group,
                SidesPrice = area * sides
            };
        }).ToList();

        return new ValueTask<string>($"{results.Sum(e => e.SidesPrice)}");
    }

    // Helper methods
    
    private static int GetSides(HashSet<(int X, int Y)> figure)
    {
        var left = new HashSet<(int PosX, int PosY)>();
        var up = new HashSet<(int PosX, int PosY)>();
        var right = new HashSet<(int PosX, int PosY)>();
        var down = new HashSet<(int PosX, int PosY)>();
    
        foreach (var pos in figure)
        {
            if (!figure.Contains((pos.X - 1, pos.Y))) right.Add((pos.X, pos.Y));  // Left check
            if (!figure.Contains((pos.X + 1, pos.Y))) left.Add((pos.X, pos.Y));   // Right check
            if (!figure.Contains((pos.X, pos.Y - 1))) down.Add((pos.X, pos.Y));   // Below check
            if (!figure.Contains((pos.X, pos.Y + 1))) up.Add((pos.X, pos.Y));     // Above check
        }
        
        var leftiesCount = GetNonAdjacentWallsOnSpecificSide(left, (0, -1), true);
        var uppiesCount = GetNonAdjacentWallsOnSpecificSide(up, (1, 0), false);
        var downiesCount = GetNonAdjacentWallsOnSpecificSide(down, (-1, 0), false);
        var rightiesCount = GetNonAdjacentWallsOnSpecificSide(right, (0, 1), true);
        
        return leftiesCount + uppiesCount + downiesCount + rightiesCount;
    }

    private static int GetNonAdjacentWallsOnSpecificSide(HashSet<(int PosX, int PosY)> positions, (int XO, int YO) offset, bool horizontal)
    {
        return positions.Count(pos => !positions.Contains((pos.PosX + offset.XO, pos.PosY + offset.YO)));
    }
    
    private static int GetCircumference((int X, int Y)[] group)
    {
        var sum = 0;
        var figure = new HashSet<(int X, int Y)>(group);
        foreach (var pos in group)
        {
            var sides = 4;
            if (figure.Contains((pos.X - 1, pos.Y))) sides--;  // Left
            if (figure.Contains((pos.X + 1, pos.Y))) sides--;  // Right
            if (figure.Contains((pos.X, pos.Y - 1))) sides--;  // Down
            if (figure.Contains((pos.X, pos.Y + 1))) sides--;  // Up
            sum += sides;
        }
        return sum;
    }

    private static int GetArea(List<(int X, int Y, char C)> group) => group.Count;

    private static List<List<(int X, int Y, char C)>> GetFigures(char[][] map)
    {
        var figures = new List<List<(int X, int Y, char C)>>();
        var visited = new bool[map[0].Length, map.Length];
        for (var x = 0; x < map.Length; x++)
        {
            for (var y = 0; y < map[x].Length; y++)
            {
                var ch = map[y][x];
                if (visited[y,x]) continue;
                var figure = new List<(int X, int Y, char C)>();
                GetAdjacent(map, visited, ch, x, y, figure);
                figures.Add(figure);
            }
        }

        return figures;
    }

    private static void GetAdjacent(char[][] map, bool[,] visited, char current, int x, int y, List<(int X, int Y, char C)> figure)
    {
        if (visited[y,x]) return;
        figure.Add((x, y, current));
        visited[y, x] = true;
        if (x != 0 && map[y][x - 1] == current) GetAdjacent(map, visited, current, x - 1, y, figure); // Left, within width bounds
        if (x != map[0].Length - 1 && map[y][x + 1] == current) GetAdjacent(map, visited, current, x + 1, y, figure); // Right, within width bounds
        if (y != 0 && map[y - 1][x] == current) GetAdjacent(map, visited, current, x, y - 1, figure); // Top, within height bounds
        if (y != map.Length - 1 && map[y + 1][x] == current) GetAdjacent(map, visited, current, x, y + 1, figure); // Bottom, within height bounds
    }
}