using Archivator;

var filePath = @"C:\Users\Антон\source\repos\c#\Encoding-and-encryption\Archivator\test.txt";
var compressedFilePath = @"C:\Users\Антон\source\repos\c#\Encoding-and-encryption\Archivator\compressed";
var dictFilePath = @"C:\Users\Антон\source\repos\c#\Encoding-and-encryption\Archivator\dict";
var decompressedFilePath = @"C:\Users\Антон\source\repos\c#\Encoding-and-encryption\Archivator\decompressed";




Haffman.Compress(filePath, dictFilePath, compressedFilePath);
Haffman.Decompress(dictFilePath, compressedFilePath, decompressedFilePath);


