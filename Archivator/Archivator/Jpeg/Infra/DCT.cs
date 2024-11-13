namespace Archivator.Jpeg.Infra;

public class DCT
{
    public static double[,] DCT2D(double[,] input)
    {
        var height = input.GetLength(0);
        var width = input.GetLength(1);
        var coeffs = new double[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var sum = Sum(input, x, y, height, width);
                coeffs[x,y] = sum * Beta(height, width) * Alpha(x) * Alpha(y);
            }
        }
        
        return coeffs;
    }

    private static double Sum(double[,] matrix, int curX, int curY, int height, int width)
    {
        var sum = 0d;
        for (var x = 0; x < width; x++)
        {
            for(var y = 0;y<height;y++)
            {
                sum += BasisFunction(matrix[x, y], curX, curY, x, y, height, width);
            }
        }

        return sum;
    }

    public static void IDCT2D(double[,] coeffs, double[,] output)
    {
        var height = coeffs.GetLength(0);
        var width = coeffs.GetLength(1);
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var sum = Sum2(coeffs, x, y, height, width);
                output[x, y] = sum * Beta(height, width);
            }
        }
    }

    private static double Sum2(double[,] matrix, int curX, int curY, int height, int width)
    {
        var sum = 0d;
        for (var x = 0; x < width; x++)
        {
            for(var y = 0;y<height;y++)
            {
                sum += BasisFunction(matrix[x, y], x, y, curX, curY, height, width) *
                       Alpha(x) * Alpha(y);
            }
        }

        return sum;
    }

    public static double BasisFunction(double a, double u, double v, double x, double y, int height, int width)
    {
        var b = Math.Cos(((2d * x + 1d) * u * Math.PI) / (2 * width));
        var c = Math.Cos(((2d * y + 1d) * v * Math.PI) / (2 * height));

        return a * b * c;
    }

    private static double Alpha(int u)
    {
        if (u == 0)
            return 1 / Math.Sqrt(2);
        return 1;
    }

    private static double Beta(int height, int width)
    {
        return 1d / width + 1d / height;
    }
}