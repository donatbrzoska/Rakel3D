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
            int newVolume = volumes[y, x] + volume;
            double addedColorPart = volume / (double)newVolume;
            double currentColorPart = volumes[y, x] / (double)newVolume;
            Color newColor = new Color(
                (float) (colors[y, x].r * currentColorPart + color.r * addedColorPart),
                (float) (colors[y, x].g * currentColorPart + color.g * addedColorPart),
                (float) (colors[y, x].b * currentColorPart + color.b * addedColorPart)
            );

            volumes[y, x] = newVolume;
            colors[y, x] = newColor;
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
