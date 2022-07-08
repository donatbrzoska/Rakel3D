using System;
using NUnit.Framework;
using UnityEngine;

public class TestBasicMaskApplicator
{

    private Rakel ConstructRakel()
    {
        Rakel r = new Rakel(new BasicMaskCalculator(), new BasicMaskApplicator());
        return r;
    }

    private void InitializeWithDefaults(Rakel r)
    {
        r.UpdateLength(1);
        r.UpdateWidth(1);
        r.UpdateColor(new Color(0, 0.4f, 0.8f));
        r.UpdatePosition(new Vector2Int(1, 1));
        r.UpdateNormal(Vector2Int.right);
    }

    [Test]
    public void Apply_Point()
    {
        bool[,] mask = new BasicMaskCalculator().Calculate(1, 1, Vector2.down);
        Vector2Int maskPosition = new Vector2Int(1, 1);
        OilPaintTexture t = new OilPaintTexture(3, 3);
        BasicMaskApplicator bma = new();

        bma.Apply(mask, maskPosition, t, new Color(0, 0.4f, 0.8f));

        Color[] colors = t.GetPixels();
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

    [Test]
    public void Apply_Rectangle()
    {
        bool[,] mask = new BasicMaskCalculator().Calculate(3, 2, Vector2.right);
        Vector2Int maskPosition = new Vector2Int(2, 1);
        OilPaintTexture t = new OilPaintTexture(3, 3);
        BasicMaskApplicator bma = new();

        bma.Apply(mask, maskPosition, t, new Color(0, 0.4f, 0.8f));

        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
                new Color(1, 1, 1), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
                new Color(1, 1, 1), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
            },
            colors
        );
    }

    [Test]
    public void Apply_Rectangle_Rotated90_SymmetricCase()
    {
        bool[,] mask = new BasicMaskCalculator().Calculate(3, 2, Vector2.down);
        Vector2Int maskPosition = new Vector2Int(1, 1);
        OilPaintTexture t = new OilPaintTexture(3, 3);
        BasicMaskApplicator bma = new();

        bma.Apply(mask, maskPosition, t, new Color(0, 0.4f, 0.8f));

        Color[] colors = t.GetPixels();
        //LogUtil.Log(colors, 3);
        AssertUtil.AssertColorsAreEqual(
            // NOTE the written array representation is the real coordinate system but mirrored by the x axis!
            new Color[]
            {
                new Color(1, 1, 1),       new Color(1, 1, 1),       new Color(1, 1, 1),
                new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
                new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
            },
            colors
        );
    }

    [Test]
    public void Apply_Rectangle_Rotated90_LargerTextureAsymmetricCase()
    {
        bool[,] mask = new BasicMaskCalculator().Calculate(3, 2, Vector2.down);
        Vector2Int maskPosition = new Vector2Int(1, 4);
        OilPaintTexture t = new OilPaintTexture(3, 6);
        BasicMaskApplicator bma = new();

        bma.Apply(mask, maskPosition, t, new Color(0, 0.4f, 0.8f));

        Color[] colors = t.GetPixels();
        //LogUtil.Log(colors, 6);
        AssertUtil.AssertColorsAreEqual(
            // NOTE the written array representation is the real coordinate system but mirrored by the x axis!
            new Color[]
            {
                new Color(1, 1, 1),       new Color(1, 1, 1),       new Color(1, 1, 1),
                new Color(1, 1, 1),       new Color(1, 1, 1),       new Color(1, 1, 1),
                new Color(1, 1, 1),       new Color(1, 1, 1),       new Color(1, 1, 1),
                new Color(1, 1, 1),       new Color(1, 1, 1),       new Color(1, 1, 1),
                new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
                new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
            },
            colors
        );
    }
}