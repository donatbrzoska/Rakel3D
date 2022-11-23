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
    [Test]
    public void ApplyAt_AppliesMask()
    {
        MaskApplicatorMock ma_mock = new MaskApplicatorMock();
        Rakel rakel = new Rakel(1, 1, null, null, ma_mock);
        rakel.ApplyAt(null, new Vector2Int(0, 0));

        Assert.AreEqual(
            "Apply ",
            ma_mock.Log
        );
    }

    // This only brings problems, when trying to test for Apply calls only and not injecting a mask calculator.
    // Rakel is never actually used in this way anyways
    //// This is an integration test and should be moved maybe later though.
    //// Testing the actual result of another ApplyAt is necessary to make sure,
    //// the new mask is based on the new normal and also stored
    //// HACK This test case overrides the paint reservoir and rakel setup from Init()
    //[Test]
    //public void DefaultNormal()
    //{
    //    rakelPaintReservoir = new RakelPaintReservoir(3, 1, 0);
    //    rakelPaintReservoir.Fill(new Color(0, 0.4f, 0.8f), 1);
    //    rakel = new Rakel(3, 1, rakelPaintReservoir, oilPaintSurface, new MaskCalculator(), new MaskApplicator());
    //    rakel.ApplyAt(new Vector2Int(1, 1));

    //    Color[] colors = texture.Texture.GetPixels();
    //    AssertUtil.AssertColorsAreEqual(
    //        new Color[]
    //        {
    //            Colors.CANVAS_COLOR, new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
    //            Colors.CANVAS_COLOR, new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
    //            Colors.CANVAS_COLOR, new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
    //        },
    //        colors
    //    );
    //}

    // This is an integration test and should be moved maybe later though.
    // Testing the actual result of another ApplyAt is necessary to make sure,
    // the new mask is based on the new normal and also stored
    [Test]
    public void UpdateNormal_RecalculatesMask()
    {
        IFastTexture2D texture = new FastTexture2D(3, 3);
        IFastTexture2D normalMap = new FastTexture2D(3, 3);
        IOilPaintSurface oilPaintSurface = new OilPaintSurface(texture, normalMap);

        IRakelPaintReservoir rakelPaintReservoir = new RakelPaintReservoir(3, 1);
        rakelPaintReservoir.Fill(new Paint(new Color(0, 0.4f, 0.8f), 1));

        IRakel rakel = new Rakel(3, 1, rakelPaintReservoir, new MaskCalculator(), new MaskApplicator());
        rakel.UpdateNormal(Vector2.left); // make sure that not only the first update is considered
        rakel.UpdateNormal(Vector2.down);
        rakel.ApplyAt(oilPaintSurface, new Vector2Int(1, 1));

        Color[] colors = texture.Texture.GetPixels();
        //LogUtil.Log(colors, 3);
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR,
                new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
                Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR,
            },
            colors
        );
    }

    [Test]
    public void UpdateNormal_RecalculatesMask_OnlyUponRealUpdate()
    {
        MaskCalculatorMock mc_mock = new MaskCalculatorMock();
        Rakel rakel = new Rakel(1, 1, null, mc_mock, null);
        rakel.UpdateNormal(Vector2.right);
        rakel.UpdateNormal(Vector2.right);

        Assert.AreEqual(
            "Calculate ",
            mc_mock.Log
        );

        rakel.UpdateNormal(Vector2.down);

        Assert.AreEqual(
            "Calculate Calculate ",
            mc_mock.Log
        );
    }
}
