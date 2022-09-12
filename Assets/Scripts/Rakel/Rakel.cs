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

    private IRakelPaintReservoir PaintReservoir;

    public Rakel(
        int length,
        int width,
        IRakelPaintReservoir paintReservoir,
        IMaskCalculator maskCalculator,
        IMaskApplicator maskApplicator)
    {
        Length = length;
        Width = width;
        PaintReservoir = paintReservoir;
        MaskCalculator = maskCalculator;
        MaskApplicator = maskApplicator;
    }

    public void UpdateNormal(Vector2 normal, bool logMaskCalcTime = false)
    {
        if (!normal.Equals(PreviousNormal))
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            LatestMask = MaskCalculator.Calculate(Length, Width, normal);

            if (logMaskCalcTime)
            {
                double ns = 1000000000.0 * (double)sw.ElapsedTicks / Stopwatch.Frequency;
                //UnityEngine.Debug.Log("mask calc took " + ns / 1000000.0 + "ms");
                UnityEngine.Debug.Log("mask calc took " + ns / 1000.0 + "us");
            }

            PreviousNormal = normal;
        }
    }

    public void ApplyAt(IOilPaintSurface oilPaintSurface, Vector2Int position, bool logMaskApplyTime = false)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        MaskApplicator.Apply(LatestMask, position, oilPaintSurface, PaintReservoir);

        if (logMaskApplyTime)
            UnityEngine.Debug.Log("mask apply took " + sw.ElapsedMilliseconds + "ms");
    }
}
