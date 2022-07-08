using System;
using NUnit.Framework;
using UnityEngine;

class BasicMaskApplicatorMock : BasicMaskApplicator
{
    public string Log { get; private set; }
    public override void Apply(bool[,] mask, Vector2Int maskPosition, OilPaintTexture texture, Color color)
    {
        Log += "Apply ";
    }
}

public class TestRakel_ReapplyMask
{
    OilPaintTexture texture;
    Rakel rakel;
    BasicMaskApplicatorMock ma_mock;

    [SetUp]
    public void Init()
    {
        texture = new OilPaintTexture(3, 3);
        ma_mock = new BasicMaskApplicatorMock();
        rakel = new Rakel(new BasicMaskCalculator(), ma_mock);
        rakel.UpdateLength(1);
        rakel.UpdateWidth(1);
        rakel.UpdateColor(new Color(0, 0.4f, 0.8f));
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.UpdateNormal(Vector2Int.right);
    }

    [Test]
    public void ApplyToCanvas_ReapplyMask_UpdateWidth()
    {
        // initial value set
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> recalculate
        rakel.UpdateWidth(2);
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }

    [Test]
    public void ApplyToCanvas_ReapplyMask_UpdateLength()
    {
        // initial value set
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> recalculate
        rakel.UpdateLength(2);
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }

    [Test]
    public void ApplyToCanvas_ReapplyMask_UpdateNormal()
    {
        // initial value set
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> recalculate
        rakel.UpdateNormal(new Vector2(0, 0));
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }

    [Test]
    public void ApplyToCanvas_ReapplyMask_UpdatePosition()
    {
        // initial value set
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> recalculate
        rakel.UpdatePosition(new Vector2Int(0, 0));
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }
}
