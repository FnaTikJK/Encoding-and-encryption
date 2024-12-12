namespace Qr_Coder;

public static class DataPrettier
{
    public static bool[] Pretty(bool[] encoded, bool[] workingFields, int needsBitsLength)
    {
        var result = new bool[needsBitsLength];
        
        workingFields.CopyTo(result, 0);
        encoded.CopyTo(result, workingFields.Length);
        var totalLength = workingFields.Length + encoded.Length;
        var zeros = new bool[8 - totalLength % 8];
        zeros.CopyTo(result, totalLength);
        totalLength += zeros.Length;
        var curByte = 0;
        while (totalLength != needsBitsLength)
        {
            bytesToFill[curByte % 2].CopyTo(result, totalLength);
            totalLength += 8;
            curByte++;
        }
        
        return result;
    }
    
    private static bool[][] bytesToFill = {"11101100".ToBoolArray(), "00010001".ToBoolArray()};
}