using System.Numerics;
using System.Text;

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
        var numbers = _input.Split(' ').Select(BigInteger.Parse).ToList();
        for (var i = 0; i < 25; i++)
            Blink(numbers);
        return new ValueTask<string>($"{numbers.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        var numbers = _input.Split(' ').Select(BigInteger.Parse).ToList();
        for (var i = 0; i < 75; i++)
            Blink(numbers);
        return new ValueTask<string>($"{numbers.Count}");
    }
    
    private static void Blink(List<BigInteger> input)
    {
        var result = new List<BigInteger>();
        foreach (var number in input)
        {
            RunThroughNumber(result, number);
        };
        input.Clear();
        input.AddRange(result);
    }
    
    private static void RunThroughNumber(List<BigInteger> result, BigInteger number)
    {
        if (number == 0)
        {
            result.Add(1);
            return;
        }
        
        var length = (long)BigInteger.Log10(number) + 1;
        if (length % 2 == 0)
        {
            var half = (int)length / 2;
            result.Add((long)(number / BigInteger.Pow(10, half)));
            result.Add((long)(number % BigInteger.Pow(10, half)));
        }
        else
        {
            result.Add(number * 2024);
        }
    }

}