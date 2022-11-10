using System;
using System.Collections.Generic;
using UnityEngine;

public class PaintReservoir
{
    public int Height { get; private set; }
    public int Width { get; private set; }

    private Paint[,] Reservoir;

    public PaintReservoir(int height, int width)
    {
        Height = height;
        Width = width;

        Reservoir = new Paint[height, width];
        for (int i=0; i < Reservoir.GetLength(0); i++)
        {
            for (int j = 0; j < Reservoir.GetLength(1); j++)
            {
                Reservoir[i, j] = new Paint(Colors.NO_PAINT_COLOR, 0);
            }
        }
    }

    public void Set(int x, int y, Paint paint)
    {
        Reservoir[y, x] = new Paint(paint);
    }

    public Paint Get(int x, int y)
    {
        if (IsInBounds(x, y))
        {
            return Reservoir[y, x];
        }
        else
        {
            return Paint.EMPTY_PAINT;
        }
    }

    public void Pickup(int x, int y, Paint paint)
    {
        Reservoir[y, x] += paint;
    }

    public Paint Emit(int x, int y, int volume)
    {
        if (IsInBounds(x, y))
        {
            if (Reservoir[y, x].Volume > 0)
            {
                Paint result;

                if (Reservoir[y, x].Volume <= volume)
                {
                    result = Reservoir[y, x];
                    Reservoir[y, x] = Paint.EMPTY_PAINT;
                }
                else
                {
                    result = new Paint(Reservoir[y, x].Color, volume);
                    Reservoir[y, x].Volume -= volume;
                }

                return result;
            }
            else
            {
                return Paint.EMPTY_PAINT;
            }
        }
        else
        {
            return Paint.EMPTY_PAINT;
        }
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0
            && x < Width
            && y >= 0
            && y < Height;
    }
}
