using System.Drawing;

namespace Qr_Coder;

public static class MaskCodeDrawer
{
    public static void Draw(Bitmap image, int qrSize, Correction correction, int maskNumber)
    {
        var maskCode = MaskCodesPerCorrection[correction][maskNumber];
        
        image.SetLine(0, 8, 5, 8, maskCode.Substring(0, 6).ToBoolArray());
        image.SetFilledRectangle(7, 8, 1, maskCode[6] == '1' ? Color.Black : Color.White);
        image.SetFilledRectangle(8, 8, 1, maskCode[7] == '1' ? Color.Black : Color.White);
        image.SetFilledRectangle(8, 7, 1, maskCode[8] == '1' ? Color.Black : Color.White);
        image.SetLine(8, 0, 8, 5, maskCode.Substring(9).ToBoolArray().Reverse().ToArray());

        image.SetLine(8, qrSize - 7, 8, qrSize, (maskCode.Substring(0, 7) + "1").ToBoolArray().Reverse().ToArray());
        image.SetLine(qrSize - 7, 8, qrSize, 8, maskCode.Substring(7).ToBoolArray());
    }

    private static Dictionary<Correction, string[]> MaskCodesPerCorrection = new()
    {
        {
            Correction.L,
            new[] {"111011111000100", "111001011110011", "111110110101010", "111100010011101", "110011000101111"}
        },
        {
            Correction.M,
            new[] {"101010000010010", "101000100100101", "101111001111100", "101101101001011", "100010111111001"}
        },
        {
            Correction.Q,
            new[] {"011010101011111", "011000001101000", "011111100110001", "011101000000110", "010010010110100"}
        },
        {
            Correction.H,
            new[] {"001011010001001", "001001110111110", "001110011100111", "001100111010000", "000011101100010"}
        },
    };
}