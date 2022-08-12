using System;
using System.Collections.Generic;
using UnityEngine;

public struct Mask
{
    public int[,] coordinates;
    public int y_eq_0_index; // -> At which index is y == 0? | Whats the value of y at index 0?
}

public class MaskCalculator
{
    public MaskCalculator()
    {
    }

    /*
     * Returns a mask as an array of start and end x-coordinates
     * The row indicates the y, first row representing the highest y
     * 
     * initial orientation:
     *    |
     *   ##
     * --##--------
     *   ##
     *    |
     */
    public virtual Mask Calculate(int height, int width, Vector2 normal)
    {
        // always ensure a precise center
        if (height % 2 == 0)
            height += 1;

        List<Vector2Int> rectangleVertices = RectangleVerticesFromNormal(height, width, normal);

        int y_max = Mathf.Max(Mathf.Max(Mathf.Max(rectangleVertices[0].y, rectangleVertices[1].y), rectangleVertices[2].y), rectangleVertices[3].y);
        int y_min = Mathf.Min(Mathf.Min(Mathf.Min(rectangleVertices[0].y, rectangleVertices[1].y), rectangleVertices[2].y), rectangleVertices[3].y);
        int n_rows = y_max - y_min + 1;
        Mask mask = new Mask();
        mask.coordinates = new int[n_rows, 2];
        mask.y_eq_0_index = y_max; // because the highest y is stored first

        // initialize, so both coordinates are ensured to get set
        for (int i = 0; i < mask.coordinates.GetLength(0); i++)
        {
            mask.coordinates[i, 0] = int.MaxValue;
            mask.coordinates[i, 1] = int.MinValue;
        }

        DrawRectangle(rectangleVertices, mask);

        return mask;
    }

    /* returns vertices of a rectangle, which is centered like this:
     *    |
     *   ##
     * --##--------
     *   ##
     *    |
     * Order of coordinates is upper left, upper right, lower right, lower left
     */
    private List<Vector2Int> RectangleVerticesFromNormal(int height, int width, Vector2 direction)
    {
        // 1. Generate unrotated vertices
        Vector2Int ul = new Vector2Int(-width + 1, height / 2);
        Vector2Int ur = new Vector2Int(0, height / 2);
        Vector2Int ll = new Vector2Int(-width + 1, -height / 2);
        Vector2Int lr = new Vector2Int(0, -height / 2);

        // 2. Rotate vertices
        float angle = MathUtil.Angle360(Vector2.right, direction);
        return new List<Vector2Int> {
            MathUtil.RotateAroundOrigin(ul, angle),
            MathUtil.RotateAroundOrigin(ur, angle),
            MathUtil.RotateAroundOrigin(lr, angle),
            MathUtil.RotateAroundOrigin(ll, angle),
        };
    }

    private void DrawRectangle(List<Vector2Int> vs, Mask target)
    {
        DrawLine(vs[0].x, vs[0].y, vs[1].x, vs[1].y, target);
        DrawLine(vs[1].x, vs[1].y, vs[2].x, vs[2].y, target);
        DrawLine(vs[2].x, vs[2].y, vs[3].x, vs[3].y, target);
        DrawLine(vs[3].x, vs[3].y, vs[0].x, vs[0].y, target);
    }

    // TODO generalize for both classes
    // Bresenham from https://stackoverflow.com/a/11683720
    private void DrawLine(int x1, int y1, int x2, int y2, Mask target)
    {
        int w = x2 - x1;
        int h = y2 - y1;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);
        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            int y1_arr = target.y_eq_0_index - y1; // higher y -> lower index

            // always possibly write to both positions, so when the first set is done, we can be sure, that both values are definitely set
            if (x1 < target.coordinates[y1_arr, 0])
            {
                target.coordinates[y1_arr, 0] = x1;
            }
            if (x1 > target.coordinates[y1_arr, 1])
            {
                target.coordinates[y1_arr, 1] = x1;
            }

            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x1 += dx1;
                y1 += dy1;
            }
            else
            {
                x1 += dx2;
                y1 += dy2;
            }
        }
    }
}
