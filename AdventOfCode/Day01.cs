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
        var input = _input.Select(e => e.Split("   ")).Select(parts => (int.Parse(parts[0]), int.Parse(parts[1]))).ToArray();
        var (left, right) = GetSortedArrays(input);
        var result = 0;
        for (var i = 0; i < input.Length; i++)
        {   // Calculate the absolute difference between the two sorted arrays
            result += Math.Abs(left[i] - right[i]);
        }
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var input = _input.Select(e => e.Split("   ")).Select(parts => (int.Parse(parts[0]), int.Parse(parts[1]))).ToArray();
        var (left, right) = GetSortedArrays(input);
        var count = new Dictionary<int, int>();
        for (var i = 0; i < input.Length; i++)
        {   // Count the amount of times a number appears in the right array
            count[right[i]] = count.GetValueOrDefault(right[i]) + 1;
        }
        var result = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (count.TryGetValue(left[i], out var value))
            {   // Multiply the left array value with the amount of times it appears in the right array
                result += left[i] * value;
            }
        }
        return new ValueTask<string>($"{result}");
    }
    
    private static (int[] left, int[] right) GetSortedArrays((int, int)[] input)
    {
        var left = new int[input.Length];
        var right = new int[input.Length];
        for (var i = 0; i < input.Length; i++)
        {
            left[i] = input[i].Item1;
            right[i] = input[i].Item2;
        }
        Array.Sort(left);
        Array.Sort(right);
        return (left, right);
    }
}