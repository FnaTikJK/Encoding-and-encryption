﻿namespace Archivator.Jpeg.Infra;

public class CompressedImage
{
	public int Width { get; set; }
	public int Height { get; set; }
	public int Quality { get; set; }
	public byte[] CompressedBytes { get; set; }

	public void WriteInFile(string path)
	{
		using(var sw = new FileStream(path, FileMode.Create))
		{
			byte[] buffer;

			buffer = BitConverter.GetBytes(Width);
			sw.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(Height);
			sw.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(Quality);
			sw.Write(buffer, 0, buffer.Length);

			buffer = BitConverter.GetBytes(CompressedBytes.Length);
			sw.Write(buffer, 0, buffer.Length);

			sw.Write(CompressedBytes, 0, CompressedBytes.Length);
		}
	}

	public static CompressedImage FromFile(string filePath)
	{
		var result = new CompressedImage();
		using(var sr = new FileStream(filePath, FileMode.Open))
		{
			byte[] buffer = new byte[8];

			sr.Read(buffer, 0, 4);
			result.Width = BitConverter.ToInt32(buffer, 0);

			sr.Read(buffer, 0, 4);
			result.Height = BitConverter.ToInt32(buffer, 0);

			sr.Read(buffer, 0, 4);
			result.Quality = BitConverter.ToInt32(buffer, 0);

			sr.Read(buffer, 0, 4);
			var compressedBytesCount = BitConverter.ToInt32(buffer, 0);

			result.CompressedBytes = new byte[compressedBytesCount];
			var totalRead = 0;
			while(totalRead < compressedBytesCount)
				totalRead += sr.Read(result.CompressedBytes, totalRead, compressedBytesCount - totalRead);
		}
		return result;
	}
}