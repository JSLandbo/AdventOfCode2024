using System.Runtime.CompilerServices;

namespace AdventOfCode.Extensions;

public static class ArrayExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T FastRefAt<T>(this T[] array, int index)
    {
        return ref Unsafe.Add(ref array[0], index);
    }
}