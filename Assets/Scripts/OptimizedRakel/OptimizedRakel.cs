using System;
using UnityEngine;

public class OptimizedRakel: Rakel
{
    private OptimizedMask LatestMask;
    private OptimizedMaskCalculator MaskCalculator;
    private OptimizedMaskApplicator MaskApplicator;

    public OptimizedRakel(OptimizedMaskCalculator rectangleCalculator, OptimizedMaskApplicator maskApplicator)
    {
        MaskCalculator = rectangleCalculator;
        MaskApplicator = maskApplicator;
    }

    protected override void CalculateMask()
    {
        LatestMask = MaskCalculator.Calculate(Length, Width, Normal);
    }

    protected override void ApplyMask(OilPaintTexture texture)
    {
        MaskApplicator.Apply(LatestMask, Position, texture, Color);
    }
}
