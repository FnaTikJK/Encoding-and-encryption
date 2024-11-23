using System.Text;

namespace Encryption.Vigener;

public static class Vigener
{
    public static string Encrypt(string text, string key)
    {
        var shift = GetShift(key);
        var result = new StringBuilder();
        for (var i = 0; i < text.Length; i++)
        {
            var ch = text[i];
            var shifted = ch + shift[i % shift.Length];
            result.Append((char) shifted);
        }

        return result.ToString();
    }

    public static string Decrypt(string encrypted, string key)
    {
        var shift = GetShift(key);
        var result = new StringBuilder();
        for (var i = 0; i < encrypted.Length; i++)
        {
            var ch = encrypted[i];
            var shifted = ch - shift[i % shift.Length];
            result.Append((char) shifted);
        }

        return result.ToString();
    }

    private static int[] GetShift(string key)
    {
        var shift = key
            .Select(ch => (int)ch)
            .ToArray();
        
        return shift;
    }
}