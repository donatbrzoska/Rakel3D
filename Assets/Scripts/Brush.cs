using UnityEngine;

/*
 TODO use Vector2Int?
    - not performant though
 */

public class Brush
{
    private int Size; // amount of pixels the sides of the square have
    private Vector2 Position;

    private Vector2 Direction;

    public Brush(int size)
    {
        // TODO what about even numbers?
        Size = size; // always keep uneven so there is a center to rotate around
    }

    public void UpdatePosition(Vector2 pos)
    {
        Position = pos;
    }

    public void UpdateDirection(Vector2 dir)
    {
        Debug.Log("updating direction to " + dir);
        Direction = dir;
    }

    public void UpdateTexture(Texture2D texture)
    {
        // calculate brush footprint mask
        // map active mask points to texture coordinates
        // set these pixels

        /*
         * points -> initial brush orientation as int
         * points -> rotated brush orientation as float
         * points -> rotated brush orientation as int
         * draw line between pixels
         * fill area
         */
        //Vector2[] FootprintDescriptionPoints = new Vector2[4];
        bool[,] mask = GenerateMask(Size, Direction);
        ApplyMask(mask, Rasterize(Position), texture);




        //Vector2[] pixelsToSet = new Vector2[Size*Size];
        //int x = (int)Position.x;
        //int y = (int)Position.y;

        //int x_min = x - Size / 2;
        //int y_min = y - Size / 2;

        //for (int i=x_min; i<x_min + Size; i++)
        //{
        //    for (int j=y_min; j<y_min + Size; j++)
        //    {
        //        if (i>=0 && j>=0) // TODO greater than 0 && smaller than CanvasBoundaries
        //            texture.SetPixel(i, j, Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 0f, 1f));
        //    }
        //}
        ////Debug.Log("setting texture at " + x + " " + y);
        //texture.Apply();
    }

    private Vector2Int MaskPointToTexturePoint(int maskX, int maskY, int maskSize, Vector2 maskPosition)
    {
        return new Vector2Int((int) maskPosition.x - maskSize / 2 + maskX, (int) maskPosition.y - maskSize / 2 + maskY);
    }

    private void ApplyMask(bool[,] mask, Vector2 maskPosition, Texture2D texture)
    {
        for (int i=0; i<mask.GetLength(0); i++)
        {
            for (int j = 0; j < mask.GetLength(1); j++) // NOTE mask.GetLength(0) would also work, since the mask is a perfect square
            {
                Vector2Int tc = MaskPointToTexturePoint(i, j, mask.GetLength(0), maskPosition);
                texture.SetPixel(tc.x, tc.y, Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 0f, 1f));
            }
        }
        texture.Apply();
    }

    private Vector2 RotateAroundOrigin(Vector2 vec, float angle)
    {
        float angle_rad = angle * Mathf.Deg2Rad;
        float angle_minus_90_rad = (angle - 90) * Mathf.Deg2Rad;
        return new Vector2(
            Mathf.Cos(-angle_rad) * vec.x + Mathf.Cos(angle_minus_90_rad),
            Mathf.Sin(-angle_rad) * vec.y + Mathf.Sin(angle_minus_90_rad));
    }

    /* returns vertices of a rectangle, which is centered like this:
     *    |
     *   ##
     * --##--------
     *   ##
     *    |
     */
    private Vector2[] RectangleVerticesFromNormal(int brushWidth, int brushThickness, Vector2 direction)
    {

        // 1. Generate unrotated vertices
        Vector2 ul = new Vector2(-brushThickness + 1, brushWidth / 2);
        Vector2 ur = new Vector2(0, brushWidth / 2);
        Vector2 ll = new Vector2(-brushThickness + 1, -brushWidth / 2);
        Vector2 lr = new Vector2(0, -brushWidth / 2);

        // 2. Rotate vertices
        float angle = Vector2.Angle(Vector2.right, direction);
        Debug.Log("Angle between " + Vector2.right.normalized + " and " + direction.normalized + " is " + angle);
        return new Vector2[]
        {
            RotateAroundOrigin(ul, angle),
            RotateAroundOrigin(ur, angle),
            RotateAroundOrigin(ll, angle),
            RotateAroundOrigin(lr, angle),
        };
    }

    private Vector2 Rasterize(Vector2 vec)
    {
        return new Vector2((int)vec.x, (int)vec.y);
    }

    private Vector2[] Rasterize(Vector2[] vecs)
    {
        for (int i = 0; i < vecs.Length; i++)
        {
            vecs[i] = Rasterize(vecs[i]);
        }
        return vecs;
    }

    private Vector2[] Translate(Vector2[] vecs, Vector2 by)
    {
        for (int i=0; i<vecs.Length; i++)
        {
            vecs[i] = vecs[i] + by;
        }
        return vecs;
    }

    // Bresenham from https://stackoverflow.com/a/11683720
    public void DrawLine(bool[,] target, int x1, int y1, int x2, int y2)
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
            target[x1, y1] = true;
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

    private void DrawRectangle(bool[,] target, Vector2[] vertices)
    {
        DrawLine(target, (int)vertices[0].x, (int)vertices[0].y, (int)vertices[1].x, (int)vertices[1].y);
        DrawLine(target, (int)vertices[1].x, (int)vertices[1].y, (int)vertices[2].x, (int)vertices[2].y);
        DrawLine(target, (int)vertices[2].x, (int)vertices[2].y, (int)vertices[3].x, (int)vertices[3].y);
        DrawLine(target, (int)vertices[3].x, (int)vertices[3].y, (int)vertices[0].x, (int)vertices[0].y);
    }

    // Is this a scanline algorithm?
    private void FillRectangle(bool[,] target)
    {
        int start;
        int end;
        for (int i=0; i<target.GetLength(0); i++)
        {
            // find start and beginning
            start = -1;
            end = -1;
            for (int j=0; j<target.GetLength(1); j++)
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

            // fill if there were start and end
            if (start != -1 && end != -1)
            {
                for (int k=start; i<=end; i++)
                {
                    target[i, k] = true;
                }
            }
        }
    }

    private void DrawMask(bool[,] target, Vector2[] rasterizedRectangleVertices)
    {
        DrawRectangle(target, rasterizedRectangleVertices);
        FillRectangle(target);
    }

    private bool[,] GenerateMask(int size, Vector2 brushDirection)
    {
        // TODO parameterize
        int WIDTH = 21; // TODO only allow odd numbers
        int THICKNESS = 8;

        Vector2[] rectangleVertices = RectangleVerticesFromNormal(WIDTH, THICKNESS, brushDirection);
        Debug.Log("new rectangle");
        foreach (Vector2 vec in rectangleVertices)
        {
            Debug.Log(vec);
        }
        Vector2[] rasterizedRectangleVertices = Rasterize(rectangleVertices);
        // move to positive "array" space
        Vector2[] movedRasterizedRectangleVertices = Translate(rasterizedRectangleVertices, new Vector2(size/2, size/2));

        bool[,] mask = new bool[WIDTH*2, WIDTH*2];
        DrawMask(mask, movedRasterizedRectangleVertices);
        return mask;
    }
}
