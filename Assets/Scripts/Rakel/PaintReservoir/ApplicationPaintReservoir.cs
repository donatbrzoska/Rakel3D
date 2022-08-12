using System;
using UnityEngine;

public class ApplicationPaintReservoir: PaintReservoir
{
    public ApplicationPaintReservoir(int height, int width): base(height, width)
    {
    }

    public void Set(int x, int y, Color color, int volume)
    {
        colors[y, x] = color;
        volumes[y, x] = volume;
    }
}
