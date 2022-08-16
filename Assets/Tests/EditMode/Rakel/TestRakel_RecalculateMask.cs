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

public class TestRakel_RecalculateMask
{
    OilPaintSurface oilPaintSurface_null;
    Rakel rakel;
    MaskCalculatorMock brc_mock;

    [SetUp]
    public void Init()
    {
        oilPaintSurface_null = null; // real object is not needed, since MaskApplicator is mocked hence not working on it
        brc_mock = new MaskCalculatorMock();
        rakel = new Rakel(1, 1, 0, brc_mock, new MaskApplicatorMock());
        rakel.UpdatePaint(new Color(0, 0.4f, 0.8f), 1);
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.UpdateNormal(Vector2Int.right);
    }

    [Test]
    public void ApplyToCanvas_RecalculateMask_UpdateNormal()
    {
        // initial value set
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // make change -> recalculate
        rakel.UpdateNormal(Vector2.left);
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);

        // make fake change -> recalculate
        rakel.UpdateNormal(Vector2.left);
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);
    }
}