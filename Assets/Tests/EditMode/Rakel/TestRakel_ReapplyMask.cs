using System;
using NUnit.Framework;
using UnityEngine;

public class TestRakel_ReapplyMask
{
    OilPaintSurface oilPaintSurface_null;
    Rakel rakel;
    MaskApplicatorMock ma_mock;

    [SetUp]
    public void Init()
    {
        oilPaintSurface_null = null; // real object is not needed, since MaskApplicator is mocked hence not working on it
        ma_mock = new MaskApplicatorMock();
        rakel = new Rakel(1, 1, 0, new MaskCalculator(), ma_mock);
        rakel.UpdatePaint(new Color(0, 0.4f, 0.8f), 1);
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.UpdateNormal(Vector2Int.right);
    }

    [Test]
    public void ApplyToCanvas_ReapplyMask_UpdateNormal()
    {
        // initial value set
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> reapply
        rakel.UpdateNormal(new Vector2(0, 0));
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // make fake change -> no reapply
        rakel.UpdateNormal(new Vector2(0, 0));
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }

    [Test]
    public void ApplyToCanvas_ReapplyMask_UpdatePosition()
    {
        // initial value set
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> reapply
        rakel.UpdatePosition(new Vector2Int(0, 0));
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // make fake change -> no reapply
        rakel.UpdatePosition(new Vector2Int(0, 0));
        rakel.ApplyToCanvas(oilPaintSurface_null);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }
}
