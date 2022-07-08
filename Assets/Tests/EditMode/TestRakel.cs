using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestRakel
{
    private Rakel ConstructRakel()
    {
        Rakel r = new Rakel(new BasicRectangleCalculator(), new MaskApplicator());
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
        OilPaintTexture t = new OilPaintTexture(3, 3);
        Rakel r = ConstructRakel();
        InitializeWithDefaults(r);

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
        Rakel r = ConstructRakel();
        InitializeWithDefaults(r);
        r.UpdateLength(3);
        r.UpdateWidth(2);
        r.UpdatePosition(new Vector2Int(2, 1));

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
        Rakel r = ConstructRakel();
        InitializeWithDefaults(r);
        r.UpdateLength(3);
        r.UpdateWidth(2);
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
        Rakel r = ConstructRakel();
        InitializeWithDefaults(r);
        r.UpdateLength(3);
        r.UpdateWidth(2);
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

    [Test]
    public void Apply_RecalculateMask_UpdateLength()
    {
        OilPaintTexture t = new OilPaintTexture(3, 3);
        BasicRectangleCalculatorMock brc_mock = new BasicRectangleCalculatorMock();
        Rakel r = new Rakel(brc_mock, new MaskApplicator());
        InitializeWithDefaults(r);

        // initial value set
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // make change -> recalculate
        r.UpdateLength(2);
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);
    }

    [Test]
    public void Apply_RecalculateMask_UpdateWidth()
    {
        OilPaintTexture t = new OilPaintTexture(3, 3);
        BasicRectangleCalculatorMock brc_mock = new BasicRectangleCalculatorMock();
        Rakel r = new Rakel(brc_mock, new MaskApplicator());
        InitializeWithDefaults(r);

        // initial value set
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // make change -> recalculate
        r.UpdateWidth(2);
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);
    }

    [Test]
    public void Apply_RecalculateMask_UpdateNormal()
    {
        OilPaintTexture t = new OilPaintTexture(3, 3);
        BasicRectangleCalculatorMock brc_mock = new BasicRectangleCalculatorMock();
        Rakel r = new Rakel(brc_mock, new MaskApplicator());
        InitializeWithDefaults(r);

        // initial value set
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate ", brc_mock.Log);

        // make change -> recalculate
        r.UpdateNormal(Vector2.left);
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);

        // nothing changed -> no recalculate
        r.ApplyToCanvas(t);
        Assert.AreEqual("Calculate Calculate ", brc_mock.Log);
    }

    [Test]
    public void Apply_ReapplyMask_UpdateWidth()
    {
        OilPaintTexture t = new OilPaintTexture(3, 3);
        MaskApplicatorMock ma_mock = new MaskApplicatorMock();
        Rakel r = new Rakel(new BasicRectangleCalculator(), ma_mock);
        InitializeWithDefaults(r);

        // initial value set
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> recalculate
        r.UpdateWidth(2);
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }

    [Test]
    public void Apply_ReapplyMask_UpdateLength()
    {
        OilPaintTexture t = new OilPaintTexture(3, 3);
        MaskApplicatorMock ma_mock = new MaskApplicatorMock();
        Rakel r = new Rakel(new BasicRectangleCalculator(), ma_mock);
        InitializeWithDefaults(r);

        // initial value set
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> recalculate
        r.UpdateLength(2);
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }

    [Test]
    public void Apply_ReapplyMask_UpdateNormal()
    {
        OilPaintTexture t = new OilPaintTexture(3, 3);
        MaskApplicatorMock ma_mock = new MaskApplicatorMock();
        Rakel r = new Rakel(new BasicRectangleCalculator(), ma_mock);
        InitializeWithDefaults(r);

        // initial value set
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> recalculate
        r.UpdateNormal(new Vector2(0, 0));
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }

    [Test]
    public void Apply_ReapplyMask_UpdatePosition()
    {
        OilPaintTexture t = new OilPaintTexture(3, 3);
        MaskApplicatorMock ma_mock = new MaskApplicatorMock();
        Rakel r = new Rakel(new BasicRectangleCalculator(), ma_mock);
        InitializeWithDefaults(r);

        // initial value set
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply ", ma_mock.Log);

        // make change -> recalculate
        r.UpdatePosition(new Vector2Int(0, 0));
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);

        // nothing changed -> no reapply
        r.ApplyToCanvas(t);
        Assert.AreEqual("Apply Apply ", ma_mock.Log);
    }

    // TODO wrong usage (Apply before set values)
    // TODO don't pickup canvas color (currently white)

}

class BasicRectangleCalculatorMock : BasicRectangleCalculator
{
    public string Log { get; private set; }
    public override bool[,] Calculate(int height, int width, Vector2 normal)
    {
        Log += "Calculate ";
        return new bool[1, 1] { { true } };
    }
}

class MaskApplicatorMock : MaskApplicator
{
    public string Log { get; private set; }
    public override void Apply(bool[,] mask, Vector2Int maskPosition, OilPaintTexture texture, Color color)
    {
        Log += "Apply ";
    }
}