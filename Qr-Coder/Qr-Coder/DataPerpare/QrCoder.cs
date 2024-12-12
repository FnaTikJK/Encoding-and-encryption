using System.Drawing;

namespace Qr_Coder;

public static class QrCoder
{
    public static Bitmap Encode(string text, Correction correction, int maskNumber)
    {
        var (bits, version) = DataPreparerV2.Prepare(text, correction);
        var image = Drawer.Draw(bits, version, correction, maskNumber);

        return image;
    }
}