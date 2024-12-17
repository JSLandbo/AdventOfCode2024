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
        var groups = GetGroups(map);
        var results = groups.Select(group => {
            var area = GetArea(group);
            var circumference = GetCircumference(group.Select(e => (e.X, e.Y)));
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
        var results = GetGroups(map).Select(group => {
            var area = GetArea(group);
            var sides = GetSides(group.Select(e => (e.X, e.Y)).ToList());
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
    
    private static int GetSides(List<(int X, int Y)> group)
    {
        var left = new List<(int PosX, int PosY)>();
        var up = new List<(int PosX, int PosY)>();
        var right = new List<(int PosX, int PosY)>();
        var down = new List<(int PosX, int PosY)>();
    
        foreach (var pos in group)
        {
            if (!group.Contains((pos.X - 1, pos.Y))) right.Add((pos.X, pos.Y));  // Left check
            if (!group.Contains((pos.X + 1, pos.Y))) left.Add((pos.X, pos.Y));   // Right check
            if (!group.Contains((pos.X, pos.Y - 1))) down.Add((pos.X, pos.Y));   // Below check
            if (!group.Contains((pos.X, pos.Y + 1))) up.Add((pos.X, pos.Y));     // Above check
        }

        var leftiesCount = left
            .GroupBy(e => e.PosX)
            .Sum(gp => gp.Count(
                    pos1 => !gp.Contains((pos1.PosX, pos1.PosY - 1))
                )
            );
        var uppiesCount = up
            .GroupBy(e => e.PosY)
            .Sum(gp => gp.Count(
                    pos1 => !gp.Contains((pos1.PosX + 1, pos1.PosY))
                )
            );
        var downiesCount = down
            .GroupBy(e => e.PosY)
            .Sum(gp => gp.Count(
                    pos1 => !gp.Contains((pos1.PosX - 1, pos1.PosY))
                )
            );
        var rightiesCount = right
            .GroupBy(e => e.PosX)
            .Sum(
                gp => gp.Count(
                    pos1 => !gp.Contains((pos1.PosX, pos1.PosY + 1))
                )
            );
        
        return leftiesCount + uppiesCount + downiesCount + rightiesCount;
    }

    private static int GetCircumference(IEnumerable<(int X, int Y)> group)
    {
        var sum = 0;
        var positionSet = new HashSet<(int X, int Y)>(group);
        foreach (var pos in group)
        {
            var sides = 4;
            if (positionSet.Contains((pos.X - 1, pos.Y))) sides--;  // Left
            if (positionSet.Contains((pos.X + 1, pos.Y))) sides--;  // Right
            if (positionSet.Contains((pos.X, pos.Y - 1))) sides--;  // Down
            if (positionSet.Contains((pos.X, pos.Y + 1))) sides--;  // Up
            sum += sides;
        }
        return sum;
    }

    private static int GetArea(List<(int X, int Y, char C)> group) => group.Count;

    private static List<List<(int X, int Y, char C)>> GetGroups(char[][] map)
    {
        var groups = new List<List<(int X, int Y, char C)>>();

        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                var ch = map[j][i];
                if (ch == ' ') continue;
                var group = new List<(int X, int Y, char C)>();
                GetGroup(map, ch, i, j, group);
                groups.Add(group);
            }
        }

        return groups;
    }

    private static void GetGroup(char[][] map, char current, int x, int y, List<(int X, int Y, char C)> group)
    {
        if (current == ' ') return;
    
        group.Add((x, y, current));
    
        var left = x == 0;
        var top = y == 0;
        var bottom = y == map.Length - 1;
        var right = x == map[0].Length - 1;

        map[y][x] = ' ';

        if (!left && map[y][x - 1] == current) GetGroup(map, current, x - 1, y, group); // Left
        if (!right && map[y][x + 1] == current) GetGroup(map, current, x + 1, y, group); // Right
        if (!top && map[y - 1][x] == current) GetGroup(map, current, x, y - 1, group); // Top
        if (!bottom && map[y + 1][x] == current) GetGroup(map, current, x, y + 1, group); // Bottom
    }
}