using Archivator;

var relPath = "../../../../";

var filePath = @$"{relPath}test.txt";
var compressedFilePath = @$"{relPath}compressed";
var decompressedFilePath = @$"{relPath}decompressed";


Haffman.Compress(filePath, compressedFilePath);
Haffman.Decompress(compressedFilePath, decompressedFilePath);


var imagePath = @$"{relPath}testImage.jpg";
var resultImagePath = @$"{relPath}resultImage.jpg";

// Jpeg.Compress(imagePath, resultImagePath);
