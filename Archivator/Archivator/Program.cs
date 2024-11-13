using Archivator;
using Archivator.Jpeg;

var relPath = "../../../../";

var filePath = @$"{relPath}test.txt";
var compressedFilePath = @$"{relPath}compressed";
var decompressedFilePath = @$"{relPath}decompressed";


// Haffman.Compress(filePath, compressedFilePath);
// Haffman.Decompress(compressedFilePath, decompressedFilePath);


var imagePath = @$"{relPath}sample.bmp";
var imageCompressRes = @$"{relPath}compressedImg";
var decompressedImagePath = @$"{relPath}decompressed.bmp";

Jpeg.Compress(imagePath, imageCompressRes);
Jpeg.Decompress(imageCompressRes, decompressedImagePath);
