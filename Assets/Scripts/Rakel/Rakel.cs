using System;
using System.Diagnostics;
using UnityEngine;

public abstract class Rakel
{
    public int Length { get; protected set; }
    public int Width { get; protected set; }
    protected Vector2 Normal;
    protected bool RecalculateMask;

    protected Vector2Int Position;
    protected bool ReapplyMask;

    public void UpdateNormal(Vector2 normal)
    {
        if (!normal.Equals(Normal))
        {
            Normal = normal;
            RecalculateMask = true;
        }
    }

    public void UpdatePosition(Vector2Int position)
    {
        if (!position.Equals(Position))
        {
            Position = position;
            ReapplyMask = true;
        }
    }

    public void ApplyToCanvas(IOilPaintSurface oilPaintSurface, bool logTime = false)
    {
        if (RecalculateMask)
        {
            RecalculateMask = false; // reset
            ReapplyMask = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            CalculateMask();
            if (logTime)
                UnityEngine.Debug.Log("mask calc took " + sw.ElapsedMilliseconds + "ms");
        }

        if (ReapplyMask)
        {
            ReapplyMask = false;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            ApplyMask(oilPaintSurface);
            if (logTime)
                UnityEngine.Debug.Log("mask apply took " + sw.ElapsedMilliseconds + "ms");
        }
    }

    protected abstract void CalculateMask();

    protected abstract void ApplyMask(IOilPaintSurface oilPaintSurface);
}
