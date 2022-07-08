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
    private BasicMaskCalculator MaskCalculator;
    private bool[,] LatestBasicMask;

    protected Vector2Int Position;
    private bool ReapplyMask;
    private BasicMaskApplicator MaskApplicator;

    public Rakel(BasicMaskCalculator rectangleCalculator, BasicMaskApplicator maskApplicator)
    {
        MaskCalculator = rectangleCalculator;
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

    protected virtual void CalculateMask()
    {
        LatestBasicMask = MaskCalculator.Calculate(Length, Width, Normal);
    }

    protected virtual void ApplyMask(OilPaintTexture texture)
    {
        MaskApplicator.Apply(LatestBasicMask, Position, texture, Color);
    }
}
