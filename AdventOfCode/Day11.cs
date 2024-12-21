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
        if (steps == 0) return 1; // Nothing more to do (nice) and I am a single block, return me!
        if (B4.ContainsKey((number, steps))) return B4[(number, steps)]; // Dont reiterate same number and steps..
        BigInteger sum = 0; // BigInteger 2^68685922272 (256MB memory) digits
        // 0 => 1, next step.
        if (number == 0) sum += RecursivelyRunThroughNumber(1, steps - 1);
        else
        {   // Uneven number, multiply by 2024 and continue 
            var length = (int)BigInteger.Log10(number) + 1; // Number of digits
            if (length % 2 != 0) sum += RecursivelyRunThroughNumber(number * 2024, steps - 1);
            else
            {   // Even number, split in half and continue
                var half = length / 2;
                sum += RecursivelyRunThroughNumber(number / BigInteger.Pow(10, half), steps - 1) + // First half
                       RecursivelyRunThroughNumber(number % BigInteger.Pow(10, half), steps - 1);  // Second half
            }
        }
        B4[(number, steps)] = sum; // We are going to return now, save this result in case we hit this number and step again
        return sum;
    }

    public override ValueTask<string> Solve_2()
    {
        var numbers = _input.Split(' ').Select(BigInteger.Parse).ToArray();
        var result = RunThroughNumbers(numbers, 75);
        return new ValueTask<string>($"{result}");
    }
}