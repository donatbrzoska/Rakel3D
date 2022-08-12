using System;
using UnityEngine;

public class OptimizedRakel: Rakel
{
    private OptimizedMask LatestMask;
    private OptimizedMaskCalculator MaskCalculator;
    private OptimizedMaskApplicator MaskApplicator;

    private RakelPaintReservoir PaintReservoir;

    public OptimizedRakel(int length, int width, OptimizedMaskCalculator maskCalculator, OptimizedMaskApplicator maskApplicator)
    {
        Length = length;
        Width = width;
        MaskCalculator = maskCalculator;
        MaskApplicator = maskApplicator;
        PaintReservoir = new RakelPaintReservoir(Length, Width);
    }

    protected override void CalculateMask()
    {
        LatestMask = MaskCalculator.Calculate(Length, Width, Normal);
    }

    protected override void ApplyMask(IOilPaintSurface oilPaintSurface)
    {
        MaskApplicator.Apply(LatestMask, Position, Normal, oilPaintSurface, PaintReservoir);
    }

    public void UpdatePaint(Color color, int volume)
    {
        PaintReservoir.Fill(color, volume);
    }
}
