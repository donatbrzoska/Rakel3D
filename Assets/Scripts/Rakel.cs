using System;
using System.Diagnostics;
using UnityEngine;

public abstract class Rakel
{
    protected Color Color;

    protected int Length;
    protected int Width;
    protected Vector2 Normal;
    protected bool RecalculateMask;

    protected Vector2Int Position;
    protected bool ReapplyMask;

    public void UpdateLength(int length)
    {
        // TODO always keep uneven so there is a center to rotate around
        Length = length;
        RecalculateMask = true;
    }

    public void UpdateWidth(int width)
    {
        Width = width;
        RecalculateMask = true;
    }

    public void UpdateColor(Color color)
    {
        Color = color;
    }

    public void UpdateNormal(Vector2 normal)
    {
        Normal = normal;
        RecalculateMask = true;
    }

    public void UpdatePosition(Vector2Int position)
    {
        Position = position;
        ReapplyMask = true;
    }

    public void ApplyToCanvas(OilPaintTexture texture)
    {
        if (RecalculateMask)
        {
            RecalculateMask = false; // reset
            ReapplyMask = true;

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            CalculateMask();
            //UnityEngine.Debug.Log("mask calc took " + sw.ElapsedMilliseconds + "ms");
        }

        if (ReapplyMask)
        {
            ReapplyMask = false;
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            ApplyMask(texture);
            //UnityEngine.Debug.Log("mask apply took " + sw.ElapsedMilliseconds + "ms");
        }
    }

    protected abstract void CalculateMask();

    protected abstract void ApplyMask(OilPaintTexture texture);
}
