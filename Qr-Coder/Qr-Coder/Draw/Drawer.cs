using System.Drawing;

namespace Qr_Coder;

public static class Drawer
{
    public const int BorderModules = 4;
    
    public static int PixelSizeModule;
    public static int BorderSize => BorderModules * PixelSizeModule;

    public static Bitmap Draw(bool[] bits, int version, Correction correction, int maskNumber)
    {
        var qrSize = LevelingPatternDrawer.GetQrSize(version);
        
        PixelSizeModule = 4;
        if (PixelSizeModule % 2 != 0)
            throw new Exception("Incorrect size of module. Should be %2 == 0");
        
        var imageSize = qrSize * PixelSizeModule // Внутренняя часть
            + BorderSize * 2; // Рамки
        var image = new Bitmap(imageSize, imageSize);
        image.Fill(Color.White);

        qrSize--; // с (0, 0)
        SearchingPatternDrawer.Draw(image, qrSize);
        SyncLinesDrawer.Draw(image, qrSize); 
        VersionCodeDrawer.Draw(image, version, qrSize);
        MaskCodeDrawer.Draw(image, qrSize, correction, maskNumber);
        LevelingPatternDrawer.Draw(image, qrSize);
        DataDrawer.Draw(image, qrSize, bits, maskNumber);

        return image;
    }
}