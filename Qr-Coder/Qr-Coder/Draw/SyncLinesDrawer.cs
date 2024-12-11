using System.Drawing;

namespace Qr_Coder;

public static class SyncLinesDrawer
{
    public static void Draw(Bitmap image, int qrSize)
    {
        var values = new[] {true, false};
        image.SetLine(8, 6, qrSize - 8, 6, values, true);
        image.SetLine(6, 8, 6, qrSize - 8, values, true);
    }
}