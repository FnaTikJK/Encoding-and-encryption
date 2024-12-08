using System.Drawing;

namespace Qr_Coder;

public static class Drawer
{
    public static void Draw(byte[] data, int version)
    {
        var levelingPositions = LevelingPatternsPositionsByVersion[version];
        var qrSize = levelingPositions != null
            ? levelingPositions.Last() + 7
            : 21;
        var versionCode = GetVersionCode(version);
        var moduleSizeInPixels = 1;
        var imageSize = (qrSize + 4 + 4) * moduleSizeInPixels;
        var image = new Bitmap(imageSize, imageSize);
        SearchingPatternDrawer.Draw(image, moduleSizeInPixels);
    }

    private static bool[][]? GetVersionCode(int version)
    {
        return VersionCodes.ContainsKey(version) 
            ? null 
            : VersionCodes[version ];
    }
    
    private static Dictionary<int, bool[][]> VersionCodes = new()
    {
        {7, "000010 011110 100110".Split().Select(e => e.ToBoolArray()).ToArray()},
        {8, "010001 011100 111000".Split().Select(e => e.ToBoolArray()).ToArray()},
        {9, "110111 011000 000100".Split().Select(e => e.ToBoolArray()).ToArray()},
        {10, "101001 111110 000000".Split().Select(e => e.ToBoolArray()).ToArray()},
        {11, "001111 111010 111100".Split().Select(e => e.ToBoolArray()).ToArray()},
        {12, "001101 100100 011010".Split().Select(e => e.ToBoolArray()).ToArray()},
        {13, "101011 100000 100110".Split().Select(e => e.ToBoolArray()).ToArray()},
        {14, "110101 000110 100010".Split().Select(e => e.ToBoolArray()).ToArray()},
        {15, "010011 000010 011110".Split().Select(e => e.ToBoolArray()).ToArray()},
        {16, "011100 010001 011100".Split().Select(e => e.ToBoolArray()).ToArray()},
        {17, "111010 010101 100000".Split().Select(e => e.ToBoolArray()).ToArray()},
        {18, "100100 110011 100100".Split().Select(e => e.ToBoolArray()).ToArray()},
        {19, "000010 110111 011000".Split().Select(e => e.ToBoolArray()).ToArray()},
        {20, "000000 101001 111110".Split().Select(e => e.ToBoolArray()).ToArray()},
        {21, "100110 101101 000010".Split().Select(e => e.ToBoolArray()).ToArray()},
        {22, "111000 001011 000110".Split().Select(e => e.ToBoolArray()).ToArray()},
        {23, "011110 001111 111010".Split().Select(e => e.ToBoolArray()).ToArray()},
        {24, "001101 001101 100100".Split().Select(e => e.ToBoolArray()).ToArray()},
        {25, "101011 001001 011000".Split().Select(e => e.ToBoolArray()).ToArray()},
        {26, "110101 101111 011100".Split().Select(e => e.ToBoolArray()).ToArray()},
        {27, "010011 101011 100000".Split().Select(e => e.ToBoolArray()).ToArray()},
        {28, "010001 110101 000110".Split().Select(e => e.ToBoolArray()).ToArray()},
        {29, "110111 110001 111010".Split().Select(e => e.ToBoolArray()).ToArray()},
        {30, "101001 010111 111110".Split().Select(e => e.ToBoolArray()).ToArray()},
        {31, "001111 010011 000010".Split().Select(e => e.ToBoolArray()).ToArray()},
        {32, "101000 011000 101101".Split().Select(e => e.ToBoolArray()).ToArray()},
        {33, "001110 011100 010001".Split().Select(e => e.ToBoolArray()).ToArray()},
        {34, "010000 111010 010101".Split().Select(e => e.ToBoolArray()).ToArray()},
        {35, "110110 111110 101001".Split().Select(e => e.ToBoolArray()).ToArray()},
        {36, "110100 100000 001111".Split().Select(e => e.ToBoolArray()).ToArray()},
        {37, "010010 100100 110011".Split().Select(e => e.ToBoolArray()).ToArray()},
        {38, "001100 000010 110111".Split().Select(e => e.ToBoolArray()).ToArray()},
        {39, "101010 000110 001011".Split().Select(e => e.ToBoolArray()).ToArray()},
        {40, "111001 000100 010101".Split().Select(e => e.ToBoolArray()).ToArray()},
    };

    private static Dictionary<int, int[]?> LevelingPatternsPositionsByVersion = new()
    {
        {1, null},
        {2, new[] {18}},
        {3, new[] {22}},
        {4, new[] {26}},
        {5, new[] {30}},
        {6, new[] {34}},
        {7, new[] {6, 22, 38}},
        {8, new[] {6, 24, 42}},
        {9, new[] {6, 26, 46}},
        {10, new[] {6, 28, 50}},
        {11, new[] {6, 30, 54}},
        {12, new[] {6, 32, 58}},
        {13, new[] {6, 34, 62}},
        {14, new[] {6, 26, 46, 66}},
        {15, new[] {6, 26, 48, 70}},
        {16, new[] {6, 26, 50, 74}},
        {17, new[] {6, 30, 54, 78}},
        {18, new[] {6, 30, 56, 82}},
        {19, new[] {6, 30, 58, 86}},
        {20, new[] {6, 34, 62, 90}},
        {21, new[] {6, 28, 50, 72, 94}},
        {22, new[] {6, 26, 50, 74, 98}},
        {23, new[] {6, 30, 54, 78, 102}},
        {24, new[] {6, 28, 54, 80, 106}},
        {25, new[] {6, 32, 58, 84, 110}},
        {26, new[] {6, 30, 58, 86, 114}},
        {27, new[] {6, 34, 62, 90, 118}},
        {28, new[] {6, 26, 50, 74, 98, 122}},
        {29, new[] {6, 30, 54, 78, 102, 126}},
        {30, new[] {6, 26, 52, 78, 104, 130}},
        {31, new[] {6, 30, 56, 82, 108, 134}},
        {32, new[] {6, 34, 60, 86, 112, 138}},
        {33, new[] {6, 30, 58, 86, 114, 142}},
        {34, new[] {6, 34, 62, 90, 118, 146}},
        {35, new[] {6, 30, 54, 78, 102, 126, 150}},
        {36, new[] {6, 24, 50, 76, 102, 128, 154}},
        {37, new[] {6, 28, 54, 80, 106, 132, 158}},
        {38, new[] {6, 32, 58, 84, 110, 136, 162}},
        {39, new[] {6, 26, 54, 82, 110, 138, 166}},
        {40, new[] {6, 30, 58, 86, 114, 142, 170}},
    };
}