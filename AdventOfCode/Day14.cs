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

        return default;
    }

    public override ValueTask<string> Solve_2()
    {
        var robots = GetRobots(_input);

        return default;
    }

    private static List<((int x, int y) position, (int x, int y) velocity)> GetRobots(string[] input)
    {
        return (from line in input select 
            line.Split(' ') into pv 
            let p = pv[0][2..].Split(',') 
            let v = pv[1][2..].Split(',') 
            select ((int.Parse(p[0]), int.Parse(p[1])), (int.Parse(v[0]), int.Parse(v[1])))
        ).ToList();
    }
}