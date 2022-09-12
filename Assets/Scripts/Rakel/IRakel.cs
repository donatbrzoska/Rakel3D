using System;
using UnityEngine;

public interface IRakel
{
    public int Length { get; }
    public int Width { get; }
    public void UpdateNormal(Vector2 normal, bool logMaskCalcTime = false);
    public void ApplyAt(IOilPaintSurface oilPaintSurface, Vector2Int position, bool logMaskApplyTime = false);
}
