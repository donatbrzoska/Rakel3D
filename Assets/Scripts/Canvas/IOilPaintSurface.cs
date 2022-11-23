using System;
using UnityEngine;

public interface IOilPaintSurface
{
    public bool IsInBounds(int x, int y);
    public void AddPaint(int x, int y, Paint paint);
    public Paint GetPaint(int x, int y, int volume);
    public void UpdateNormal(int x, int y);
    public void Apply();
}