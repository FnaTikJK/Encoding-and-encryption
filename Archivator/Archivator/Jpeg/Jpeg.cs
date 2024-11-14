using System.Drawing;
using System.Drawing.Imaging;
using Archivator.Jpeg.Infra;
using Archivator.Jpeg.Infra.Image;
using PixelFormat = Archivator.Jpeg.Infra.Image.PixelFormat;

namespace Archivator.Jpeg;

public static class Jpeg
{
    public const int CompressionQuality = 70;
	private const int DCTSize = 8;

	public static void Compress(string imagePath, string compressedImagePath)
	{
		using var fileStream = File.OpenRead(imagePath);
		using var bmp = (Bitmap)Image.FromStream(fileStream, false, false);
		var imageMatrix = (Matrix)bmp;
		var compressionResult = Compress(imageMatrix, CompressionQuality);
		compressionResult.WriteInFile(compressedImagePath);
	}

	public static void Decompress(string compressedImagePath, string uncompressedImagePath)
	{
		var compressedImage = CompressedImage.FromFile(compressedImagePath);
		var uncompressedImage = Decompress(compressedImage);
		var resultBmp = (Bitmap)uncompressedImage;
		resultBmp.Save(uncompressedImagePath, ImageFormat.Bmp);
	}

	private static CompressedImage Compress(Matrix matrix, int quality = 50)
	{
		var allQuantizedBytes = new List<byte>();

		var selectors = new Func<Pixel, double>[] {p => p.Y, p => p.Cb, p => p.Cr};
		for (var y = 0; y < matrix.Height; y += DCTSize)
		{
			for (var x = 0; x < matrix.Width; x += DCTSize)
			{
				foreach (var selector in selectors)
				{
					var subMatrix = GetSubMatrix(matrix, y, DCTSize, x, DCTSize, selector);
					ShiftMatrixValues(subMatrix, -128);
					var channelFreqs = DCT.DCT2D(subMatrix);
					var quantizedFreqs = Quantizer.Quantize(channelFreqs, quality);
					var plainBytes = ZigZag.Scan(quantizedFreqs);
					allQuantizedBytes.AddRange(plainBytes);	
				}
			}
		}

		var compressedBytes = RLE.Encode(allQuantizedBytes);

		return new CompressedImage
		{
			Quality = quality,
			CompressedBytes = compressedBytes,
			Height = matrix.Height, 
			Width = matrix.Width
		};
	}

	private static Matrix Decompress(CompressedImage image)
	{
		var result = new Matrix(image.Height, image.Width);
		var channels = new[]
		{
			new double[DCTSize, DCTSize], // Y
			new double[DCTSize, DCTSize], // Cb
			new double[DCTSize, DCTSize], // Cr
		};
		var buffer = new byte[DCTSize * DCTSize];
		using var allQuantizedBytes = new MemoryStream(RLE.Decode(image.CompressedBytes));
		for (var y = 0; y < image.Height; y += DCTSize)
		{
			for (var x = 0; x < image.Width; x += DCTSize)
			{
				foreach (var channel in channels)
				{
					allQuantizedBytes.ReadAsync(buffer, 0, buffer.Length).Wait();
					var quantizedFreqs = ZigZag.UnScan(buffer.AsSpan());
					var channelFreqs = Quantizer.DeQuantize(quantizedFreqs, image.Quality);
					DCT.IDCT2D(channelFreqs, channel);
					ShiftMatrixValues(channel, 128);
				}

				SetPixels(result, channels[0], channels[1], channels[2], PixelFormat.YCbCr, y, x);
			}
		}

		return result;
	}

	private static void ShiftMatrixValues(double[,] subMatrix, int shiftValue)
	{
		var height = subMatrix.GetLength(0);
		var width = subMatrix.GetLength(1);

		for (var y = 0; y < height; y++)
		for (var x = 0; x < width; x++)
			subMatrix[y, x] += shiftValue;
	}

	private static void SetPixels(Matrix matrix, double[,] a, double[,] b, double[,] c, PixelFormat format,
		int yOffset, int xOffset)
	{
		var height = a.GetLength(0);
		var width = a.GetLength(1);

		for (var y = 0; y < height; y++)
		for (var x = 0; x < width; x++)
			matrix.Pixels[yOffset + y, xOffset + x] = new Pixel(a[y, x], b[y, x], c[y, x], format);
	}

	private static double[,] GetSubMatrix(Matrix matrix, int yOffset, int yLength, int xOffset, int xLength,
		Func<Pixel, double> componentSelector)
	{
		var result = new double[yLength, xLength];
		for (var j = 0; j < yLength; j++)
		for (var i = 0; i < xLength; i++)
			result[j, i] = componentSelector(matrix.Pixels[yOffset + j, xOffset + i]);
		return result;
	}
}