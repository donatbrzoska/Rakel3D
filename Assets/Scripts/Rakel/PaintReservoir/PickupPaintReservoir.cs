using System;
using System.Collections.Generic;
using UnityEngine;

public class PickupPaintReservoir
{
    private int[,] volumes;
    private Color[,] colors;

    public PickupPaintReservoir(int height, int width)
    {
        volumes = new int[height, width];
        colors = new Color[height, width];
    }

    public void Add(int x, int y, Color color, int volume)
    {
        if (!color.Equals(Colors.NO_PAINT_COLOR))
        {
            volumes[y, x] += volume;
            colors[y, x] = color;
        }
    }

    public Color Emit(int x, int y)
    {
        if (volumes[y, x] > 0)
        {
            volumes[y, x] -= 1;
            return colors[y, x];
        }
        else
        {
            return Colors.NO_PAINT_COLOR;
        }
    }
}
