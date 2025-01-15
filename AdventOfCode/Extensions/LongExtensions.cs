using System.Runtime.CompilerServices;

namespace AdventOfCode.Extensions;

public static class FastMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long PowInt(this long baseVal, long exp)
    {
        long result = 1;
        var power = baseVal;
        while (exp > 0)
        {
            if ((exp & 1) == 1) result *= power;
            power *= power;
            exp >>= 1;
        }

        return result;
    }

    private static readonly long[] SPow10 =
    [
        1L, 
        10L, 
        100L, 
        1_000L, 
        10_000L, 
        100_000L, 
        1_000_000L, 
        10_000_000L, 
        100_000_000L, 
        1_000_000_000L,
        10_000_000_000L, 
        100_000_000_000L, 
        1_000_000_000_000L, 
        10_000_000_000_000L, 
        100_000_000_000_000L,
        1_000_000_000_000_000L, 
        10_000_000_000_000_000L, 
        100_000_000_000_000_000L, 
        1_000_000_000_000_000_000L
    ];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ConcatDecimal(this long num1, long num2)
    {
        if (num2 == 0) return num1 * 10;
        var digits = CountDigits(num2);
        return num1 * SPow10[digits] + num2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountDigits(this long num)
    {
        return num switch
        {
            < 10L => 1,
            < 100L => 2,
            < 1_000L => 3,
            < 10_000L => 4,
            < 100_000L => 5,
            < 1_000_000L => 6,
            < 10_000_000L => 7,
            < 100_000_000L => 8,
            < 1_000_000_000L => 9,
            < 10_000_000_000L => 10,
            < 100_000_000_000L => 11,
            < 1_000_000_000_000L => 12,
            < 10_000_000_000_000L => 13,
            < 100_000_000_000_000L => 14,
            < 1_000_000_000_000_000L => 15,
            < 10_000_000_000_000_000L => 16,
            < 100_000_000_000_000_000L => 17,
            < 1_000_000_000_000_000_000L => 18,
            _ => 19
        };
    }
}