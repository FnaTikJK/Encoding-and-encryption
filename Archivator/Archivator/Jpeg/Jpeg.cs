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
		compressionResult.Save(compressedImagePath);
	}

	public static void Decompress(string compressedImagePath, string uncompressedImagePath)
	{
		var compressedImage = CompressedImage.Load(compressedImagePath);
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
					var quantizedBytes = ZigZagScan(quantizedFreqs);
					allQuantizedBytes.AddRange(quantizedBytes);
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
					var quantizedFreqs = ZigZagUnScan(buffer);
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
			subMatrix[y, x] = subMatrix[y, x] + shiftValue;
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

	private static IEnumerable<byte> ZigZagScan(byte[,] channelFreqs)
	{
		return new[]
		{
			channelFreqs[0, 0], channelFreqs[0, 1], channelFreqs[1, 0], channelFreqs[2, 0], channelFreqs[1, 1],
			channelFreqs[0, 2], channelFreqs[0, 3], channelFreqs[1, 2],
			channelFreqs[2, 1], channelFreqs[3, 0], channelFreqs[4, 0], channelFreqs[3, 1], channelFreqs[2, 2],
			channelFreqs[1, 3], channelFreqs[0, 4], channelFreqs[0, 5],
			channelFreqs[1, 4], channelFreqs[2, 3], channelFreqs[3, 2], channelFreqs[4, 1], channelFreqs[5, 0],
			channelFreqs[6, 0], channelFreqs[5, 1], channelFreqs[4, 2],
			channelFreqs[3, 3], channelFreqs[2, 4], channelFreqs[1, 5], channelFreqs[0, 6], channelFreqs[0, 7],
			channelFreqs[1, 6], channelFreqs[2, 5], channelFreqs[3, 4],
			channelFreqs[4, 3], channelFreqs[5, 2], channelFreqs[6, 1], channelFreqs[7, 0], channelFreqs[7, 1],
			channelFreqs[6, 2], channelFreqs[5, 3], channelFreqs[4, 4],
			channelFreqs[3, 5], channelFreqs[2, 6], channelFreqs[1, 7], channelFreqs[2, 7], channelFreqs[3, 6],
			channelFreqs[4, 5], channelFreqs[5, 4], channelFreqs[6, 3],
			channelFreqs[7, 2], channelFreqs[7, 3], channelFreqs[6, 4], channelFreqs[5, 5], channelFreqs[4, 6],
			channelFreqs[3, 7], channelFreqs[4, 7], channelFreqs[5, 6],
			channelFreqs[6, 5], channelFreqs[7, 4], channelFreqs[7, 5], channelFreqs[6, 6], channelFreqs[5, 7],
			channelFreqs[6, 7], channelFreqs[7, 6], channelFreqs[7, 7]
		};
	}

	private static byte[,] ZigZagUnScan(IReadOnlyList<byte> quantizedBytes)
	{
		return new[,]
		{
			{
				quantizedBytes[0], quantizedBytes[1], quantizedBytes[5], quantizedBytes[6], quantizedBytes[14],
				quantizedBytes[15], quantizedBytes[27], quantizedBytes[28]
			},
			{
				quantizedBytes[2], quantizedBytes[4], quantizedBytes[7], quantizedBytes[13], quantizedBytes[16],
				quantizedBytes[26], quantizedBytes[29], quantizedBytes[42]
			},
			{
				quantizedBytes[3], quantizedBytes[8], quantizedBytes[12], quantizedBytes[17], quantizedBytes[25],
				quantizedBytes[30], quantizedBytes[41], quantizedBytes[43]
			},
			{
				quantizedBytes[9], quantizedBytes[11], quantizedBytes[18], quantizedBytes[24], quantizedBytes[31],
				quantizedBytes[40], quantizedBytes[44], quantizedBytes[53]
			},
			{
				quantizedBytes[10], quantizedBytes[19], quantizedBytes[23], quantizedBytes[32], quantizedBytes[39],
				quantizedBytes[45], quantizedBytes[52], quantizedBytes[54]
			},
			{
				quantizedBytes[20], quantizedBytes[22], quantizedBytes[33], quantizedBytes[38], quantizedBytes[46],
				quantizedBytes[51], quantizedBytes[55], quantizedBytes[60]
			},
			{
				quantizedBytes[21], quantizedBytes[34], quantizedBytes[37], quantizedBytes[47], quantizedBytes[50],
				quantizedBytes[56], quantizedBytes[59], quantizedBytes[61]
			},
			{
				quantizedBytes[35], quantizedBytes[36], quantizedBytes[48], quantizedBytes[49], quantizedBytes[57],
				quantizedBytes[58], quantizedBytes[62], quantizedBytes[63]
			}
		};
	}
}