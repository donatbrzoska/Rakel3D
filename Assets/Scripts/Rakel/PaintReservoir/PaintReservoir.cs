using System;
using UnityEngine;

public class PaintReservoir
{
    protected Color[,] colors;
    protected int[,] volumes;

    public PaintReservoir(int height, int width)
    {
        colors = new Color[height, width];
        volumes = new int[height, width];
    }

    public Color Emit(int x, int y)
    {
        volumes[y, x]--;
        if (volumes[y, x] > -1)
        {
            return colors[y, x];
        }
        else
        {
            return Colors.NO_PAINT_COLOR;
        }
    }
}
