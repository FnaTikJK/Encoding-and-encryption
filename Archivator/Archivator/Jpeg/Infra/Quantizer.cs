namespace Archivator.Jpeg.Infra;

public static class Quantizer
{

    public static byte[,] Quantize(double[,] channelFreqs, int quality)
    {
        if (settedQuality != quality)
            SetQuality(quality);

        var result = new byte[channelFreqs.GetLength(0), channelFreqs.GetLength(1)];
        for (var y = 0; y < channelFreqs.GetLength(0); y++)
        {
            for (var x = 0; x < channelFreqs.GetLength(1); x++)
            {
                result[y, x] = (byte)(channelFreqs[y, x] / workingMatrix[y, x]);
            }
        }

        return result;
    }
    
    public static double[,] DeQuantize(byte[,] quantizedBytes, int quality)
    {
        if (settedQuality != quality)
            SetQuality(quality);
        
        var result = new double[quantizedBytes.GetLength(0), quantizedBytes.GetLength(1)];
        for (int y = 0; y < quantizedBytes.GetLength(0); y++)
        {
            for (int x = 0; x < quantizedBytes.GetLength(1); x++)
            {
                result[y, x] =
                    ((sbyte)quantizedBytes[y, x]) *
                    workingMatrix[y, x]; //NOTE cast to sbyte not to loose negative numbers
            }
        }

        return result;
    }


    private static void SetQuality(int quality)
    {
        if (quality < 1 || quality > 99)
            throw new ArgumentException("quality must be in [1,99] interval");

        var multiplier = quality < 50 
            ? 5000 / quality 
            : 200 - 2 * quality;
        for (var y = 0; y < workingMatrix.GetLength(0); y++)
        {
            for (var x = 0; x < workingMatrix.GetLength(1); x++)
            {
                workingMatrix[y, x] = (multiplier * DefaultMatrix[y, x] + 50) / 100;
            }
        }

        settedQuality = quality;
    }
    
    
    private static readonly int[,] DefaultMatrix = {
        { 16, 11, 10, 16, 24, 40, 51, 61 },
        { 12, 12, 14, 19, 26, 58, 60, 55 },
        { 14, 13, 16, 24, 40, 57, 69, 56 },
        { 14, 17, 22, 29, 51, 87, 80, 62 },
        { 18, 22, 37, 56, 68, 109, 103, 77 },
        { 24, 35, 55, 64, 81, 104, 113, 92 },
        { 49, 64, 78, 87, 103, 121, 120, 101 },
        { 72, 92, 95, 98, 112, 100, 103, 99 }
    };

    private static int settedQuality = 0;
    private static int[,] workingMatrix = new int[DefaultMatrix.GetLength(0), DefaultMatrix.GetLength(1)];
}