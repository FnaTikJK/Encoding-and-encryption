namespace Qr_Coder;

public static class WorkingFieldsDeterminer
{
    public static bool[] GetWorkingFields(int textLength, int version)
    {
        var method = "0010";
        var encodingBitsLength = GetCountFieldLength(version);
        var dataCountField = Convert.ToString(textLength, 2).PadLeft(encodingBitsLength, '0');
        return (method + dataCountField).ToBoolArray();
    }

    private static int GetCountFieldLength(int version)
    {
        if (version <= 9)
            return 9;
        if (version <= 26)
            return 11;

        return 13;
    }
}