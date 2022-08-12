using System;
using UnityEngine;

public class PickupPaintReservoir: PaintReservoir
{
    public PickupPaintReservoir(int height, int width) : base(height, width)
    {
    }

    // TODO this is identical to the Set, because without color layering it doesn't make any sense
    public void Add(int x, int y, Color color, int volume)
    {
        colors[y, x] = color; // TODO this doesn't make any sense without color layering
        volumes[y, x] = volume;
    }
}
