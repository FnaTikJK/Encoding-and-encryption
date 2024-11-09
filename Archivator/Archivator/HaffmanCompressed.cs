using System.Text;

namespace Archivator;

public class HaffmanCompressed
{
    public Dictionary<char, string> DecodeTable { get; set; }
    public byte[] Encoded { get; set; }

    public void WriteInFile(string resultPath, Encoding encoding)
    {
        using var stream = new FileStream(resultPath, FileMode.Create);
        stream.Write(BitConverter.GetBytes(DecodeTable.Count));
        foreach (var pair in DecodeTable)
        {
            stream.Write(BitConverter.GetBytes(pair.Key));
            var buffer = encoding.GetBytes(pair.Value);
            stream.Write(BitConverter.GetBytes(buffer.Length));
            stream.Write(buffer);
        }
        
        stream.Write(Encoded);
    }

    public static HaffmanCompressed FromFile(string filePath, Encoding encoding)
    {
        var haffman = new HaffmanCompressed
        {
            DecodeTable = new Dictionary<char, string>(),
        };
        using var stream = new FileStream(filePath, FileMode.Open);
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, 4);
        var tableLength = BitConverter.ToInt32(buffer, 0);
        
        for (int i = 0; i < tableLength; i++)
        {
            stream.Read(buffer, 0, 2);
            var ch = BitConverter.ToChar(buffer);

            stream.Read(buffer, 0, 4);
            var strLength = BitConverter.ToInt32(buffer);
            stream.Read(buffer, 0, strLength);
            var code =encoding.GetString(buffer, 0, strLength);
            
            haffman.DecodeTable.Add(ch, code);
        }

        haffman.Encoded = new byte[stream.Length - stream.Position];
        stream.Read(haffman.Encoded);

        return haffman;
    }
}