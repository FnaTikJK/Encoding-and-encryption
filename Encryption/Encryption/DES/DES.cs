namespace Encryption.DES;

public static class DES
{
    public static byte[] Encrypt(byte[] data, byte[] key)
    {
        KeyGenerator.Generate(key);
        var result = data
            .ChunkWithAddition(8)
            .Select(chunk => chunk.ToBoolArray())
            .Select(block => block.Shuffle(TShuffle))
            .Select(FeistelTransformer.Transform)
            .SelectMany(block => block.Shuffle(ReverseTShuffle))
            .Chunk(8)
            .Select(e => e.ToByte())
            .ToArray();

        return result;
    }

    public static byte[] Decrypt(byte[] data, byte[] key)
    {
        KeyGenerator.Generate(key);
        var result = data
            .ChunkWithAddition(8)
            .Select(chunk => chunk.ToBoolArray())
            .Select(block => block.Shuffle(TShuffle))
            .Select(FeistelTransformer.ReverseTransform)
            .SelectMany(block => block.Shuffle(ReverseTShuffle))
            .Chunk(8)
            .Select(e => e.ToByte())
            .ToArray();

        var mockZerosStartInd = GetSizeWithoutMockedZeros(result);
        return result.Take(mockZerosStartInd).ToArray();
    }

    private static readonly int[] TShuffle =
    {
        58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
        62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
        57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7,
    };

    private static readonly int[] ReverseTShuffle =
    {
        40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31,
        38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29,
        36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27,
        34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25,
    };

    private static int GetSizeWithoutMockedZeros(byte[] decrypted)
    {
        var length = decrypted.Length;
        var toTake = length;
        for (var i = length - 1; i >= Math.Max(0, length - 8); i--)
        {
            if (decrypted[i] == 0)
                toTake = i;
        }

        return toTake;
    }
}