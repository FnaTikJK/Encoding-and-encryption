using System.Drawing;

namespace Qr_Coder;

public static class DataDrawer
{
    public static void Draw(Bitmap image, int qrSize, bool[] bits)
    {
        var ind = 0;
        foreach (var position in GetPositions(qrSize))
        {
            if (bits[ind])
                image.SetFilledRectangle(position.x, position.y, 1, Color.Black);

            ind++;
        }

        // if (ind != bits.Length - 1)
        //     throw new Exception("Incorrect DataDrawing");
    }

    private static IEnumerable<(int x, int y)> GetPositions(int qrSize)
    {
        var x = qrSize;
        var y = qrSize;
        var direction = true; // up
        var stepsFromMoveLeft = 0;
        while (true)
        {
            if (x < 0)
                yield break;
            if (ShouldSkip(x, y, qrSize, out var withMoveLeft))
            {
                if (withMoveLeft)
                    x--;
                else 
                    y += direction ? -1 : 1;
                continue;
            }

            yield return (x, y);
            x--;
            yield return (x, y);

            if (ShouldMoveLeft(x, y, qrSize) && stepsFromMoveLeft > 1)
            {
                direction = !direction;
                x--;
                stepsFromMoveLeft = 0;
            }
            else
            {
                stepsFromMoveLeft++;
                x++;
                if (direction)
                    y--;
                else
                    y++;
            }
        }
    }

    private static bool ShouldMoveLeft(int x, int y, int qrSize)
    {
        return y == 0
               || y == qrSize
               || y == 9 && (x < 9 || x > qrSize - 9)
               || y == qrSize - 8 && x < 8
               || x == 8 && (y < 9 || x > qrSize - 9);
    }

    private static bool ShouldSkip(int x, int y, int qrSize, out bool withMoveLeft)
    {
        withMoveLeft = x == 6;

        return x == 6 || y == 6 // sync lines
            || x < 9 && y > qrSize - 8;
    }
}