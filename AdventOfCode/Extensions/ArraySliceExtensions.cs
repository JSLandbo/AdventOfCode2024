using System.Runtime.CompilerServices;

namespace AdventOfCode.Extensions;

public static class ArraySliceExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T[] SliceFast<T>(this T[] source, int startIndex) where T : unmanaged
    {
        var length = source.Length - startIndex;
        var result = new T[length];
        fixed (T* pSource = source)
        fixed (T* pDest = result)
        {
            long byteCount = length * sizeof(T);
            Buffer.MemoryCopy(pSource + startIndex, pDest, byteCount, byteCount);
        }
        return result;
    }
}