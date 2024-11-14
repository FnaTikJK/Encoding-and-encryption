namespace Archivator.Jpeg.Infra;

public static class RLE
{
    public static byte[] Encode(List<byte> bytes)
    {
        var result = new List<byte>();
        byte zeros = 0;
        foreach (var curByte in bytes)
        {
            if (curByte != 0)
            {
                result.Add(zeros);
                result.Add(curByte);
                if (curByte == 255)
                    result.Add(0);
                zeros = 0;
            }
            else if(zeros == 255)
            {
                result.Add(zeros);
                result.Add(0);
                zeros = 1;
            }
            else
            {
                zeros++;
            }
        }
        if (zeros != 0)
            result.Add(zeros);

        return result.ToArray();
    }

    public static byte[] Decode(byte[] encoded)
    {
        var result = new List<byte>();
        var isZero = true;
        var isLast255 = false;
        foreach (var count in encoded)
        {
            if (count == 0 && isLast255)
            {
                isLast255 = false;
                continue;
            }

            isLast255 = count == 255;
            
            if (isZero)
                result.AddRange(Enumerable.Repeat((byte)0, count));
            else
                result.Add(count);
            
            isZero = !isZero 
                     || isLast255 && isZero;
        }

        return result.ToArray();
    }
}