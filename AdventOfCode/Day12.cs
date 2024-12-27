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
        var results = GetFigures(map).AsParallel().Select(group => {
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
        var results = GetFigures(map).AsParallel().Select(group => {
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
            if (!figure.Contains(MoveLeft(pos))) left.Add(pos);
            if (!figure.Contains(MoveRight(pos))) right.Add(pos);
            if (!figure.Contains(MoveUp(pos))) up.Add(pos);
            if (!figure.Contains(MoveDown(pos))) down.Add(pos);
        }
        var leftWalls = GetNonAdjacentWalls(left, Dirs.up);
        var topWalls = GetNonAdjacentWalls(up, Dirs.right);
        var rightWalls = GetNonAdjacentWalls(right, Dirs.down);
        var bottomWalls = GetNonAdjacentWalls(down, Dirs.left);
        return leftWalls + topWalls + bottomWalls + rightWalls;
    }

    private static int GetNonAdjacentWalls(HashSet<(int PosX, int PosY)> positions, (int X, int Y) direction)
    {
        return positions.Count(pos => !positions.Contains((pos.PosX + direction.X, pos.PosY + direction.Y)));
    }
    
    private static int GetCircumference((int X, int Y)[] group)
    {
        var sum = 0;
        var figure = new HashSet<(int X, int Y)>(group);
        foreach (var pos in group)
        {
            var sides = 4;
            if (figure.Contains(MoveLeft(pos))) sides--; 
            if (figure.Contains(MoveRight(pos))) sides--;  
            if (figure.Contains(MoveDown(pos))) sides--;  
            if (figure.Contains(MoveUp(pos))) sides--;  
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
        if (LeftIsSame()) GetAdjacent(map, visited, current, x - 1, y, figure); // Left, within width bounds
        if (RightIsSame()) GetAdjacent(map, visited, current, x + 1, y, figure); // Right, within width bounds
        if (UpIsSame()) GetAdjacent(map, visited, current, x, y - 1, figure); // Top, within height bounds
        if (DownIsSame()) GetAdjacent(map, visited, current, x, y + 1, figure); // Bottom, within height bounds
        return;
        bool LeftIsSame() => x != 0 && map[y][x - 1] == current;
        bool RightIsSame() => x != map[0].Length - 1 && map[y][x + 1] == current;
        bool UpIsSame() => y != 0 && map[y - 1][x] == current;
        bool DownIsSame() => y != map.Length - 1 && map[y + 1][x] == current;
    }

    private static readonly ((int, int) up, (int, int) right, (int, int) down, (int, int) left) Dirs = ((0, -1), (1, 0), (0, 1), (-1, 0));
    private static (int X, int Y) MoveLeft((int X, int Y) pos) => (pos.X - 1, pos.Y);
    private static (int X, int Y) MoveUp((int X, int Y) pos) => (pos.X, pos.Y - 1);
    private static (int X, int Y) MoveRight((int X, int Y) pos) => (pos.X + 1, pos.Y);
    private static (int X, int Y) MoveDown((int X, int Y) pos) => (pos.X, pos.Y + 1);
}