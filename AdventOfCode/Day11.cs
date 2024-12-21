using System.Numerics;

namespace AdventOfCode;

public sealed class Day11 : BaseDay
{
    private readonly string _input;

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var numbers = _input.Split(' ').Select(BigInteger.Parse).ToArray();
        var result = RunThroughNumbers(numbers, 25);
        return new ValueTask<string>($"{result}");
    }

    private static BigInteger RunThroughNumbers(BigInteger[] numbers, int steps)
    {
        BigInteger sum = 0;
        foreach (var number in numbers)
        {
            sum += RecursivelyRunThroughNumber(number, steps);
        }
        return sum;
    }

    private static readonly Dictionary<(BigInteger number, int steps), BigInteger> B4 = [];

    private static BigInteger RecursivelyRunThroughNumber(BigInteger number, int steps)
    {
        if (steps == 0) return 1;
        if (B4.ContainsKey((number, steps))) return B4[(number, steps)];
        BigInteger sum = 0;
        if (number == 0) sum += RecursivelyRunThroughNumber(1, steps - 1);
        else
        {
            var length = (int)BigInteger.Log10(number) + 1;
            if (length % 2 != 0) sum += RecursivelyRunThroughNumber(number * 2024, steps - 1);
            else
            {
                var half = length / 2;
                sum += RecursivelyRunThroughNumber(number / BigInteger.Pow(10, half), steps - 1) +
                       RecursivelyRunThroughNumber(number % BigInteger.Pow(10, half), steps - 1);
            }
        }
        B4[(number, steps)] = sum;
        return sum;
    }

    public override ValueTask<string> Solve_2()
    {
        var numbers = _input.Split(' ').Select(BigInteger.Parse).ToArray();
        var result = RunThroughNumbers(numbers, 75);
        return new ValueTask<string>($"{result}");
    }
}