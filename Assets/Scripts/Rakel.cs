using System;
using System.Diagnostics;
using UnityEngine;

public class Rakel
{
    private Color Color;

    private int Length;
    private int Width;
    private Vector2 Normal;
    private bool RecalculateMask;
    private BasicRectangleCalculator RectangleCalculator;
    private bool[,] LatestMask;

    private Vector2Int Position;
    private bool ReapplyMask;
    private MaskApplicator MaskApplicator;

    public Rakel(BasicRectangleCalculator rectangleCalculator, MaskApplicator maskApplicator)
    {
        RectangleCalculator = rectangleCalculator;
        MaskApplicator = maskApplicator;
    }

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
        this.Color = color;
    }

    public void UpdateNormal(Vector2 normal)
    {
        this.Normal = normal;
        RecalculateMask = true;
    }

    public void UpdatePosition(Vector2Int position)
    {
        this.Position = position;
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
            LatestMask = RectangleCalculator.Calculate(Length, Width, Normal);
            //UnityEngine.Debug.Log("mask calc took " + sw.ElapsedMilliseconds + "ms");
        }

        if (ReapplyMask)
        {
            ReapplyMask = false;
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            MaskApplicator.Apply(LatestMask, Position, texture, Color);
            //UnityEngine.Debug.Log("mask apply took " + sw.ElapsedMilliseconds + "ms");
        }
    }
}
