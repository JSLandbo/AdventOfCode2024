namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string[] _input;

    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        Dictionary<int, List<int>> rules = [];
        var section = -1;
        for (var i = 0; i < _input.Length; i++)
        {
            // Blank line = after this row comes the values to check the rules on
            if (_input[i] == string.Empty)
            {
                section = i;
                break;
            }
            // main (must be before) | number (this value)
            var split = _input[i].Split('|'); // [50, 25]
            var main = int.Parse(split[0]); // 50
            var number = int.Parse(split[1]); // 25
            // main (50) must be before (25) in the row
            if (!rules.ContainsKey(main)) rules.Add(main, []);
            // Only add if new rule, if not (main = 50, number = 25) is in rule: {50,[.., 20, 25]}
            if (rules[main].All(e => e != number)) rules[main].Add(number);
        }
        var lines = _input.Where((_, i) => i > section).Select(line => line.Split(",").Select(int.Parse).ToArray()).ToArray();
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
                    if (rule.Any(e => e == lines[h][j])) ok = false;
                }
            }
            (ok ? correctLines : incorrectLines).Add(lines[h]);
        }
        
        return new ValueTask<string>($"{GetMediansSum(correctLines)}");
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<int, List<int>> rules = [];
        var section = -1;
        for (var i = 0; i < _input.Length; i++)
        {
            if (_input[i] == string.Empty)
            {
                section = i;
                break;
            }
            var split = _input[i].Split('|'); // [50, 25]
            var main = int.Parse(split[0]); // 50
            var number = int.Parse(split[1]); // 25
            if (!rules.ContainsKey(main)) rules.Add(main, []);
            if (rules[main].All(e => e != number)) rules[main].Add(number);
        }
        var lines = _input.Where((_, i) => i > section).Select(line => line.Split(",").Select(int.Parse).ToArray()).ToArray();
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
                    if (rule.Any(e => e == lines[h][j])) ok = false;
                }
            }
            (ok ? correctLines : incorrectLines).Add(lines[h]);
        }
        var correctedLines = incorrectLines.Select(t => SortNumbers(t, rules)).ToList();
        
        return new ValueTask<string>($"{GetMediansSum(correctedLines)}");
    }
    
    // Helper methods

    private static int[] SortNumbers(int[] numbers, Dictionary<int, List<int>> rules)
    {
        var inDegree = new Dictionary<int, int>();
        var graph = new Dictionary<int, List<int>>();

        foreach (var number in numbers)
        {
            inDegree[number] = 0;
            graph[number] = [];
        }

        foreach (var (before, afters) in rules)
        {
            if (!inDegree.ContainsKey(before)) continue;
            foreach (var after in afters.Where(after => inDegree.ContainsKey(after)))
            {
                graph[before].Add(after);
                inDegree[after]++;
            }
        }

        var queue = new Queue<int>();
        foreach (var number in inDegree.Where(e => e.Value == 0))
        {
            queue.Enqueue(number.Key);
        }

        var result = new List<int>();
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);
            foreach (var neighbor in graph[current])
            {
                inDegree[neighbor]--;
                if (inDegree[neighbor] == 0)
                {
                    queue.Enqueue(neighbor);
                }
            }
        }
        if (result.Count != numbers.Length) throw new InvalidOperationException("There is a cycle in the dependencies.");
        return result.ToArray();
    }

    private static int GetMediansSum(List<int[]> lines) => lines.Select(line => line.Length % 2 == 0 ? line[line.Length / 2] : line[(line.Length - 1) / 2]).Sum();
}
