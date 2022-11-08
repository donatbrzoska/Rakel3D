using System;
using System.Collections.Generic;
using UnityEngine;

public class PaintReservoir
{
    public int Height { get; private set; }
    public int Width { get; private set; }

    private int[,] volumes;
    private Color[,] colors;

    public PaintReservoir(int height, int width)
    {
        Height = height;
        Width = width;
        volumes = new int[height, width];
        colors = new Color[height, width];
    }

    public void Set(int x, int y, Color color, int volume)
    {
        colors[y, x] = color;
        volumes[y, x] = volume;
    }

    public void Pickup(int x, int y, Color color, int volume)
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
