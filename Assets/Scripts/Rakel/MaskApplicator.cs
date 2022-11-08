using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

/* Optimizations compared to previous version:
 * - more efficient mask data structure
 * - direct access to texture color array
 * - parallelization
 */
public class MaskApplicator: IMaskApplicator
{
    public MaskApplicator()
    {
    }

    public void Apply(
        Mask mask,
        Vector2Int maskPosition,
        IOilPaintSurface oilPaintSurface,
        IRakelPaintReservoir paintReservoir)
    {
        float maskAngle = MathUtil.Angle360(Vector2Int.right, mask.normal);

        // 4 Loops:
        // - GetPaint g from Canvas
        // - Emit e from Reservoir
        // - AddPaint e to Canvas
        // - Pickup g to Reservoir

        // 1. Get paint from canvas: Looping through PickupMap
        // TODO Volume through int[,] pickedUpVolumes ...
        Color[,] pickedUp = new Color[paintReservoir.Height, paintReservoir.Width];
        Parallel.For(0, paintReservoir.Height, (y_reservoir, state) =>
        {
            for (int x_reservoir = 0; x_reservoir < paintReservoir.Width; x_reservoir++)
            {
                Vector2Int coord_reservoir = new Vector2Int(x_reservoir, y_reservoir);
                // TODO what about even values for Height?
                Vector2Int coord_reservoir_mask_aligned = coord_reservoir
                                                          - new Vector2Int(paintReservoir.Width - 1, paintReservoir.Height / 2); // TODO duplicate code
                Vector2Int coord_texture = MathUtil.RotateAroundOrigin(coord_reservoir_mask_aligned, maskAngle) + maskPosition;

                pickedUp[y_reservoir, x_reservoir] = oilPaintSurface.GetPaint(coord_texture.x, coord_texture.y);
            }
        });

        // 2. Get paint from reservoir: Looping through Mask
        // TODO Volume through int[,] emittedVolumes ...
        // 4. Add emitted paint to canvas: ALSO Looping through Mask
        Parallel.For(0, mask.coordinates.GetLength(0), (i, state) =>
        {
            int x_mask_start = mask.coordinates[i, 0];
            int x_mask_end = mask.coordinates[i, 1];
            int y_mask = mask.y_eq_0_index - i; // because mask.y0_index is also the first y coordinate

            for (int x_mask = x_mask_start; x_mask <= x_mask_end; x_mask++)
            {
                int x_canvas = x_mask + maskPosition.x;
                int y_canvas = y_mask + maskPosition.y;
                // if the canvas coordinated are out of range, we MUST NOT emit paint from the reservoir
                if (oilPaintSurface.IsInBounds(x_canvas, y_canvas))
                {
                    Vector2Int coord_mask = new Vector2Int(x_mask, y_mask);
                    Vector2Int coord_mask_reservoir_aligned = MathUtil.RotateAroundOrigin(coord_mask, -maskAngle);
                    Vector2Int coord_reservoir = coord_mask_reservoir_aligned
                                            + new Vector2Int(paintReservoir.Width - 1, paintReservoir.Height / 2); // TODO duplicate code

                    Color emitted = paintReservoir.Emit(coord_reservoir.x, coord_reservoir.y);
                    oilPaintSurface.AddPaint(x_canvas, y_canvas, emitted);
                }
            }
        });

        // 3. Add picked up paint to reservoir: Looping through PickupMap
        Parallel.For(0, paintReservoir.Height, (y_reservoir, state) =>
        {
            for (int x_reservoir = 0; x_reservoir < paintReservoir.Width; x_reservoir++)
            {
                paintReservoir.Pickup(x_reservoir, y_reservoir, pickedUp[y_reservoir, x_reservoir], 1);
            }
        });

        //// 1. Transfer paint from Canvas to Rakel: Looping through PickupMap
        //// -> color comes from: reservoir coordinate rotation to mask coordinate
        //// TODO Only pickup color if in mask range (rotated reservoir coordinate is out of mask range)
        //// - find a case where coord_texture would be out of mask range
        //// - then return special value and don't Pickup paint
        //// -> maybe this can be ignored because the out of range cases are only one pixel off and there will be canvas OOB checks
        //// DONE P TODO Only pickup paint if in canvas range
        //// - find a case where coord_texture would be out of range
        //// - then return special value and don't GetPaint
        //// -> this is now all done by proper return values, no extra handling required in this code
        ////   - out of range GetPaint returns empty color value for paint
        ////   - Pickup empty color value for paint does nothing
        //// TODO There will be pixels on the canvas from which no paint is picked up (applies per rakel position)
        ////      And also pixels on the canvas from which paint is picked up twice!
        //// -> interpolate volume somehow?
        ////   - generates new paint though...
        //// -> always pickup paint from canvas pixels around also?
        //// -> maybe there is no problem because of the motion though, maybe this is good even

        //Parallel.For(0, paintReservoir.Height, (y_reservoir, state) =>
        //{
        //    for (int x_reservoir = 0; x_reservoir < paintReservoir.Width; x_reservoir++)
        //    {
        //        Vector2Int coord_reservoir = new Vector2Int(x_reservoir, y_reservoir);
        //        // TODO what about even values for Height?
        //        Vector2Int coord_reservoir_mask_aligned = coord_reservoir
        //                                                  - new Vector2Int(paintReservoir.Width - 1, paintReservoir.Height / 2); // TODO duplicate code
        //        Vector2Int coord_texture = MathUtil.RotateAroundOrigin(coord_reservoir_mask_aligned, maskAngle) + maskPosition;

        //        Color pickedUpFromCanvas = oilPaintSurface.GetPaint(coord_texture.x, coord_texture.y);
        //        paintReservoir.Pickup(x_reservoir, y_reservoir, pickedUpFromCanvas, 1);
        //    }
        //});

        //// 2. Transfer paint from Rakel to Canvas: Looping through Mask
        //// -> color comes from: reservoir mask coordinate rotation to reservoir coordinate
        //// DONE P TODO Only apply paint if in reservoir range
        //// - find a case where coord_reservoir would be out of range
        //// - then return special value and don't AddPaint
        //// -> this is now all done by proper return values, no extra handling required in this code
        ////   - out of range Emit returns empty color value for paint
        ////   - AddPaint empty color value for paint does nothing
        //// DONE P TODO Only Emit and Add Paint if in Canvas range
        //// -> this not applied paint should not be emitted from the reservoir too
        //// -> 1. calculate texture coordinates
        //// -> 2. check if in Texture range
        //// -> 3. only proceed if in Texture range
        //// TODO There will be pixels on the reservoir from which no paint is emitted (applies per rakel orientation/rotation)
        ////      And also pixels on the reservoir from which paint is emitted twice!
        //// -> interpolate volume somehow?
        ////    - generates new paint though ...
        //// -> always emit paint to from reservoir pixels around also?
        //// -> Reservoir Equalizer -> Paint flowing towards the negative gradient of volume

        //Parallel.For(0, mask.coordinates.GetLength(0), (i, state) =>
        //{
        //    int x_mask_start = mask.coordinates[i, 0];
        //    int x_mask_end = mask.coordinates[i, 1];
        //    int y_mask = mask.y_eq_0_index - i; // because mask.y0_index is also the first y coordinate

        //    for (int x_mask = x_mask_start; x_mask <= x_mask_end; x_mask++)
        //    {
        //        int x_canvas = x_mask + maskPosition.x;
        //        int y_canvas = y_mask + maskPosition.y;
        //        // if the canvas coordinated are out of range, we MUST NOT emit paint from the reservoir
        //        if (oilPaintSurface.IsInBounds(x_canvas, y_canvas))
        //        {
        //            Vector2Int coord_mask = new Vector2Int(x_mask, y_mask);
        //            Vector2Int coord_mask_reservoir_aligned = MathUtil.RotateAroundOrigin(coord_mask, -maskAngle);
        //            Vector2Int coord_reservoir = coord_mask_reservoir_aligned
        //                                       + new Vector2Int(paintReservoir.Width - 1, paintReservoir.Height / 2); // TODO duplicate code

        //            Color emittedFromReservoir = paintReservoir.Emit(coord_reservoir.x, coord_reservoir.y);
        //            oilPaintSurface.AddPaint(x_canvas, y_canvas, emittedFromReservoir);
        //        }
        //    }
        //});

        oilPaintSurface.Apply();
    }
}
