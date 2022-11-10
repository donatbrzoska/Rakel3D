using System;
using NUnit.Framework;
using UnityEngine;

public class TestPaintReservoir
{
    [Test]
    public void Emit_Filled_EmitsCorrectColorVolume()
    {
        // use bigger reservoir to check for correct coordinate to array mapping also
        PaintReservoir paintReservoir = new PaintReservoir(2, 1);
        paintReservoir.Set(0, 1, new Paint(new Color(0.2f, 0.3f, 0.4f), 2));

        Paint emitted = paintReservoir.Emit(0, 1, 2);

        Assert.AreEqual(new Paint(new Color(0.2f, 0.3f, 0.4f), 2), emitted);
    }

    [Test]
    public void Emit_Empty_EmitsEmptyColor()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);

        Paint emitted = paintReservoir.Emit(0, 0, 1);

        Assert.AreEqual(Paint.EMPTY_PAINT, emitted);
    }

    // prevent problems with possibly negative volumes
    [Test]
    public void Emit_Empty_FollowedBySet_EmitsCorrectColor()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Emit(0, 0, 1);
        paintReservoir.Set(0, 0, new Paint(new Color(0.2f, 0.3f, 0.4f), 1));

        Paint emitted = paintReservoir.Emit(0, 0, 1);

        Assert.AreEqual(new Paint(new Color(0.2f, 0.3f, 0.4f), 1), emitted);
    }

    // prevent problems with possibly negative volumes
    [Test]
    public void Emit_Empty_FollowedByPickup_EmitsCorrectColor()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Emit(0, 0, 1);
        paintReservoir.Pickup(0, 0, new Paint(new Color(0.2f, 0.3f, 0.4f), 1));

        Paint emitted = paintReservoir.Emit(0, 0, 1);

        Assert.AreEqual(new Paint(new Color(0.2f, 0.3f, 0.4f), 1), emitted);
    }

    [Test]
    public void Emit_VolumeReduction_HasVolume()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Set(0, 0, new Paint(new Color(0.2f, 0.3f, 0.4f), 2));

        Paint emitted1 = paintReservoir.Emit(0, 0, 1);
        Paint emitted2 = paintReservoir.Emit(0, 0, 1);
        Paint emitted3 = paintReservoir.Emit(0, 0, 1);

        Assert.AreEqual(new Paint(new Color(0.2f, 0.3f, 0.4f), 1), emitted1);
        Assert.AreEqual(new Paint(new Color(0.2f, 0.3f, 0.4f), 1), emitted2);
        Assert.AreEqual(Paint.EMPTY_PAINT, emitted3);
    }

    [Test]
    public void Emit_VolumeReduction_HasVolumeButNotEnough()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Set(0, 0, new Paint(new Color(0.2f, 0.3f, 0.4f), 2));

        Paint emitted = paintReservoir.Emit(0, 0, 3);

        Assert.AreEqual(new Paint(new Color(0.2f, 0.3f, 0.4f), 2), emitted);
    }

    [Test]
    public void Set_EmptyPaint_SetsEmptyPaint()
    {
        // use bigger reservoir to check for correct coordinate to array mapping also
        PaintReservoir paintReservoir = new PaintReservoir(2, 1);
        paintReservoir.Set(0, 1, new Paint(new Color(0.4f, 0.5f, 0.6f), 1));

        paintReservoir.Set(0, 1, Paint.EMPTY_PAINT);

        Paint emitted = paintReservoir.Emit(0, 1, 10);
        Assert.AreEqual(Paint.EMPTY_PAINT, emitted);
    }

    [Test]
    public void Pickup_EmptyPaint_DoesNothing()
    {
        // use bigger reservoir to check for correct coordinate to array mapping also
        PaintReservoir paintReservoir = new PaintReservoir(2, 1);
        paintReservoir.Pickup(0, 1, new Paint(new Color(0.4f, 0.5f, 0.6f), 1));

        paintReservoir.Pickup(0, 1, Paint.EMPTY_PAINT);

        Paint emitted = paintReservoir.Emit(0, 1, 1);
        Assert.AreEqual(new Paint(new Color(0.4f, 0.5f, 0.6f), 1), emitted);
    }

    [Test]
    public void Pickup_EmptyReservoir()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);

        paintReservoir.Pickup(0, 0, new Paint(new Color(0.8f, 0.8f, 0.8f), 2));

        Paint emitted = paintReservoir.Emit(0, 0, 1);
        Assert.AreEqual(new Paint(new Color(0.8f, 0.8f, 0.8f), 1), emitted);
    }

    [Test]
    public void Pickup_FilledReservoir_ResultsInColorMixing()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Pickup(0, 0, new Paint(new Color(0.2f, 0.2f, 0.2f), 1));

        paintReservoir.Pickup(0, 0, new Paint(new Color(0.8f, 0.8f, 0.8f), 2));

        Paint emitted = paintReservoir.Emit(0, 0, 1);
        Assert.AreEqual(new Paint(new Color(0.6f, 0.6f, 0.6f), 1), emitted);
    }

    [Test]
    public void Get_EmptyReservoir()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);

        Paint got = paintReservoir.Get(0, 0);

        Assert.AreEqual(Paint.EMPTY_PAINT, got);
    }

    [Test]
    public void Get_FilledReservoir()
    {
        // use bigger reservoir to check for correct coordinate to array mapping also
        PaintReservoir paintReservoir = new PaintReservoir(2, 1);
        paintReservoir.Set(0, 1, new Paint(new Color(0.2f, 0.2f, 0.2f), 2));

        Paint got = paintReservoir.Get(0, 1);

        Assert.AreEqual(new Paint(new Color(0.2f, 0.2f, 0.2f), 2), got);

        // paint is still there
        Paint emitted = paintReservoir.Emit(0, 1, 2);
        Assert.AreEqual(new Paint(new Color(0.2f, 0.2f, 0.2f), 2), emitted);
    }
}
