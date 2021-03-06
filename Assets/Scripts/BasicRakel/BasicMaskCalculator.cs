using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicMaskCalculator: MaskCalculator
{
    public BasicMaskCalculator() { }

    /*
     * Returns a mask as an array which can be layed onto a coordinate system as it is
     * 
     * initial orientation:
     *    |
     *   ##
     * --##--------
     *   ##
     *    |
     */
    public virtual bool[,] Calculate(int height, int width, Vector2 normal)
    {
        if (height % 2 == 0)
            height += 1;

        List<Vector2Int> rectangleVertices = RectangleVerticesFromNormal(height, width, normal);

        int maskSize = Mathf.Max(height, width) * 2 + 1; // always ensure a precise center
        bool[,] mask = new bool[maskSize, maskSize];
        DrawLines(rectangleVertices, mask);
        FillRectangle(mask);

        return mask;
    }

    private void DrawLines(List<Vector2Int> vertices, bool[,] target)
    {
        List<Vector2Int> asv = ToPositiveSpace(vertices, target.GetLength(0));

        DrawLine(asv[0].x, asv[0].y, asv[1].x, asv[1].y, target);
        DrawLine(asv[1].x, asv[1].y, asv[2].x, asv[2].y, target);
        DrawLine(asv[2].x, asv[2].y, asv[3].x, asv[3].y, target);
        DrawLine(asv[3].x, asv[3].y, asv[0].x, asv[0].y, target);
    }

    /* Normal space:
     *    |
     *   11
     * --11--------
     *   11
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
}