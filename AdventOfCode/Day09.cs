using System.Text;

namespace AdventOfCode;

public sealed class Day09 : BaseDay
{
    private readonly string _input;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var row = BuildBlocks(_input).ToArray();
        var compressed = CompressSinglesBlocks(row);
        var result = compressed.block.Take(compressed.count).Select((x,i) => (long)x * i).Sum();
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var row = BuildBlocks(_input).ToArray();
        var compressed = CompressChunkedBlocks(row);
        var result = compressed.block.Take(compressed.count).Select((x,i) => (long)x * i).Sum();
        return new ValueTask<string>($"{result}");
    }

    private static (int count, int[] block) CompressSinglesBlocks(int[] blocks)
    {
        var rightIndex = blocks.Length - 1;
        for (var i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != -1) continue;
            for (var j = rightIndex; j > i; j--)
            {
                if (blocks[j] == -1) continue;
                blocks[i] = blocks[j];
                blocks[j] = -1;
                rightIndex = j - 1;
                break;
            }
        }

        return (rightIndex + 1, blocks);
    }
    private static (int count, int[] block) CompressChunkedBlocks(int[] blocks)
    {
        return (0,[]);
    }

    private static List<int> BuildBlocks(string input)
    {
        List<int> blocks = [];
        var line = input.ToArray();
        for (var i = 0; i < input.Length; i+= 2)
        {
            var n1 = line[i] - '0';
            for (var j = 0; j < n1; j++) blocks.Add(i / 2);
            if (i + 1 >= input.Length) break;
            var n2 = line[i + 1] - '0';
            for (var j = 0; j < n2; j++) blocks.Add(-1);
        }
        return blocks;
    }
}