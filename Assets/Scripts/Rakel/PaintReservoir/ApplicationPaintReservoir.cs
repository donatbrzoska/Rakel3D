using System;
using UnityEngine;

public class ApplicationPaintReservoir
{
    private Color[,] colors;
    private int[,] volumes;

    public ApplicationPaintReservoir(int height, int width)
    {
        colors = new Color[height, width];
        volumes = new int[height, width];
    }

    public void Set(int x, int y, Color color, int volume)
    {
        colors[y, x] = color;
        volumes[y, x] = volume;
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
