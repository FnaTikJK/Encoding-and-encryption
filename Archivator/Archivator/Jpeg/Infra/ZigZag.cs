namespace Archivator.Jpeg.Infra;

public static class ZigZag
{
    public static IEnumerable<T> Scan<T>(this T[,] arr)
    {
        var height = arr.GetLength(0);
        var width = arr.GetLength(1);
        var mover = new ZigZagMover(height, width);
        while (mover.MoveNext(out var x, out var y))
            yield return arr[y, x];
    }

    public static T[,] UnScan<T>(this Span<T> span)
    {
        var size = (int)Math.Sqrt(span.Length);
        var result = new T[size, size];
        var mover = new ZigZagMover(size, size);
        foreach (var e in span)
        {
            mover.MoveNext(out var x, out var y);
            result[y, x] = e;
        }

        return result;
    }
}

public class ZigZagMover
{
    private int curX = 0;
    private int curY = 0;
    private (int, int) step = (0, 0);
    private bool moveRight = true;
    private bool isBorderMove = false;
    private bool isDowning = true;
    private int height;
    private int width;
    private bool isFirst = true;

    public ZigZagMover(int height, int width)
    {
        this.height = height;
        this.width = width;
    }

    public bool MoveNext(out int x, out int y)
    {
        x = 0;
        y = 0;
        if (isFirst)
        {
            isFirst = false;
            return true;
        }
        if (curX == width - 1 && curY == height - 1)
            return false;
        
        if (!isBorderMove && 
            (curX == 0 || curX == width - 1 || curY == 0 || curY == height - 1))
        {
            if (curY == height - 1)
            {
                moveRight = true;
                isDowning = false;
            }

            if (moveRight)
            {
                curX++;
                step = isDowning ? (1, -1) : (-1, 1);
            }
            else
            {
                curY++;
                step = isDowning ? (-1, 1) : (1, -1);
            }

            isBorderMove = true;
            moveRight = !moveRight;
        }
        else
        {
            isBorderMove = false;
            curY += step.Item1;
            curX += step.Item2;
        }

        x = curX;
        y = curY;
        return true;
    }
}