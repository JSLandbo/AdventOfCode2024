namespace AdventOfCode;

public sealed class Day14 : BaseDay
{
    private readonly string[] _input;

    public Day14()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var robots = GetRobots(_input);
        const int width = 101;
        const int height = 103;
        const int steps = 100;
        var moved = MoveRobotsNewPositions(robots, width, height, steps);
        const int halfWidth = width / 2;
        const int halfHeight = height / 2;
        var topLeftQuadrantSum = moved.Where(e => e.Key is { x: < halfWidth, y: < halfHeight }).Sum(e => e.Value);
        var topRightQuadrantSum = moved.Where(e => e.Key is { x: > halfWidth, y: < halfHeight }).Sum(e => e.Value);
        var bottomLeftQuadrantSum = moved.Where(e => e.Key is { x: < halfWidth, y: > halfHeight }).Sum(e => e.Value);
        var bottomRightQuadrantSum = moved.Where(e => e.Key is { x: > halfWidth, y: > halfHeight }).Sum(e => e.Value);
        return new ValueTask<string>($"{topLeftQuadrantSum * topRightQuadrantSum * bottomLeftQuadrantSum * bottomRightQuadrantSum}");
    }

    public override ValueTask<string> Solve_2()
    {
        var robots = GetRobots(_input);
        const int width = 101;
        const int height = 103;
        var steps = 0;
        while (!MoveRobotsNoDuplicatePositions(robots, width, height, ++steps));
        return new ValueTask<string>($"{steps}");
    }
    
    // Helper methods

    private static bool MoveRobotsNoDuplicatePositions(((int x, int y) position, (int x, int y) velocity)[] robots, int width, int height, int steps)
    {
        var result = new bool[width, height];
        foreach (var robot in robots)
        {
            var x = (robot.position.x + robot.velocity.x * steps) % width;
            var y = (robot.position.y + robot.velocity.y * steps) % height;
            if (x < 0) x += width; // If minus coordinates, move into map
            if (y < 0) y += height; // If minus coordinates, move into map
            if (result[x, y]) return false;
            result[x, y] = true;
        }
        return true;
    }
    
    private static Dictionary<(int x, int y), int> MoveRobotsNewPositions(((int x, int y) position, (int x, int y) velocity)[] robots, int width, int height, int steps)
    {
        var result = new Dictionary<(int x, int y), int>();
        foreach (var robot in robots)
        {
            var x = (robot.position.x + robot.velocity.x * steps) % width;
            var y = (robot.position.y + robot.velocity.y * steps) % height;
            if (x < 0) x += width; // If minus coordinates, move into map
            if (y < 0) y += height; // If minus coordinates, move into map
            if (!result.TryAdd((x, y), 1)) result[(x, y)]++;
        }
        return result;
    }

    private static ((int x, int y) position, (int x, int y) velocity)[] GetRobots(string[] input)
    {
        return (from line in input
            select line.Split(' ')
            into pv
            let p = pv[0][2..].Split(',')
            let v = pv[1][2..].Split(',')
            select (
                (int.Parse(p[0]), int.Parse(p[1])), 
                (int.Parse(v[0]), int.Parse(v[1]))
            )).ToArray();
    }
}