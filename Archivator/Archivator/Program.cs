using Archivator.Huffman;
using Archivator.Jpeg;

var dir = Directory.GetCurrentDirectory() + "/testing";

try
{
    Console.WriteLine("Введите команду из доступных:");
    Console.WriteLine("`Huffman {fileName}` - сжимает файл Хаффманом");
    Console.WriteLine("`Jpeg {filename}` - сжимает картинку Jpeg-ом");

    var command = Console.ReadLine()!.ToLower().Split();
    string fileName, filePath, compressedFilePath, decompressedFilePath;
    fileName = command[1];
    filePath = $"{dir}/{fileName}";
    compressedFilePath = $"{dir}/{command[0]}.compressed";
    decompressedFilePath = $"{dir}/{command[0]}.decompressed.{fileName}";
    if (command[0] == "huffman")
    {
        Console.WriteLine("Хаффман. Работаем...");
        Huffman.Compress(filePath, compressedFilePath);
        Huffman.Decompress(compressedFilePath, decompressedFilePath);
    }
    else if (command[0] == "jpeg")
    {
        Console.WriteLine("Jpeg. Работаем...");
        Jpeg.Compress(filePath, compressedFilePath);
        Jpeg.Decompress(compressedFilePath, decompressedFilePath);
    }
    else
    {
        Console.WriteLine("Неправильная команда");
        return;
    }

    Console.WriteLine("Успех!");
}
catch (Exception e)
{
    Console.WriteLine($"Exception: {e.Message}. Trace: {e.StackTrace}");
}
Console.ReadKey();