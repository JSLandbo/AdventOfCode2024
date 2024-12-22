namespace AdventOfCode;

public sealed class Day08 : BaseDay
{
    private readonly string[] _input;

    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var map = _input.Select(e => e.ToArray()).ToArray();
        
        return default;
    }

    public override ValueTask<string> Solve_2()
    {
        var map = _input.Select(e => e.ToArray()).ToArray();
        
        return default;
    }
}