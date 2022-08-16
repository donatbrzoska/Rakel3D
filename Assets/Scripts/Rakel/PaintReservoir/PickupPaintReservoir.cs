using System;
using System.Collections.Generic;
using UnityEngine;

public class PickupPaintReservoir
{
    private Queue<Color>[,] colors;

    public PickupPaintReservoir(int height, int width, int pickupDelay)
    {
        colors = new Queue<Color>[height, width];

        for (int i=0; i<colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                colors[i, j] = new Queue<Color>();
                for (int k = 0; k < pickupDelay; k++)
                {
                    colors[i, j].Enqueue(Colors.NO_PAINT_COLOR);
                }
            }
        }
    }

    public void Add(int x, int y, Color color, int volume)
    {
        for (int i = 0; i < volume; i++)
        {
            colors[y, x].Enqueue(color);
        }
    }

    public Color Emit(int x, int y)
    {
        if (colors[y, x].Count > 0)
        {
            return colors[y, x].Dequeue();
        }
        else
        {
            return Colors.NO_PAINT_COLOR;
        }
    }
}
