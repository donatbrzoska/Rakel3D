using System;
using NUnit.Framework;
using UnityEngine;

class BasicMaskCalculatorMock : BasicMaskCalculator
{
    public string Log { get; private set; }
    public override bool[,] Calculate(int height, int width, Vector2 normal)
    {
        Log += "Calculate ";
        return new bool[1, 1] { { true } };
    }
}

public class TestRakel_RecalculateMask
{
    OilPaintTexture texture;
    Rakel rakel;
    BasicMaskCalculatorMock brc_mock;

    [SetUp]
    public void Init()
    {
        texture = new OilPaintTexture(3, 3);
        brc_mock = new BasicMaskCalculatorMock();
        rakel = new BasicRakel(brc_mock, new BasicMaskApplicator()); // this could be any concrete Rakel
        rakel.UpdateLength(1);
        rakel.UpdateWidth(1);
        rakel.UpdateColor(new Color(0, 0.4f, 0.8f));
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.UpdateNormal(Vector2Int.right);
    }

    [Test]
    public void ApplyToCanvas_RecalculateMask_UpdateLength()
    {
        // initial value set
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // make change -> recalculate
        rakel.UpdateLength(2);
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);
    }

    [Test]
    public void ApplyToCanvas_RecalculateMask_UpdateWidth()
    {
        // initial value set
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // make change -> recalculate
        rakel.UpdateWidth(2);
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);
    }

    [Test]
    public void ApplyToCanvas_RecalculateMask_UpdateNormal()
    {
        // initial value set
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // make change -> recalculate
        rakel.UpdateNormal(Vector2.left);
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        rakel.ApplyToCanvas(texture);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);
    }
}