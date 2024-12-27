namespace AdventOfCode;

public sealed class Day02 : BaseDay
{
    private readonly string[] _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var input = _input.Select(e => e.Split(' ').Select(int.Parse).ToArray());
        const int maxChange = 3;
        var result = 0;
        foreach (var t in input)
        {   // Check chain for every number in array
            var allIncreasingSafely = Enumerable.Range(1, t.Length - 1).All(idx => t[idx - 1] < t[idx] && Math.Abs(t[idx - 1] - t[idx]) <= maxChange);
            var allDecreasingSafely = !allIncreasingSafely && Enumerable.Range(1, t.Length - 1).All(idx => t[idx - 1] > t[idx] && Math.Abs(t[idx - 1] - t[idx]) <= maxChange);
            if (!allIncreasingSafely && !allDecreasingSafely) continue;
            result++;
        }
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var input = _input.Select(e => e.Split(' ').Select(int.Parse).ToArray());
        const int maxChange = 3;
        var result = 0;
        foreach (var ot in input)
        {
            var allIncreasingSafely = Enumerable.Range(0, ot.Length).Any(x =>
            {   // Check chain for every number in array
                var nt = ot.Where((_, index) => index != x).ToArray(); // Skip this index, check all other numbers. We allow 1 faulty.
                return Enumerable.Range(1, nt.Length - 1).All(idx => nt[idx - 1] < nt[idx] && Math.Abs(nt[idx - 1] - nt[idx]) <= maxChange);
            });
            var allDecreasingSafely = !allIncreasingSafely && Enumerable.Range(0, ot.Length).Any(x =>
            {   // Check chain for every number in array
                var nt = ot.Where((_, index) => index != x).ToArray(); // Skip this index, check all other numbers. We allow 1 faulty.
                return Enumerable.Range(1, nt.Length - 1).All(idx => nt[idx - 1] > nt[idx] && Math.Abs(nt[idx - 1] - nt[idx]) <= maxChange);
            });
            if (!allIncreasingSafely && !allDecreasingSafely) continue;
            result++;
        }
        return new ValueTask<string>($"{result}");
    }
}