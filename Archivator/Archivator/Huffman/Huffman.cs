using System.Text;

namespace Archivator.Huffman;

public static class Huffman
{
    private static readonly Encoding Encoding = Encoding.Default;
    
    public static void Decompress(string compressedFilePath, string resultFilePath)
    {
        var haffmanCompressed = HuffmanCompressed.FromFile(compressedFilePath, Encoding);
        var dict = haffmanCompressed.DecodeTable
            .ToDictionary(e => e.Value, e => e.Key);
        var bytes = haffmanCompressed.Encoded
            .Select(e => Convert.ToString(e, 2))
            .ToArray();
        var bytesAsString = bytes
            .Take(bytes.Length - 1)
            .Select(ToFullByte)
            .Concat(new[] {bytes.Last()});
        var str = string.Join("", bytesAsString);

        using var writer = new StreamWriter(resultFilePath);
        var curCh = "";
        foreach (var ch in str)
        {
            curCh += ch;
            if (dict.TryGetValue(curCh, out var decodedCh))
            {
                writer.Write(decodedCh);
                curCh = "";
            }
        }
    }

    private static string ToFullByte(string source)
    {
        if (source.Length == 8)
            return source;

        return new string('0', 8 - source.Length) + source;
    }

    public static void Compress(string filePath, string resultFilePath)
    {
        var text = File.ReadAllText(filePath);
        var countByCh = text
            .GroupBy(e => e)
            .Select(e => new { ch = e.Key, count = e.Count()})
            .OrderByDescending(e => e.count)
            .ToDictionary(e => e.ch, e => e.count);
        var codesByCh = GetCodes(countByCh);
        
        var bytes = string.Join("", text.Select(e => codesByCh[e]))
            .Chunk(8)
            .Select(e => new string(e))
            .Select(e => Convert.ToByte(e, 2))
            .ToArray();

        var haffmanCompressed = new HuffmanCompressed
        {
            DecodeTable = codesByCh,
            Encoded = bytes,
        };
        haffmanCompressed.WriteInFile(resultFilePath, Encoding);
    }

    private static Dictionary<char, string> GetCodes(Dictionary<char, int> countPerCodes)
    {
        var dictElementsCount = countPerCodes.Count;
        var treeDepth = 1;
        while (Math.Pow(2, treeDepth) < dictElementsCount)
        {
            treeDepth++;
        }

        return countPerCodes.Select((e, i) => new {ch = e.Key, code = GetCode(i, treeDepth)})
            .ToDictionary(e => e.ch, e => e.code);
    }
    
    private static StringBuilder cur = new("");
    private static string GetCode(int ind, int treeDepth)
    {
        if (ind == 0)
            cur = new StringBuilder(new string('0', treeDepth));
        else
            cur.BitAdd();

        return cur.ToString();
    }
}

public static class StrExtensions
{
    public static void BitAdd(this StringBuilder builder)
    {
        var ind = builder.Length - 1;
        while (builder[ind] != '0')
        {
            builder[ind] = '0';
            ind--;
        }

        builder[ind] = '1';
    }
}