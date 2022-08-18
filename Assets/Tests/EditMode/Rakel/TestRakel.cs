using System;
using NUnit.Framework;
using UnityEngine;

class MaskCalculatorMock : LogMock, IMaskCalculator
{
    public Mask Calculate(int height, int width, Vector2 normal)
    {
        Log += "Calculate ";
        return new Mask();
    }
}

public class TestRakel
{
    FastTexture2D texture;
    OilPaintSurface oilPaintSurface;
    RakelPaintReservoir rakelPaintReservoir;
    Rakel rakel;

    [SetUp]
    public void Init()
    {
        texture = new FastTexture2D(3, 3);
        oilPaintSurface = new OilPaintSurface(texture);
        rakelPaintReservoir = new RakelPaintReservoir(1, 1, 0);
        rakel = new Rakel(1, 1, rakelPaintReservoir, oilPaintSurface, new MaskCalculator(), new MaskApplicator());
        rakel.UpdateNormal(Vector2Int.right);
    }

    // this is an integration test and should be moved maybe later though
    [Test]
    public void PointRakel_Point()
    {
        rakelPaintReservoir.Fill(new Color(0, 0.4f, 0.8f), 1);
        rakel.ApplyAt(new Vector2Int(1, 1));

        Color[] colors = texture.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR, new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR,
            },
            colors
        );
    }

    [Test]
    public void ApplyAt_AppliesMask()
    {
        MaskApplicatorMock ma_mock = new MaskApplicatorMock();
        Rakel rakel = new Rakel(1, 1, null, null, null, ma_mock);
        rakel.ApplyAt(new Vector2Int(0, 0));

        Assert.AreEqual(
            "Apply ",
            ma_mock.Log
        );
    }

    [Test]
    public void UpdateNormal_RecalculatesMask()
    {
        MaskCalculatorMock mc_mock = new MaskCalculatorMock();
        Rakel rakel = new Rakel(1, 1, null, null, mc_mock, null);
        rakel.UpdateNormal(Vector2Int.right);

        Assert.AreEqual(
            "Calculate ",
            mc_mock.Log
        );
    }

    [Test]
    public void UpdateNormal_RecalculatesMask_OnlyUponRealUpdate()
    {
        MaskCalculatorMock mc_mock = new MaskCalculatorMock();
        Rakel rakel = new Rakel(1, 1, null, null, mc_mock, null);
        rakel.UpdateNormal(Vector2Int.right);
        rakel.UpdateNormal(Vector2Int.right);

        Assert.AreEqual(
            "Calculate ",
            mc_mock.Log
        );

        rakel.UpdateNormal(Vector2Int.left);

        Assert.AreEqual(
            "Calculate Calculate ",
            mc_mock.Log
        );
    }
}
