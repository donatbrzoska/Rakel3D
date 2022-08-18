using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Rakel : IRakel
{
    public int Length { get; private set; }
    public int Width { get; private set; }
    private Vector2 PreviousNormal;

    private Mask LatestMask;
    private IMaskCalculator MaskCalculator;
    private IMaskApplicator MaskApplicator;

    private IOilPaintSurface OilPaintSurface;
    private IRakelPaintReservoir PaintReservoir;

    public Rakel(
        int length,
        int width,
        IRakelPaintReservoir paintReservoir,
        IOilPaintSurface oilPaintSurface,
        IMaskCalculator maskCalculator,
        IMaskApplicator maskApplicator)
    {
        Length = length;
        Width = width;
        PaintReservoir = paintReservoir;
        OilPaintSurface = oilPaintSurface;
        MaskCalculator = maskCalculator;
        MaskApplicator = maskApplicator;
    }

    public void UpdateNormal(Vector2 normal, bool logMaskCalcTime = false)
    {
        if (!normal.Equals(PreviousNormal))
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            LatestMask = MaskCalculator.Calculate(Length, Width, PreviousNormal);

            if (logMaskCalcTime)
                UnityEngine.Debug.Log("mask calc took " + sw.ElapsedMilliseconds + "ms");

            PreviousNormal = normal;
        }
    }

    public void ApplyAt(Vector2Int position, bool logMaskApplyTime = false)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        MaskApplicator.Apply(LatestMask, position, OilPaintSurface, PaintReservoir);

        if (logMaskApplyTime)
            UnityEngine.Debug.Log("mask apply took " + sw.ElapsedMilliseconds + "ms");
    }
}
