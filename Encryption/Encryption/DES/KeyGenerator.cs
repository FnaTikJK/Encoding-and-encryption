namespace Encryption.DES;

public static class KeyGenerator
{
    public static bool[][] Keys = new bool[16][];

    public static void Generate(byte[] initKey)
    {
        if (initKey.Length < 7)
            throw new Exception("Размер ключа должен быть минимум 7 байт (56 бит)");

        var keyBits = initKey.Take(7).ToBoolArray();
        var extendedTo64Bits = ExtendKey(keyBits);
        for (var i = 0; i < 16; i++)
            Keys[i] = GetKey(extendedTo64Bits, i);
    }

    private static bool[] ExtendKey(bool[] key56Bits)
    {
        var result = new bool[64];
        var bytesWritten = 0;
        var onesCount = 0;
        for (var i = 0; i < key56Bits.Length; i++)
        {
            if (i % 8 == 0)
            {
                result[i + bytesWritten] = onesCount % 2 == 0; // every byte contains not (% 2 == 0) One's count
                bytesWritten++;
                onesCount = 0;
            }
            
            result[i + bytesWritten] = key56Bits[i];
            if (result[i])
                onesCount++;
        }

        return result;
    }

    public static bool[] GetKey(bool[] key, int keyInd)
    {
        var shuffledByCD = new bool[56];
        var shift = IShifts[..keyInd].Sum();
        for (var i = 0; i < 28; i++)
        {
            var ind = i - shift;
            ind = ind >= 0 
                ? ind 
                : 28 + ind;
            shuffledByCD[i] = key[CShuffle28[ind]];
            shuffledByCD[i + 28] = key[DShuffle28[ind]];
        }

        return shuffledByCD.ShuffleWithResize(KeyShuffle48);
    }
    

    private static int[] IShifts =
    {
        1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
    };

    private static int[] CShuffle28 =
    {
        57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18,
        10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36,
    };

    private static int[] DShuffle28 =
    {
        63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22,
        14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4,
    };

    private static int[] KeyShuffle48 =
    {
        14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4,
        26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40,
        51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32,
    };
}