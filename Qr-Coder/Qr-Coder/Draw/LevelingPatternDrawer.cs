using System.Drawing;

namespace Qr_Coder;

public static class LevelingPatternDrawer
{
    private static int[]? levelingPositions;
    
    public static int GetQrSize(int version)
    {
        levelingPositions = LevelingPatternsPositionsByVersion[version];
        var qrSize = levelingPositions != null
            ? levelingPositions.Last() + 7
            : 21;

        return qrSize;
    }

    public static void Draw(Bitmap image, int qrSize)
    {
        if (levelingPositions == null)
            return;

        foreach (var xPosition in levelingPositions)
        {
            foreach (var yPosition in levelingPositions)
            {
                if (NeedSkipLevel(xPosition, yPosition, qrSize))
                    continue;

                image.SetFilledRectangle(xPosition, yPosition, 1, Color.Black);
                image.SetRectangle(xPosition - 1, yPosition - 1, 3, Color.White);
                image.SetRectangle(xPosition - 2, yPosition - 2, 5, Color.Black);
            }
        }
    }

    private static bool NeedSkipLevel(int x, int y, int qrSize)
    {
        return x < 12 && (y < 12 || y > qrSize - 12)
               || x > qrSize - 12 && y < 12;
    }
    
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