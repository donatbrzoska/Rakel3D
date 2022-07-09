using System;

public class BasicRakel: Rakel
{
    private bool[,] LatestMask;
    private BasicMaskCalculator MaskCalculator;
    private BasicMaskApplicator MaskApplicator;

    public BasicRakel(BasicMaskCalculator rectangleCalculator, BasicMaskApplicator maskApplicator)
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
