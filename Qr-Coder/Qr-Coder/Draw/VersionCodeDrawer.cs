using System.Drawing;

namespace Qr_Coder;

public static class VersionCodeDrawer
{
    public static void Draw(Bitmap image, int version, int qrSize)
    {
        var codes = GetVersionCode(version);
        if (codes == null)
            return;
        
        image.SetLine(0, qrSize - 10, 5, qrSize - 10, codes[0]);
        image.SetLine(0, qrSize - 9, 5, qrSize - 9, codes[1]);
        image.SetLine(0, qrSize - 8, 5, qrSize - 8, codes[2]);

        image.SetLine(qrSize - 10, 0, qrSize - 10, 5, codes[0]);
        image.SetLine(qrSize - 9, 0, qrSize - 9, 5, codes[1]);
        image.SetLine(qrSize - 8, 0, qrSize - 8, 5, codes[2]);
    }
    
     private static bool[][]? GetVersionCode(int version)
     {
         if (!VersionCodes.ContainsKey(version))
             return null;

         var codes = VersionCodes[version];
         return codes.Split().Select(e => e.ToBoolArray()).ToArray();
     }
    
    private static Dictionary<int, string> VersionCodes = new()
    {
        {7, "000010 011110 100110"},
        {8, "010001 011100 111000"},
        {9, "110111 011000 000100"},
        {10, "101001 111110 000000"},
        {11, "001111 111010 111100"},
        {12, "001101 100100 011010"},
        {13, "101011 100000 100110"},
        {14, "110101 000110 100010"},
        {15, "010011 000010 011110"},
        {16, "011100 010001 011100"},
        {17, "111010 010101 100000"},
        {18, "100100 110011 100100"},
        {19, "000010 110111 011000"},
        {20, "000000 101001 111110"},
        {21, "100110 101101 000010"},
        {22, "111000 001011 000110"},
        {23, "011110 001111 111010"},
        {24, "001101 001101 100100"},
        {25, "101011 001001 011000"},
        {26, "110101 101111 011100"},
        {27, "010011 101011 100000"},
        {28, "010001 110101 000110"},
        {29, "110111 110001 111010"},
        {30, "101001 010111 111110"},
        {31, "001111 010011 000010"},
        {32, "101000 011000 101101"},
        {33, "001110 011100 010001"},
        {34, "010000 111010 010101"},
        {35, "110110 111110 101001"},
        {36, "110100 100000 001111"},
        {37, "010010 100100 110011"},
        {38, "001100 000010 110111"},
        {39, "101010 000110 001011"},
        {40, "111001 000100 010101"},
    };
}