using System.Text;
using Encryption.DES;
using Encryption.Vigener;

try
{
    Console.WriteLine("Commands: Vigener, DES");
    Console.WriteLine("Введите команду вида: {Command} {FileName} {Key}");
    var commands = Console.ReadLine()!.ToLower().Split().Concat(new[] {""}).ToArray();
    var (cmd, filename, keyStr, type) = (commands[0], commands[1], commands[2], commands[3]);
    Console.WriteLine($"{cmd}. Работаем...");
    if (cmd == "vigener")
    {
        var data = File.ReadAllText($"testing/{filename}");
        var encrypted = Vigener.Encrypt(data, keyStr);
        File.WriteAllText($"testing/Vigener.encrypted.{filename}", encrypted, Encoding.Default);
        var decypted = Vigener.Decrypt(encrypted, keyStr);
        File.WriteAllText($"testing/Vigener.decrypted.{filename}", decypted, Encoding.Default);
    }
    else if (cmd == "des")
    {
        var key = Encoding.Default.GetBytes(keyStr);
        Func<string, string> encrypt = (string filenameToEncrypt) =>
        {
            var data = File.ReadAllBytes(filenameToEncrypt);
            var encrypted = DES.Encrypt(data, key);
            var ecnrFilename = $"testing/DES.encrypted.{filename}";
            File.WriteAllBytes(ecnrFilename, encrypted);
            return ecnrFilename;
        };
        Action<string> decrypte = (ecnrFilename) =>
        {
            var encrypted = File.ReadAllBytes(ecnrFilename);
            var decypted = DES.Decrypt(encrypted, key);
            File.WriteAllBytes($"testing/DES.decrypted.{filename}", decypted);
        };
        Handle(type, encrypt, decrypte, $"testing/{filename}");
    }
    
    Console.WriteLine("Успех");
}   
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

void Handle(string type, Func<string, string> encrypt, Action<string> decrypt, string filename)
{
    if (string.IsNullOrEmpty(type) || "full".Contains(type))
    {
        var encrypted = encrypt(filename);
        decrypt(encrypted);
    }
    if ("encrypt".Contains(type))
        encrypt(filename);
    else if ("decrypt".Contains(type))
        decrypt(filename);
}