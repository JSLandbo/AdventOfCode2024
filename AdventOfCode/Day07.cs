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
        var total = _input.AsParallel()
            .Select(e =>
            {
                var result = long.Parse(e.Split(": ")[0]);
                var values = e.Split(": ")[1].Split(' ').Select(long.Parse).ToArray();
                return CheckNumberTwo(result, values);
            })
            .Sum();
        return new ValueTask<string>($"{total}");
    }

    // Helper methods

    private static long CheckNumberOne(long target, long[] numbers)
    {
        if (numbers.Length == 1) return target == numbers[0] ? target : 0;

        var newList = new long[numbers.Length];
        for (var i = 0; i < numbers.Length; i++)
            newList[i] = numbers[i];

        var num1 = newList[0];
        var num2 = newList[1];

        newList[1] = num1 + num2;
        var result1 = CheckNumberOne(target, newList[1..]);
        if (result1 != 0) return result1;

        newList[1] = num1 * num2;
        var result2 = CheckNumberOne(target, newList[1..]);
        return result2 != 0 ? result2 : 0;
    }

    private static long CheckNumberTwo(long target, long[] numbers)
    {
        if (numbers.Length == 1) return target == numbers[0] ? target : 0;

        var newList = new long[numbers.Length];
        for (var i = 0; i < numbers.Length; i++)
            newList[i] = numbers[i];

        var num1 = newList[0];
        var num2 = newList[1];

        newList[1] = num1 + num2;
        var result1 = CheckNumberTwo(target, newList[1..]);
        if (result1 != 0) return result1;

        newList[1] = num1 * num2;
        var result2 = CheckNumberTwo(target, newList[1..]);
        if (result2 != 0) return result2;

        // "Math.Pow(10, (long)Math.Log10(num2) + 1" = length of num2.
        // 145 & 2104: 145 * 10^4 = 1450000. 1450000 + 2104 = 1452104
        newList[1] = (long)(num1 * Math.Pow(10, (short)Math.Log10(num2) + 1) + num2);
        var result3 = CheckNumberTwo(target, newList[1..]);
        return result3 != 0 ? result3 : 0;
    }
}