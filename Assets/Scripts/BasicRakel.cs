using System;

public class BasicRakel: Rakel
{
    private bool[,] LatestBasicMask;
    private BasicMaskCalculator MaskCalculator;
    private BasicMaskApplicator MaskApplicator;

    public BasicRakel(BasicMaskCalculator rectangleCalculator, BasicMaskApplicator maskApplicator)
    {
        MaskCalculator = rectangleCalculator;
        MaskApplicator = maskApplicator;
    }

    protected override void CalculateMask()
    {
        LatestBasicMask = MaskCalculator.Calculate(Length, Width, Normal);
    }

    protected override void ApplyMask(OilPaintTexture texture)
    {
        MaskApplicator.Apply(LatestBasicMask, Position, texture, Color);
    }
}
