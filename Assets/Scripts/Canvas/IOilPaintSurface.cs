using System;
using UnityEngine;

public interface IOilPaintSurface
{
    public bool IsInBounds(int x, int y);
    public void AddPaint(int x, int y, Color color);
    public Color GetPaint(int x, int y);
    public void Apply();
}