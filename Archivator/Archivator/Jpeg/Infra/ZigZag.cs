namespace Archivator.Jpeg.Infra;

public static class ZigZag
{
    public static IEnumerable<T> AsZigZag<T>(this T[,] arr)
    {
        var height = arr.GetLength(0);
        var width = arr.GetLength(1);
        var x = 0;
        var y = 0;
        var step = (0, 0);
        var moveRight = true;
        while (x != width - 1 && y != height - 1)
        {
            yield return arr[y, x];
            if (x == 0 || x == width - 1  
                || y == 0 || y == height - 1)
            {
                if (moveRight)
                {
                    x++;
                    step = (1, -1);
                }
                else
                {
                    y++;
                    step = (-1, 1);
                }

                moveRight = !moveRight;
            }
            else
            {
                y += step.Item1;
                x += step.Item2;
            }
        }

        yield return arr[y, x];
    }

    public static T[,] UnZigZag<T>(this Span<T> span)
    {
        var size = (int)Math.Sqrt(span.Length);
        var result = new T[size, size];
        throw new Exception();
    }
}