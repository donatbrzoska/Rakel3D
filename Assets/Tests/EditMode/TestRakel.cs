using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestRakel
{
    [Test]
    public void Apply_Point()
    {
        OilPaintTexture t = new OilPaintTexture(3, 3);
        Rakel r = new Rakel(1, 1);

        r.UpdateColor(new Color(0, 0.4f, 0.8f));
        r.UpdatePosition(new Vector2Int(1, 1));
        r.UpdateNormal(Vector2Int.right);
        r.ApplyToCanvas(t);

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
        OilPaintTexture t = new OilPaintTexture(3, 3);
        Rakel r = new Rakel(3, 2);

        r.UpdateColor(new Color(0, 0.4f, 0.8f));
        r.UpdatePosition(new Vector2Int(2, 1));
        r.UpdateNormal(Vector2Int.right);
        r.ApplyToCanvas(t);

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
        OilPaintTexture t = new OilPaintTexture(3, 3);
        Rakel r = new Rakel(3, 2);

        r.UpdateColor(new Color(0, 0.4f, 0.8f));
        r.UpdatePosition(new Vector2Int(1, 1));
        r.UpdateNormal(Vector2Int.down);
        r.ApplyToCanvas(t);

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
        OilPaintTexture t = new OilPaintTexture(3, 6);
        Rakel r = new Rakel(3, 2);

        r.UpdateColor(new Color(0, 0.4f, 0.8f));
        r.UpdatePosition(new Vector2Int(1, 4));
        r.UpdateNormal(Vector2Int.down);
        r.ApplyToCanvas(t);

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

    // TODO wrong usage (Apply before set values)
    // TODO don't pickup canvas color (currently white)

}