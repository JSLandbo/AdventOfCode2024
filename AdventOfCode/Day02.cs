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
        var input = _input.Select(e => e.Split(' ').Select(int.Parse).ToArray()).ToArray();
        const int maxChange = 3;
        var result = 0;
        
        foreach (var row in input)
        {
            var allIncreasingSafely = true;
            var allDecreasingSafely = true;
            for (var i = 1; i < row.Length; i++)
            {
                var curr = row[i];
                var prev = row[i - 1];
                var diff = Math.Abs(curr - prev);
                if (diff > maxChange)
                {
                    allIncreasingSafely = false;
                    allDecreasingSafely = false;
                    break;
                }
                if (allIncreasingSafely && prev >= curr) allIncreasingSafely = false;
                if (allDecreasingSafely && prev <= curr) allDecreasingSafely = false;
                if (!allIncreasingSafely && !allDecreasingSafely) break;
            }
            if (allIncreasingSafely || allDecreasingSafely) result++;
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