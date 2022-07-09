using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

/* Optimizations compared to BasicMaskApplicator
 * - more efficient mask data structure
 * - direct access to texture color array
 * - parallelization
 */
public class OptimizedMaskApplicator
{
    public OptimizedMaskApplicator()
    {
    }

    public virtual void Apply(OptimizedMask mask, Vector2Int maskPosition, OilPaintTexture texture, Color color)
    {
        //for (int i=0; i<mask.coordinates.GetLength(0); i++)
        //{
        //    int x_start = mask.coordinates[i, 0];
        //    int x_end = mask.coordinates[i, 1];
        //    int y_mask = mask.y0_index - i; // because mask.y0_index is also the first y coordinate

        //    for (int x_mask=x_start; x_mask<=x_end; x_mask++)
        //    {
        //        texture.SetPixel(
        //            x_mask + maskPosition.x,
        //            y_mask + maskPosition.y,
        //            color);
        //    }
        //}

        //for (int i=0; i<mask.coordinates.GetLength(0); i++)
        Parallel.For(0, mask.coordinates.GetLength(0), (i, state) =>
        {
            int x_mask_start = mask.coordinates[i, 0];
            int x_mask_end = mask.coordinates[i, 1];
            int y_mask = mask.y0_index - i; // because mask.y0_index is also the first y coordinate

            for (int x_mask = x_mask_start; x_mask <= x_mask_end; x_mask++)
            {
                int x_texture = x_mask + maskPosition.x;
                int y_texture = y_mask + maskPosition.y;
                texture.SetPixelFast(x_texture, y_texture, color);
            }
        //}
        });

        texture.Apply();
    }
}
