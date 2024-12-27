using System.Text;
using System.Text.RegularExpressions;
namespace AdventOfCode;

public sealed class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var regex = new Regex(@"mul\((\d+),(\d+)\)");
        var input = regex.Matches(_input);
        var sum = input.Select(matchCollection => matchCollection).Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2()
    {
        var regex = new Regex(@"mul\((\d+),(\d+)\)");
        var dos = _input.FindAllIndexes("do()");
        var donts = _input.FindAllIndexes("don't()");
        var builder = new StringBuilder();
        var going = true;
        for (var i = 0; i < _input.Length; i++)
        {   // Only append if we are in the "do()" section
            if (dos.Contains(i)) going = true;
            if (donts.Contains(i)) going = false;
            if (going) builder.Append(_input[i]);
        }
        var input = regex.Matches(builder.ToString());
        var sum = input.Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
        return new ValueTask<string>($"{sum}");
    }
}

// Helper classes

internal static class StringExtensions
{
    public static List<int> FindAllIndexes(this string text, string searchText)
    {
        List<int> positions = [];
        var index = text.IndexOf(searchText, StringComparison.Ordinal);
        while (index != -1)
        {
            positions.Add(index);
            index = text.IndexOf(searchText, index + 1, StringComparison.Ordinal);
        }
        return positions;
    }
}