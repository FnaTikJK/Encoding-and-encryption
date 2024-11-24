namespace Encryption.DES;

public static class EnumerableExtensions
{
    public static IEnumerable<T[]> ChunkWithAddition<T>(this IEnumerable<T> source, int size)
    {
        return source
            .Chunk(size)
            .Select(chunk => chunk.Length == size
                ? chunk
                : chunk.Concat(Enumerable.Repeat(default(T), size - chunk.Length)).ToArray())!;
    }

    public static bool[] Shuffle(this bool[] source, int[] shuffleTable)
    {
        var result = new bool[source.Length];
        for (var i = 0; i < result.Length; i++)
            result[i] = source[shuffleTable[i] - 1];

        return result;
    }

    public static bool[] ShuffleWithResize(this bool[] source, int[] shuffletable)
    {
        var result = new bool[shuffletable.Length];
        for (var i = 0; i < result.Length; i++)
            result[i] = source[shuffletable[i] - 1];

        return result;
    }

    public static bool[] ToBoolArray(this IEnumerable<byte> bytes)
    {
        return bytes
            .SelectMany(e => Convert.ToString(e, 2).PadLeft(8, '0'))
            .Select(e => e == '1')
            .ToArray();
    }

    public static bool[] Xor(this bool[] a, bool[] b)
    {
        var result = new bool[a.Length];
        for (var i = 0; i < a.Length; i++)
            result[i] = a[i] ^ b[i];

        return result;
    }

    public static byte ToByte(this bool[] source)
    {
        if (source.Length > 8)
            throw new Exception("bools count is more then 8");

        return Convert.ToByte(string.Join("", source.Select(e => e ? '1' : '0')), 2);
    }

    public static bool[] ToBoolArray(this byte @byte, int arraySize)
    {
        var result = new bool[arraySize];
        var bits = Convert.ToString(@byte, 2).PadLeft(4, '0');
        for (var i = 0; i < bits.Length; i++)
            result[i] = bits[i] == '1';

        return result;
    }
}