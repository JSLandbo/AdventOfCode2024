namespace AdventOfCode;

public sealed class Day15 : BaseDay
{
    private readonly string[] _input;

    public Day15()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var info = GetInfo(_input);
        
        return default;
    }

    public override ValueTask<string> Solve_2()
    {
        var info = GetInfo(_input);
        
        return default;
    }

    private static (char[,] map, (int x, int y)[] steps, (int x, int y) position) GetInfo(string[] input)
    {
        var height = Array.IndexOf(input, string.Empty);
        var width = input[0].Length;
        var position = (x: -1, y: -1);
        
        var map = new char[height, width];
        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                if ((map[y, x] = input[y][x]) == '@')
                    position = (x, y);

        var steps = new List<(int x, int y)>();
        for (var y = height + 1; y < input.Length; y++)
            for (var x = 0; x < input[y].Length; x++)
                steps.Add(GetStep(input[y][x]));
        
        return (map, steps.ToArray(), position);
    }
    
    private static (int x, int y) GetStep(char ch) => ch switch
    {
        '>' => (1, 0),
        '<' => (-1, 0),
        '^' => (0, -1),
        'v' => (0, 1),
        _ => (0, 0),
    };

}