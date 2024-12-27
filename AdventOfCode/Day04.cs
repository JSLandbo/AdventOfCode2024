namespace AdventOfCode;

public sealed class Day04 : BaseDay
{
    private readonly string[] _input;

    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>($"{XmasOccurrences(_input).Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>($"{SumOfXmas(_input).Count()}");
    }

    // Helper methods
    
    private static IEnumerable<int> SumOfXmas(string[] content) => (
        from x in Enumerable.Range(1, content.First().Length - 2)
        from y in Enumerable.Range(1, content.Length - 2)
        where content[x][y] == 'A' &&
          ((content[x - 1][y - 1], content[x + 1][y + 1]) is ('M', 'S') ||
           (content[x - 1][y - 1], content[x + 1][y + 1]) is ('S', 'M')) &&
          ((content[x - 1][y + 1], content[x + 1][y - 1]) is ('M', 'S') ||
           (content[x - 1][y + 1], content[x + 1][y - 1]) is ('S', 'M'))
        select 1
    );

    private static IEnumerable<int> XmasOccurrences(string[] content)
    {
        var width = content.First().Length;
        var height = content.Length;
        foreach (var x in Enumerable.Range(0, width))
        {
            var xLimit = x + 3;
            foreach (var y in Enumerable.Range(0, height))
            {   // Check for XMAS in all directions
                if (content[x][y] != 'X') continue;
                var yLimit = y + 3;
                var count = 0;
                if (x > 2           && (content[x-1][y], content[x-2][y], content[x-3][y]) is ('M','A','S')) count += 1; // Left
                if (y > 2           && (content[x][y-1], content[x][y-2], content[x][y-3]) is ('M','A','S')) count += 1; // Up
                if (xLimit < width  && (content[x+1][y], content[x+2][y], content[x+3][y]) is ('M','A','S')) count += 1; // Right
                if (yLimit < height && (content[x][y+1], content[x][y+2], content[x][y+3]) is ('M','A','S')) count += 1; // Down
                if (x > 2          && y > 2 && (content[x-1][y-1], content[x-2][y-2], content[x-3][y-3]) is ('M','A','S')) count += 1; // Up left
                if (xLimit < width && y > 2 && (content[x+1][y-1], content[x+2][y-2], content[x+3][y-3]) is ('M','A','S')) count += 1; // Up right
                if (x > 2          && yLimit < height && (content[x-1][y+1], content[x-2][y+2], content[x-3][y+3]) is ('M','A','S')) count += 1; // Down left
                if (xLimit < width && yLimit < height && (content[x+1][y+1], content[x+2][y+2], content[x+3][y+3]) is ('M','A','S')) count += 1; // Down right
                yield return count;
            }
        }
    }
}