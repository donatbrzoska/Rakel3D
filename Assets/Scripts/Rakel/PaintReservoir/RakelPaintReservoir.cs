using System;
using UnityEngine;

public class RakelPaintReservoir : IRakelPaintReservoir
{
    public int Height { get; private set; }
    public int Width { get; private set; }
    public Vector2Int Pivot { get { return new Vector2Int(Width - 1, Height / 2); } }

    private PaintReservoir PickupReservoir;
    private PaintReservoir ApplicationReservoir;

    public RakelPaintReservoir(int height, int width)
    {
        Height = height;
        Width = width;

        PickupReservoir = new PaintReservoir(height, width);
        ApplicationReservoir = new PaintReservoir(height, width);
    }

    public void Fill(Paint paint)
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                ApplicationReservoir.Set(j, i, paint);
            }
        }
    }

    public void Pickup(int x, int y, Paint paint)
    {
        PickupReservoir.Pickup(x, y, paint);
    }

    public Paint Emit(int x, int y, int applicationReservoirVolume, int pickupReservoirVolume)
    {
        Paint ar_paint = ApplicationReservoir.Emit(x, y, applicationReservoirVolume);
        Paint pr_paint = PickupReservoir.Emit(x, y, pickupReservoirVolume);

        return ar_paint + pr_paint;
    }
}
