using System.Drawing;
using System.Drawing.Imaging;

namespace Archivator;

public static class Jpeg
{
    public static void Compress(
        string imagePath, 
        string resultImagePath)
    {
        var bitmap = Image.FromFile(imagePath) as Bitmap;
        if (bitmap == null)
            throw new Exception($"Can not read image from path: {imagePath}");
        ToYCbCr(bitmap);
        //Sampling(bitmap, samplingSize);
        
        bitmap.Save(resultImagePath, ImageFormat.Jpeg);
    }

    private static void ToYCbCr(Bitmap bitmap)
    {
        for (var x = 0; x < bitmap.Width; x++)
        {
            for (var y = 0; y < bitmap.Height; y++)
            {
                Color pixelColor = bitmap.GetPixel(x, y);

                var Y = (byte)(0 + (0.299 * pixelColor.R) + (0.587 * pixelColor.G) + (0.114 * pixelColor.B));
                var Cb = (byte)(128 - (0.168736 * pixelColor.R) - (0.331264 * pixelColor.G) + (0.5 * pixelColor.B));
                var Cr = (byte)(128 + (0.5 * pixelColor.R) - (0.418688 * pixelColor.G) - (0.081312 * pixelColor.B));

                //bitmap.SetPixel(x, y, Color.From);
            }
        }
    }

    private static void Sampling(Bitmap bitmap, int samplingSize)
    {
        for (var x = 0; x < bitmap.Width - samplingSize; x += samplingSize)
        {
            for (var y = 0; y < bitmap.Height - samplingSize; y += samplingSize)
            {
                var rgbSum = new int[3];
                ExecActionForPixelRange(bitmap, x, y, samplingSize, (b, x, y) =>
                {
                    var pixel = b.GetPixel(x, y);
                    rgbSum[0] += pixel.R;
                    rgbSum[1] += pixel.G;
                    rgbSum[2] += pixel.B;
                });

                var rgbAvg = rgbSum.Select(e => (byte)(e / (samplingSize * samplingSize))).ToArray();
                ExecActionForPixelRange(bitmap, x, y, samplingSize, (b, x, y) =>
                {
                    b.SetPixel(x, y, Color.FromArgb(rgbAvg[0], rgbAvg[1], rgbAvg[2]));
                });
            }
        }
    }

    private static void ExecActionForPixelRange(
        Bitmap bitmap,
        int startX,
        int startY, 
        int range,
        Action<Bitmap, int, int> actionPerPixel)
    {
        for (var x = startX; x < startX + range; x++)
        {
            for (var y = startY; y < startY + range; y++)
            {
                actionPerPixel(bitmap, x, y);
            }
        }
    }
}