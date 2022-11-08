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

    public void Fill(Color color, int volume)
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                ApplicationReservoir.Set(j, i, color, volume);
            }
        }
    }

    public void Pickup(int x, int y, Color color, int volume)
    {
        PickupReservoir.Pickup(x, y, color, volume);
    }

    public Color Emit(int x, int y)
    {
        bool in_range = x >= 0
                        && x < Width
                        && y >= 0
                        && y < Height;
        if (in_range)
        {
            Color ar_col = ApplicationReservoir.Emit(x, y);
            bool ar_filled = !ar_col.Equals(Colors.NO_PAINT_COLOR);

            Color pr_col = PickupReservoir.Emit(x, y);
            bool pr_filled = !pr_col.Equals(Colors.NO_PAINT_COLOR);

            if (ar_filled && pr_filled)
            {
                return new Color(
                    (ar_col.r + pr_col.r) / 2,
                    (ar_col.g + pr_col.g) / 2,
                    (ar_col.b + pr_col.b) / 2,
                    (ar_col.a + pr_col.a) / 2
                );
            }
            else if (ar_filled)
            {
                return ar_col;
            }
            else if (pr_filled)
            {
                return pr_col;
            }
            else // could be removed if this was the default case for the entire function (if nothing else hits beforehand)
            {
                return Colors.NO_PAINT_COLOR;
            }
        }
        else
        {
            return Colors.NO_PAINT_COLOR;
        }
    }
}
