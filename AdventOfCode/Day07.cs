namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly string[] _input;

    public Day07()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var rows = _input.Select(e => (Result: long.Parse(e.Split(": ")[0]), Values: e.Split(": ")[1].Split(' ').Select(long.Parse).ToArray()));
        var total = rows.Sum(row => CheckNumberOne(row.Result, row.Values));
        
        return new ValueTask<string>($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        var rows = _input.Select(e => (Result: long.Parse(e.Split(": ")[0]), Values: e.Split(": ")[1].Split(' ').Select(long.Parse).ToArray()));
        var total = rows.Sum(row => CheckNumberTwo(row.Result, row.Values));
        
        return new ValueTask<string>($"{total}");
    }

    // Helper methods
    
    private static long CheckNumberOne(long target, long[] numbers)
    {
        if (numbers.Length == 1) return target == numbers[0] ? target : 0;
    
        var newList = (long[])numbers.Clone();
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
    
        var newList = (long[])numbers.Clone();
        var num1 = newList[0];
        var num2 = newList[1];
    
        newList[1] = num1 + num2;
        var result1 = CheckNumberTwo(target, newList[1..]);
        if (result1 != 0) return result1;
    
        newList[1] = num1 * num2;
        var result2 = CheckNumberTwo(target, newList[1..]);
        if (result2 != 0) return result2;
    
        newList[1] = long.Parse($"{num1}{num2}");
        var result3 = CheckNumberTwo(target, newList[1..]);
        return result3 != 0 ? result3 : 0;
    }
}