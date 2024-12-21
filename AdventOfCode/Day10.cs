namespace AdventOfCode;

public sealed class Day10 : BaseDay
{
    private readonly string[] _input;

    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var map = new char[_input.Length, _input[0].Length]; // Map of the area
        for (var i = 0; i < _input.Length; i++) // "i" is the row (Y)
            for (var j = 0; j < _input[i].Length; j++) // "j" is the column (X)
                map[i, j] = _input[i][j];

        return default;
    }

    public override ValueTask<string> Solve_2()
    {
        return default;
    }
}