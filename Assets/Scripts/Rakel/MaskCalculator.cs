using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MaskCalculator
{
    /* returns vertices of a rectangle, which is centered like this:
     *    |
     *   ##
     * --##--------
     *   ##
     *    |
     * Order of coordinates is upper left, upper right, lower right, lower left
     */
    protected List<Vector2Int> RectangleVerticesFromNormal(int height, int width, Vector2 direction)
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
}
