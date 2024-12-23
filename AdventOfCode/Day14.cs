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

        var moved = MoveRobots(robots, width, height, steps).ToArray();

        const int halfWidth = width / 2;
        const int halfHeight = height / 2;

        var topLeftQuadrant = moved.Count(e => e is { x: < halfWidth, y: < halfHeight });
        var topRightQuadrant = moved.Count(e => e is { x: > halfWidth, y: < halfHeight });
        var bottomLeftQuadrant = moved.Count(e => e is { x: < halfWidth, y: > halfHeight });
        var bottomRightQuadrant = moved.Count(e => e is { x: > halfWidth, y: > halfHeight });

        return new ValueTask<string>(
            $"{topLeftQuadrant * topRightQuadrant * bottomLeftQuadrant * bottomRightQuadrant}");
    }

    public override ValueTask<string> Solve_2()
    {
        var robots = GetRobots(_input);

        const int width = 101;
        const int height = 103;

        var steps = 0;
        while (true)
        {
            steps++;
            var moved = MoveRobots(robots, width, height, steps);
            if (!MoveRobots(robots, width, height, steps).Any(e => moved.Count(t => t == e) > 1)) break;
        }

        return new ValueTask<string>($"{steps}");
    }

    private static IEnumerable<(int x, int y)> MoveRobots(IEnumerable<((int x, int y) position, (int x, int y) velocity)> robots, int width, int height, int steps)
    {
        return robots.Select(
            t => (
                x: (t.position.x + (t.velocity.x * steps)) % width,
                y: (t.position.y + (t.velocity.y * steps)) % height
            )).Select(e =>
            {
                e.x = e.x < 0 ? e.x + width : e.x;
                e.y = e.y < 0 ? e.y + height : e.y;
                return e;
            });
    }

    private static ((int x, int y) position, (int x, int y) velocity)[] GetRobots(string[] input)
    {
        return (from line in input
            select line.Split(' ')
            into pv
            let p = pv[0][2..].Split(',')
            let v = pv[1][2..].Split(',')
            select ((int.Parse(p[0]), int.Parse(p[1])), (int.Parse(v[0]), int.Parse(v[1])))).ToArray();
    }
}