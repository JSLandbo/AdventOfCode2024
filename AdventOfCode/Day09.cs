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
        var compressed = CompressSinglesBlock(row);
        var result = compressed.block.Take(compressed.count).Select((x,i) => (long)x * i).Sum();
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var row = BuildBlocks(_input).ToArray();
        var compressed = CompressChunkedBlock(row);
        var result = compressed.Select((number, index) => number < 1 ? 0 : (long)number * index).Sum();
        return new ValueTask<string>($"{result}");
    }
    
    // Helper methods

    private static (int count, int[] block) CompressSinglesBlock(int[] block)
    {
        var rightIndex = block.Length - 1;
        for (var i = 0; i < block.Length; i++)
        {
            if (block[i] != -1) continue;
            for (var j = rightIndex; j > i; j--)
            {
                if (block[j] == -1) continue;
                block[i] = block[j];
                block[j] = -1;
                rightIndex = j - 1;
                break;
            }
        }
        return (rightIndex + 1, block);
    }
    
    private static int[] CompressChunkedBlock(int[] block)
    {
        var currentIndex = block.Length - 1;
        while (true)
        {
            var numbersToMove = MoveLeftInArrayFindNextNumberSection(block, currentIndex);
            if (numbersToMove.count == 0) break;
            var freeSpaceIndex = MoveRightInArrayFindNextFreeSpace(block, numbersToMove.count, numbersToMove.maxIndex, -1);
            if (freeSpaceIndex != -1)
            {
                for (var i = 0; i < numbersToMove.count; i++)
                {
                    block[freeSpaceIndex + i] = block[numbersToMove.maxIndex + i];
                    block[numbersToMove.maxIndex + i] = -1;
                }
            }
            currentIndex = numbersToMove.maxIndex - 1;
            if (currentIndex == -1) break;
        }
        return block;
    }
    
    private static (int maxIndex, int count) MoveLeftInArrayFindNextNumberSection(int[] block, int maxIndex)
    {
        var count = 0;
        var num = -2;
        for (var i = maxIndex; i >= 0; i--)
        {
            if (num == -2)
            {
                if (block[i] == -1) continue;
                num = block[i];
                count++;
                continue;
            }
            if (block[i] == num) count++;
            if (block[i] != num) return (i + 1, count);
        }
        return (-1, 0);
    }
    
    private static int MoveRightInArrayFindNextFreeSpace(int[] block, int length, int maxIndex, int number)
    {
        var count = 0;
        for (var i = 0; i < maxIndex; i++)
        {
            if (block[i] == number)
            {
                count++;
                if (count == length) return i - length + 1;
            }
            else count = 0;
        }
        return -1;
    }
    
    private static List<int> BuildBlocks(ReadOnlySpan<char> input)
    {
        List<int> blocks = [];
        for (var i = 0; i < input.Length; i+= 2)
        {
            var n1 = input[i] - '0';
            for (var j = 0; j < n1; j++) blocks.Add(i / 2);
            if (i + 1 >= input.Length) break;
            var n2 = input[i + 1] - '0';
            for (var j = 0; j < n2; j++) blocks.Add(-1);
        }
        return blocks;
    }
}