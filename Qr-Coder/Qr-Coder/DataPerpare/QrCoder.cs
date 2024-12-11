using System.Drawing;

namespace Qr_Coder;

public static class QrCoder
{
    public static Bitmap Encode(byte[] data, Correction correction)
    {
        var (bits, version) = DataPreparer.Prepare(data, correction);
        var image = Drawer.Draw(bits, version, correction);

        return image;
    }
}