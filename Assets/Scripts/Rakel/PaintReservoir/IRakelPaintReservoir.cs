using System;
using UnityEngine;

public interface IRakelPaintReservoir
{
    public int Height { get; }
    public int Width { get; }
    public Vector2Int Pivot { get; }

    public void Fill(Paint paint);
    public void Pickup(int x, int y, Paint paint);
    public Paint Emit(int x, int y, int applicationReservoirVolume, int pickupReservoirVolume);
}
