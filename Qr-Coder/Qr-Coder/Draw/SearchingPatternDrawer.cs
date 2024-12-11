using System.Drawing;

namespace Qr_Coder;

public static class SearchingPatternDrawer
{
    public static void Draw(Bitmap image, int qrSize)
    {
        foreach (var positions in GetCentredPositions(qrSize))
        {
            image.SetRectangle(positions.x - 3, positions.y - 3, 7, Color.Black);
            image.SetFilledRectangle(positions.x - 1, positions.y - 1, 3, Color.Black);
        }
    }

    private static IEnumerable<(int x, int y)> GetCentredPositions(int qrSize)
    {
        var delta = 3;
        yield return (delta, delta);
        yield return (qrSize - delta, delta);
        yield return (delta, qrSize - delta);
    }
}