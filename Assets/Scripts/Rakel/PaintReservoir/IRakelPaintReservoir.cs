using System;
using UnityEngine;

public interface IRakelPaintReservoir
{
    public int Height { get; }
    public int Width { get; }

    public void Fill(Color color, int volume);
    public void Pickup(int x, int y, Color color, int volume);
    public Color Emit(int x, int y);
}
