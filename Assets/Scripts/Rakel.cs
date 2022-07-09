using System;
using System.Diagnostics;
using UnityEngine;

public abstract class Rakel
{
    protected Color Color;

    public int Length { get; private set; }
    public int Width { get; private set; }
    protected Vector2 Normal;
    protected bool RecalculateMask;

    protected Vector2Int Position;
    protected bool ReapplyMask;

    public void UpdateLength(int length)
    {
        if (length != Length)
        {
            // TODO always keep uneven so there is a center to rotate around
            Length = length;
            RecalculateMask = true;
        }
    }

    public void UpdateWidth(int width)
    {
        if (width != Width)
        {
            Width = width;
            RecalculateMask = true;
        }
    }

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

    public void UpdateColor(Color color)
    {
        Color = color;
    }

    public void ApplyToCanvas(OilPaintTexture texture, bool logTime = false)
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
            ApplyMask(texture);
            if (logTime)
                UnityEngine.Debug.Log("mask apply took " + sw.ElapsedMilliseconds + "ms");
        }
    }

    protected abstract void CalculateMask();

    protected abstract void ApplyMask(OilPaintTexture texture);
}
