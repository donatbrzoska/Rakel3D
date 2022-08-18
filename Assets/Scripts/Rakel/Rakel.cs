using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Rakel : IRakel
{
    public int Length { get; private set; }
    public int Width { get; private set; }
    private Vector2 Normal;

    private Mask LatestMask;
    private IMaskCalculator MaskCalculator;
    private IMaskApplicator MaskApplicator;

    private IOilPaintSurface OilPaintSurface;
    private RakelPaintReservoir PaintReservoir;

    public Rakel(
        int length,
        int width,
        int pickupDelay,
        IOilPaintSurface oilPaintSurface,
        IMaskCalculator maskCalculator,
        IMaskApplicator maskApplicator)
    {
        Length = length;
        Width = width;
        OilPaintSurface = oilPaintSurface;
        MaskCalculator = maskCalculator;
        MaskApplicator = maskApplicator;

        PaintReservoir = new RakelPaintReservoir(Length, Width, pickupDelay);
    }

    public void UpdatePaint(Color color, int volume)
    {
        PaintReservoir.Fill(color, volume);
    }

    public void UpdateNormal(Vector2 normal, bool logMaskCalcTime = false)
    {
        if (!normal.Equals(Normal))
        {
            Normal = normal;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            LatestMask = MaskCalculator.Calculate(Length, Width, Normal);

            if (logMaskCalcTime)
                UnityEngine.Debug.Log("mask calc took " + sw.ElapsedMilliseconds + "ms");
        }
    }

    public void ApplyAt(Vector2Int position, bool logMaskApplyTime = false)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        MaskApplicator.Apply(LatestMask, position, Normal, OilPaintSurface, PaintReservoir);

        if (logMaskApplyTime)
            UnityEngine.Debug.Log("mask apply took " + sw.ElapsedMilliseconds + "ms");
    }
}
