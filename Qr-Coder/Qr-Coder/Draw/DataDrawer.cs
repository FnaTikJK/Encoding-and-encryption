using System.Drawing;

namespace Qr_Coder;

public static class DataDrawer
{
    public static void Draw(Bitmap image, int qrSize, bool[] bits, int maskNumber)
    {
        levelingPositions = LevelingPatternDrawer.GetPositions(qrSize).ToArray();
        var ind = 0;
        foreach (var position in GetPositions(qrSize))
        {
            var value = ind < bits.Length ? bits[ind++] : false;
            if (Masked(value, position.x, position.y, maskNumber))
                image.SetFilledRectangle(position.x, position.y, 1, Color.Black);

        }

        if (ind != bits.Length)
            Console.WriteLine("Not all data was written in QR!");
    }

    private static bool Masked(bool value, int x, int y, int maskNumber)
    {
        if (masks[maskNumber](x, y))
            return !value;
        return value;
    }

    private static Func<int, int, bool>[] masks = {
        (x, y) => (x+y) % 2 == 0,
        (x, y) => y % 2 == 0,
        (x, y) => x % 3 == 0,
        (x, y) => (x + y) % 3 == 0,
    };

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
        var isInsideLevelingPattern = levelingPositions.Any(position => position.x - 2 <= x && position.x + 2 >= x
            && position.y - 2 <= y && position.y + 2 >= y);
        
        withMoveLeft = x == 6;

        return x == 6 || y == 6 // sync lines
                      || x < 9 && y > qrSize - 8
                      || isInsideLevelingPattern;
    }

    private static (int x, int y)[] levelingPositions;
}