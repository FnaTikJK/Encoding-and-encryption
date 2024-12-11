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

    public static bool[] ToBoolArray(this IEnumerable<byte> bytes)
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
        x = x * Drawer.PixelSizeModule + Drawer.BorderSize;
        y = y * Drawer.PixelSizeModule + Drawer.BorderSize;
        
        var borderSizeInPixels = borderSize * Drawer.PixelSizeModule - 1;
        var deltaWidth = Drawer.PixelSizeModule;
        for (var i = x; i <= x + borderSizeInPixels; i++)
        {
            for (var width = 0; width < deltaWidth; width++)
            {
                image.SetPixel(i, y + width, color);
                image.SetPixel(i, y + borderSizeInPixels - width, color);
            }
        }
        for (var i = y; i <= y + borderSizeInPixels; i++)
        {
            for (var width = 0; width < deltaWidth; width++)
            {
                image.SetPixel(x + width, i, color);
                image.SetPixel(x + borderSizeInPixels - width, i, color);
            }
        }
    }

    public static void SetFilledRectangle(this Bitmap image, int x, int y, int borderSize, Color color)
    {
        x = x * Drawer.PixelSizeModule + Drawer.BorderSize;
        y = y * Drawer.PixelSizeModule + Drawer.BorderSize;

        var borderSizeInPixels = borderSize * Drawer.PixelSizeModule;
        for (var i = x; i < x + borderSizeInPixels; i++)
        {
            for (var j = y; j < y + borderSizeInPixels; j++)
            {
                image.SetPixel(i, j, color);
            }
        }
    }

    public static void Fill(this Bitmap image, Color color)
    {
        for (var i = 0; i < image.Width; i++)
        for (var j = 0; j < image.Height; j++)
            image.SetPixel(i, j, color);
    }

    public static void SetLine(this Bitmap image, int xStart, int yStart, int xEnd, int yEnd, bool[] values, bool isRepeatable = false)
    {
        var ind = 0;
        for (var i = xStart; i <= xEnd; i++)
        {
            for (var j = yStart; j <= yEnd; j++)
            {
                if (values[ind])
                {
                    image.SetFilledRectangle(i, j, 1, Color.Black);
                }

                ind = isRepeatable
                    ? (ind + 1) % values.Length
                    : ind + 1;
            }
        }
    }
}