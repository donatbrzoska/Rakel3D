using System;
using UnityEngine;

public interface IRakel
{
    public int Length { get; }
    public int Width { get; }
    public void UpdatePaint(Color color, int volume);
    public void UpdateNormal(Vector2 normal, bool logMaskCalcTime = false);
    public void ApplyAt(Vector2Int position, bool logMaskApplyTime = false);
}
