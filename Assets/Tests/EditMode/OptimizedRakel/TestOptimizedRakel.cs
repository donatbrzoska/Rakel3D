using System;
using NUnit.Framework;
using UnityEngine;

public class TestOptimizedRakel
{
    [Test]
    public void ApplyToCanvas_Point()
    {
        OilPaintTexture texture = new OilPaintTexture(3, 3);
        OptimizedRakel rakel = new OptimizedRakel(new OptimizedMaskCalculator(), new OptimizedMaskApplicator());
        rakel.UpdateLength(1);
        rakel.UpdateWidth(1);
        rakel.UpdateColor(new Color(0, 0.4f, 0.8f));
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.UpdateNormal(Vector2Int.right);

        rakel.ApplyToCanvas(texture);

        Color[] colors = texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1), new Color(1, 1, 1),       new Color(1, 1, 1),
                new Color(1, 1, 1), new Color(0, 0.4f, 0.8f), new Color(1, 1, 1),
                new Color(1, 1, 1), new Color(1, 1, 1),       new Color(1, 1, 1),
            },
            colors
        );
    }

    // TODO wrong usage (Apply before set values)
    // TODO don't pickup canvas color (currently white)
}
