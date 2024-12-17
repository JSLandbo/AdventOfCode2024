namespace AdventOfCode;

public sealed class Day01 : BaseDay
{
    private readonly string[] _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var input = _input.Select(e => e.Split("   "));
        var list1 = input.Select(e => int.Parse(e[0])).Order().ToArray();
        var list2 = input.Select(e => int.Parse(e[1])).Order().ToArray();
        var result = Enumerable.Range(0, input.Count()).Sum(i => Math.Abs(list1[i] - list2[i]));
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var input = _input.Select(e => e.Split("   "));
        var list1 = input.Select(e => int.Parse(e[0])).Order().ToArray();
        var list2 = input.Select(e => int.Parse(e[1])).Order().ToArray();
        var result = Enumerable.Range(0, input.Count()).Sum(i => list1[i] * list2.Count(e => e == list1[i]));
        return new ValueTask<string>($"{result}");
    }
}