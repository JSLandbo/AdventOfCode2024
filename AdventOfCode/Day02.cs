namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string[] _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var input = _input.Select(e => e.Split(' ').Select(int.Parse).ToList()).ToList();
        const int maxChange = 3;
        var result = (from t in input 
            let up = Enumerable.Range(1, t.Count - 1).All(idx => t[idx - 1] < t[idx] && Math.Abs(t[idx - 1] - t[idx]) <= maxChange) 
            let down = !up && Enumerable.Range(1, t.Count - 1).All(idx => t[idx - 1] > t[idx] && Math.Abs(t[idx - 1] - t[idx]) <= maxChange) 
            where up || down select true
        ).Count();
        
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var input = _input.Select(e => e.Split(' ').Select(int.Parse).ToList()).ToList();
        const int maxChange = 3;
        var result = (from ot in input
            let upOk = Enumerable.Range(0, ot.Count).Any(x =>
            {
                var nt = ot.Where((_, index) => index != x).ToList();
                return Enumerable.Range(1, nt.Count - 1).All(idx => nt[idx - 1] < nt[idx] && Math.Abs(nt[idx - 1] - nt[idx]) <= maxChange);
            })
            let downOk = !upOk && Enumerable.Range(0, ot.Count).Any(x =>
            {
                var nt = ot.Where((_, index) => index != x).ToList();
                return Enumerable.Range(1, nt.Count - 1).All(idx => nt[idx - 1] > nt[idx] && Math.Abs(nt[idx - 1] - nt[idx]) <= maxChange);
            })
            where upOk || downOk select true
        ).Count();

        return new ValueTask<string>($"{result}");
    }
}