using System.Drawing;

namespace Qr_Coder;

public static class Extensions
{
    public static bool[] ToBoolArray(this string str)
    {
        return str.Select(e =>
        {
            if (e != '1' && e != '0')
                throw new Exception("Can not parse to bool array");
            return e == '1';
        }).ToArray();
    }

    public static bool[] ToBoolArray(this byte[] bytes)
    {
        return bytes
            .SelectMany(e => Convert.ToString(e, 2).PadLeft(8, '0').ToBoolArray())
            .ToArray();
    }

    public static string ToStrByte(this bool[] bits)
    {
        if (bits.Length != 8)
            throw new Exception($"Incorrect bits count. Count: {bits.Length}");

        return string.Join("", bits.Select(e => e ? "1" : "0"));
    }
    
    public static byte[] ToByteArray(this bool[] bits)
    {
        if (bits.Length % 8 != 0)
            throw new Exception($"Incorrect bits count. Count: {bits.Length}");

        return bits
            .Chunk(8)
            .Select(e => Convert.ToByte(e.ToStrByte(), 2))
            .ToArray();
    }

    public static void MoveValuesLeft(this byte[] arr, int stepLength = 1)
    {
        for (int i = 0; i < arr.Length - stepLength; i++)
            arr[i] = arr[i + stepLength];
        for (int i = arr.Length - stepLength; i < arr.Length; i++)
            arr[i] = 0;
    }

    public static void Xor(this bool[] source, bool[] target)
    {
        for (int i = 0; i < target.Length; i++)
            source[i] ^= target[i];
    }

    public static IEnumerable<byte> ByInternalIndexesOrder(this byte[][] blocks)
    {
        for (int i = 0; i < int.MaxValue; i++)
        {
            var shouldBreak = true;
            foreach (var block in blocks)
            {
                if (i < block.Length)
                {
                    shouldBreak = false;
                    yield return block[i];
                }
            }

            if (shouldBreak)
                yield break;
        }
    }

    public static void SetRectangle(this Bitmap image, int x, int y, int borderSize, Color color)
    {
        var delta = borderSize / 2;
        for (var i = x - delta; i <= x + delta; i++)
        {
            image.SetPixel(i, y - delta, color);
            image.SetPixel(i, y + delta, color);
        }
        for (var i = y - delta; i <= y + delta; i++)
        {
            image.SetPixel(x - delta, i, color);
            image.SetPixel(x + delta, i, color);
        }
    }

    public static void SetFilledRectangle(this Bitmap image, int x, int y, int borderSize, Color color)
    {
        var delta = borderSize / 2;
        for (var i = x - delta; i <= x + delta; i++)
        {
            for (var j = y - delta; j <= y + delta; j++)
            {
                image.SetPixel(i, j, color);
            }
        }
    }
}