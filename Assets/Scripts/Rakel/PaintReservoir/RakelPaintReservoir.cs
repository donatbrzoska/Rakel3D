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
        int max_added_volume = paint.Volume;

        float scale = 10f;
        float offset_x = UnityEngine.Random.Range(0f, 1000f);
        float offset_y = UnityEngine.Random.Range(0f, 1000f);

        float[,] added_volumes = new float[Height, Width];
        float added_volumes_min = int.MaxValue;

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                float x = (float)j / Width * scale + offset_x;
                float y = (float)i / Height * scale + offset_y;

                float clipped_noise = Math.Max(Math.Min(Mathf.PerlinNoise(x, y), 1), 0);
                float added_volume = clipped_noise * max_added_volume;

                added_volumes[i, j] = added_volume;

                if (added_volume < added_volumes_min)
                    added_volumes_min = added_volume;
            }
        }

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Paint actual = new Paint(paint);
                actual.Volume += (int)(Mathf.Pow(added_volumes[i, j] - added_volumes_min, 2) / 2);
                //actual.Volume += (int) (added_volumes[i, j] - added_volumes_min);

                ApplicationReservoir.Set(j, i, actual);
            }
        }

        //for (int i = 0; i < Height; i++)
        //{
        //    for (int j = 0; j < Width; j++)
        //    {
        //        ApplicationReservoir.Set(j, i, paint);
        //    }
        //}
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
