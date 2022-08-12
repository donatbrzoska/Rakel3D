using System;
using System.Diagnostics;
using UnityEngine;

public class Rakel
{
    public int Length { get; protected set; }
    public int Width { get; protected set; }
    protected Vector2 Normal;
    protected bool RecalculateMask;

    protected Vector2Int Position;
    protected bool ReapplyMask;

    private Mask LatestMask;
    private IMaskCalculator MaskCalculator;
    private IMaskApplicator MaskApplicator;

    private RakelPaintReservoir PaintReservoir;

    public Rakel(int length, int width, IMaskCalculator maskCalculator, IMaskApplicator maskApplicator)
    {
        Length = length;
        Width = width;
        MaskCalculator = maskCalculator;
        MaskApplicator = maskApplicator;
        PaintReservoir = new RakelPaintReservoir(Length, Width);
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

    public void UpdatePaint(Color color, int volume)
    {
        PaintReservoir.Fill(color, volume);
    }

    public void ApplyToCanvas(IOilPaintSurface oilPaintSurface, bool logTime = false)
    {
        if (RecalculateMask)
        {
            RecalculateMask = false; // reset
            ReapplyMask = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            LatestMask = MaskCalculator.Calculate(Length, Width, Normal);
            if (logTime)
                UnityEngine.Debug.Log("mask calc took " + sw.ElapsedMilliseconds + "ms");
        }

        if (ReapplyMask)
        {
            ReapplyMask = false;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            MaskApplicator.Apply(LatestMask, Position, Normal, oilPaintSurface, PaintReservoir);
            if (logTime)
                UnityEngine.Debug.Log("mask apply took " + sw.ElapsedMilliseconds + "ms");
        }
    }
}
