using System;
using System.Collections.Generic;
using UnityEngine;

public class RectangleFootprint
{
    private int Height;
    private int Width;
    private Vector2 Normal;

    /* initial orientation:
     *    |
     *   ##
     * --##--------
     *   ##
     *    |
     */
    public RectangleFootprint(int height, int width, Vector2 normal)
    {
        Height = height; // TODO ensure uneven values so there always is a center
        Width = width;
        Normal = normal;
    }

    /*
     * Returns a mask as an array which can be layed onto a coordinate system as it is
     */
    public bool[,] GenerateMask()
    {
        List<Vector2Int> rectangleVertices = RectangleVerticesFromNormal(Height, Width, Normal);

        int maskSize = Mathf.Max(Height, Width) * 2 + 1; // always ensure a precise center
        bool[,] mask = new bool[maskSize, maskSize];
        DrawLines(rectangleVertices, mask);
        FillRectangle(mask);

        return mask;
    }

    public override string ToString()
    {
        string result = "";

        bool[,] mask = GenerateMask();
        for (int i=0; i<mask.GetLength(0); i++)
        {
            for (int j=0; j<mask.GetLength(1); j++)
            {
                if (mask[i,j] == true)
                {
                    result += " 1";
                }
                else
                {
                    result += " 0";
                }
            }
            result += "\n";
        }
        return result;
    }

    ///* Returns vertices of a rectangle, which is centered like this:
    // *    |
    // *   ##
    // * --##--------
    // *   ##
    // *    |
    // * Order of coordinates is upper left, upper right, lower left, lower right
    // */
    //private List<Vector2Int> InitialRectangleVertices(int width, int height)
    //{
    //    Vector2Int ul = new Vector2Int(-width + 1, height / 2);
    //    Vector2Int ur = new Vector2Int(0, height / 2);
    //    Vector2Int ll = new Vector2Int(-width + 1, -height / 2);
    //    Vector2Int lr = new Vector2Int(0, -width / 2);

    //    return new List<Vector2Int> { ul, ur, ll, lr };
    //}


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
        //LogUtil.Log(new List<Vector2Int> { ul, ur, ll, lr}, "right after generating");

        // 2. Rotate vertices
        float angle = Vector2.Angle(Vector2.right, direction);
        //Debug.Log("Angle between " + Vector2.right.normalized + " and " + direction.normalized + " is " + angle);
        return new List<Vector2Int> {
            RotateAroundOrigin(ul, angle),
            RotateAroundOrigin(ur, angle),
            RotateAroundOrigin(lr, angle),
            RotateAroundOrigin(ll, angle),
        };
    }

    private Vector2Int RotateAroundOrigin(Vector2Int vec, float angle)
    {
        //float angle_rad = angle * Mathf.Deg2Rad;
        //float angle_minus_90_rad = (angle - 90) * Mathf.Deg2Rad;
        //Vector2Int result = new Vector2Int(
        //    Mathf.RoundToInt(Mathf.Cos(-angle_rad) * vec.x + Mathf.Cos(angle_minus_90_rad)),
        //    Mathf.RoundToInt(Mathf.Sin(-angle_rad) * vec.y + Mathf.Sin(angle_minus_90_rad)));
        //Debug.Log(vec + " is now " + result);
        //return result;

        float rad = angle * Mathf.Deg2Rad;
        float s = Mathf.Sin(rad);
        float c = Mathf.Cos(rad);
        Vector2Int result = new Vector2Int(
            Mathf.RoundToInt(vec.x * c + vec.y * s),
            Mathf.RoundToInt(vec.y * c - vec.x * s));
        //Debug.Log(vec + " is now " + result);
        return result;
    }

    private void DrawLines(List<Vector2Int> vertices, bool[,] target)
    {
        List<Vector2Int> asv = ToPositiveSpace(vertices, target.GetLength(0));

        DrawLine(asv[0].x, asv[0].y, asv[1].x, asv[1].y, target);
        //LogUtil.Log(target, "1");
        DrawLine(asv[1].x, asv[1].y, asv[2].x, asv[2].y, target);
        //LogUtil.Log(target, "2");
        DrawLine(asv[2].x, asv[2].y, asv[3].x, asv[3].y, target);
        //LogUtil.Log(target, "3");
        DrawLine(asv[3].x, asv[3].y, asv[0].x, asv[0].y, target);
        //LogUtil.Log(target, "4");
    }

    /* Normal space:
     *    |
     *   ##
     * --##--------
     *   ##
     *    |
     *
     * Positive space:
     *    |
     *    0 0 0 0 0 0 0
     *    0 0 0 0 0 0 0
     *    0 0 1 1 0 0 0
     *    0 0 1 1 0 0 0
     *    0 0 1 1 0 0 0
     *    0 0 0 0 0 0 0
     * ---0 0 0 0 0 0 0-
     *    |
     * -> consider mask size!
     * -> in this case it's 7 (see "maskSize =" calculation)
     *
     */
    private List<Vector2Int> ToPositiveSpace(List<Vector2Int> normalSpaceCoordinates, int arraySize)
    {
        List<Vector2Int> result = new();

        int newOrigin = arraySize / 2;
        Vector2Int translation = new(newOrigin, newOrigin);
        foreach (Vector2Int v in normalSpaceCoordinates)
        {
            result.Add(v + translation);
        }
        return result;
    }

    // Bresenham from https://stackoverflow.com/a/11683720
    private void DrawLine(int x1, int y1, int x2, int y2, bool[,] target)
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
            //Debug.Log("drawing at " + x1 + "/" + y1);
            Vector2Int arrayIJ = ToArraySpace(x1, y1, target.GetLength(0));
            target[arrayIJ.x, arrayIJ.y] = true;
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

    // TODO merge with ToPositiveSpace
    private Vector2Int ToArraySpace(int x, int y, int arrayRows)
    {
        return new Vector2Int(arrayRows - 1 - y, x);
    }

    // Is this a scanline algorithm?
    private void FillRectangle(bool[,] target)
    {
        //Debug.Log("array has " + target.GetLength(0) + " rows and " + target.GetLength(1) + " cols");
        int start;
        int end;
        for (int i = 0; i < target.GetLength(0); i++)
        {
            // find start and beginning
            start = -1;
            end = -1;
            for (int j = 0; j < target.GetLength(1); j++)
            {
                if (target[i, j] == true)
                {
                    if (start == -1)
                    {
                        start = j;
                    }
                    else // start already found
                    {
                        end = j;
                    }
                }
            }
            //Debug.Log("start is " + start + " end is " + end);

            // fill if there were start and end
            if (start != -1 && end != -1)
            {
                for (int k = start; k <= end; k++)
                {
                    //Debug.Log("setting " + i + "," + k);
                    target[i, k] = true;
                }
            }
        }
    }


    //private List<Vector2Int> DrawRectangle(List<Vector2Int> rectangleVertices)
    //{
    //    DrawRectangleLines(rectangleVertices);
    //    FillRectangle();
    //}

    //private void DrawRectangleLines(List<Vector2Int> rectangleVertices)
    //{
    //    DrawLine(target, (int)vertices[0].x, (int)vertices[0].y, (int)vertices[1].x, (int)vertices[1].y);
    //    DrawLine(target, (int)vertices[1].x, (int)vertices[1].y, (int)vertices[2].x, (int)vertices[2].y);
    //    DrawLine(target, (int)vertices[2].x, (int)vertices[2].y, (int)vertices[3].x, (int)vertices[3].y);
    //    DrawLine(target, (int)vertices[3].x, (int)vertices[3].y, (int)vertices[0].x, (int)vertices[0].y);
    //}

    //// Bresenham from https://stackoverflow.com/a/11683720
    //public List<Vector2Int> DrawLine(int x1, int y1, int x2, int y2)
    //{
    //    List<Vector2Int> coordinates = new List<Vector2Int>();

    //    int w = x2 - x1;
    //    int h = y2 - y1;
    //    int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
    //    if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
    //    if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
    //    if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
    //    int longest = Mathf.Abs(w);
    //    int shortest = Mathf.Abs(h);
    //    if (!(longest > shortest))
    //    {
    //        longest = Mathf.Abs(h);
    //        shortest = Mathf.Abs(w);
    //        if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
    //        dx2 = 0;
    //    }
    //    int numerator = longest >> 1;
    //    for (int i = 0; i <= longest; i++)
    //    {
    //        //Debug.Log("drawing at " + x1 + "/" + y1);
    //        //target[x1, y1] = true;
    //        coordinates.Add(new Vector2Int(x1, y1));
    //        numerator += shortest;
    //        if (!(numerator < longest))
    //        {
    //            numerator -= longest;
    //            x1 += dx1;
    //            y1 += dy1;
    //        }
    //        else
    //        {
    //            x1 += dx2;
    //            y1 += dy2;
    //        }
    //    }

    //    return coordinates;
    //}

}