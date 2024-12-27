namespace AdventOfCode;

public sealed class Day05 : BaseDay
{
    private readonly string[] _input;

    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var (section, rules) = GetRules(_input);
        var (correctLines, _) = GetLines(section, rules, _input);
        return new ValueTask<string>($"{GetMediansSum(correctLines)}");
    }

    public override ValueTask<string> Solve_2()
    {
        var (section, rules) = GetRules(_input);
        var (_, incorrectLines) = GetLines(section, rules, _input);
        var correctedLines = incorrectLines.Select(t => SortNumbers(t, rules)).ToList();
        return new ValueTask<string>($"{GetMediansSum(correctedLines)}");
    }
    
    // Helper methods

    private static (int section, Dictionary<int, List<int>>) GetRules(string[] input)
    {
        Dictionary<int, List<int>> rules = [];
        var section = -1;
        for (var i = 0; i < input.Length; i++)
        {
            // Blank line = after this row comes the values to check the rules on
            if (input[i] == string.Empty)
            {
                section = i;
                break;
            }
            // main (must be before) | number (this value)
            var split = input[i].Split('|'); // [50, 25]
            var main = int.Parse(split[0]); // 50
            var number = int.Parse(split[1]); // 25
            // main (50) must be before (25) in the row
            if (!rules.ContainsKey(main)) rules.Add(main, []);
            // Only add if new rule, if not (main = 50, number = 25) is in rule: {50,[.., 20, 25]}
            if (rules[main].All(e => e != number)) rules[main].Add(number);
        }
        return (section, rules);
    }
    
    private static (List<int[]> correctLines, List<int[]> incorrectLines) GetLines(int section, Dictionary<int, List<int>> rules, string[] input)
    {
        var lines = input.Where((_, i) => i > section).Select(line => line.Split(",").Select(int.Parse).ToArray()).ToArray();
        var correctLines = new List<int[]>();
        var incorrectLines = new List<int[]>();
        for (var h = lines.Length - 1; h >= 0; h--)
        {
            var ok = true;
            for (var i = lines[h].Length - 1; i >= 0 && ok; i--)
            {
                var main = lines[h][i];
                for (var j = i - 1; j >= 0 && ok; j--)
                {
                    if (!rules.TryGetValue(main, out var rule)) continue;
                    if (rule.Contains(lines[h][j])) ok = false;
                }
            }
            (ok ? correctLines : incorrectLines).Add(lines[h]);
        }
        return (correctLines, incorrectLines);
    }
    
    private static int[] SortNumbers(int[] numbers, Dictionary<int, List<int>> rules)
    {
        Array.Sort(numbers, (x, y) =>
        {
            if (rules.TryGetValue(x, out var numOne) && numOne.Contains(y))
            {   // x should come before y
                return -1;  
            }
            if (rules.TryGetValue(y, out var numTwo) && numTwo.Contains(x))
            {   // y should come before x
                return 1;  
            }
            return 0;
        });
        return numbers;
    }

    private static int GetMediansSum(List<int[]> lines) => lines.Select(line => line.Length % 2 == 0 ? line[line.Length / 2] : line[(line.Length - 1) / 2]).Sum();
}
