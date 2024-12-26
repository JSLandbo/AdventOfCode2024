namespace AdventOfCode;

public sealed class Day07 : BaseDay
{
    private readonly string[] _input;

    public Day07()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        long total = 0;
        _input.AsParallel().ForAll(e =>
        {
            var result = long.Parse(e.Split(": ")[0]);
            var values = e.Split(": ")[1].Split(' ').Select(long.Parse).ToArray();
            var valueToAdd = CheckNumberOne(result, values);
            Interlocked.Add(ref total, valueToAdd);
        });
        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        long total = 0;
        _input.AsParallel().ForAll(e =>
        {
            var result = long.Parse(e.Split(": ")[0]);
            var values = e.Split(": ")[1].Split(' ').Select(long.Parse).ToArray();
            var valueToAdd = CheckNumberTwo(result, values);
            Interlocked.Add(ref total, valueToAdd);
        });
        return new ValueTask<string>($"{total}");
    }

    // Helper methods

    private static long CheckNumberOne(long target, long[] numbers)
    {
        if (numbers.Length == 1) return target == numbers[0] ? target : 0;

        var num1 = numbers[0];
        var num2 = numbers[1];

        numbers[1] = num1 + num2;
        var result1 = CheckNumberOne(target, numbers[1..]);
        if (result1 != 0) return result1;

        numbers[1] = num1 * num2;
        var result2 = CheckNumberOne(target, numbers[1..]);
        return result2 != 0 ? result2 : 0;
    }

    private static long CheckNumberTwo(long target, long[] numbers)
    {
        if (numbers.Length == 1) return target == numbers[0] ? target : 0;

        var num1 = numbers[0];
        var num2 = numbers[1];

        numbers[1] = num1 + num2;
        var result1 = CheckNumberTwo(target, numbers[1..]);
        if (result1 != 0) return result1;

        numbers[1] = num1 * num2;
        var result2 = CheckNumberTwo(target, numbers[1..]);
        if (result2 != 0) return result2;

        numbers[1] = (long)(num1 * Math.Pow(10, (short)Math.Log10(num2) + 1) + num2);
        var result3 = CheckNumberTwo(target, numbers[1..]);
        return result3;
    }
}