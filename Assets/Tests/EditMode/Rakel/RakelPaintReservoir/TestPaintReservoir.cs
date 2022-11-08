using System;
using NUnit.Framework;
using UnityEngine;

public class TestPaintReservoir
{
    [Test]
    public void Emit_Filled_EmitsCorrectColor()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Set(0, 0, new Color(0.2f, 0.3f, 0.4f), 1);

        Color emitted = paintReservoir.Emit(0, 0);

        Assert.AreEqual(new Color(0.2f, 0.3f, 0.4f), emitted);
    }

    [Test]
    public void Emit_Empty_EmitsEmptyColor()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);

        Color emitted = paintReservoir.Emit(0, 0);

        Assert.AreEqual(Colors.NO_PAINT_COLOR, emitted);
    }

    // prevent problems with possibly negative volumes
    [Test]
    public void Emit_Empty_FollowedBySet_EmitsCorrectColor()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Emit(0, 0);
        paintReservoir.Set(0, 0, new Color(0.2f, 0.3f, 0.4f), 1);

        Color emitted = paintReservoir.Emit(0, 0);

        Assert.AreEqual(new Color(0.2f, 0.3f, 0.4f), emitted);
    }

    // prevent problems with possibly negative volumes
    [Test]
    public void Emit_Empty_FollowedByPickup_EmitsCorrectColor()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Emit(0, 0);
        paintReservoir.Pickup(0, 0, new Color(0.2f, 0.3f, 0.4f), 1);

        Color emitted = paintReservoir.Emit(0, 0);

        Assert.AreEqual(new Color(0.2f, 0.3f, 0.4f), emitted);
    }

    // TODO Later: Also do HasLessVolumeButHasVolume
    [Test]
    public void Emit_VolumeReduction_HasVolume()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Set(0, 0, new Color(0.2f, 0.3f, 0.4f), 2);

        Color emitted1 = paintReservoir.Emit(0, 0);
        Color emitted2 = paintReservoir.Emit(0, 0);
        Color emitted3 = paintReservoir.Emit(0, 0);

        Assert.AreEqual(new Color(0.2f, 0.3f, 0.4f), emitted1);
        Assert.AreEqual(new Color(0.2f, 0.3f, 0.4f), emitted2);
        Assert.AreEqual(Colors.NO_PAINT_COLOR, emitted3);
    }

    [Test]
    public void Set_EmptyColor_SetsEmptyColor()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Set(0, 0, new Color(0.4f, 0.5f, 0.6f), 1);

        paintReservoir.Set(0, 0, Colors.NO_PAINT_COLOR, 10);

        Color emitted = paintReservoir.Emit(0, 0);
        Assert.AreEqual(Colors.NO_PAINT_COLOR, emitted);
    }

    [Test]
    public void Pickup_EmptyColor_DoesNothing()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Pickup(0, 0, new Color(0.4f, 0.5f, 0.6f), 1);

        paintReservoir.Pickup(0, 0, Colors.NO_PAINT_COLOR, 10);

        Color emitted = paintReservoir.Emit(0, 0);
        Assert.AreEqual(new Color(0.4f, 0.5f, 0.6f), emitted);
    }

    [Test]
    public void Pickup_EmptyReservoir()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);

        paintReservoir.Pickup(0, 0, new Color(0.8f, 0.8f, 0.8f), 2);

        Color emitted = paintReservoir.Emit(0, 0);
        Assert.AreEqual(new Color(0.8f, 0.8f, 0.8f), emitted);
    }

    [Test]
    public void Pickup_FilledReservoir_ResultsInColorMixing()
    {
        PaintReservoir paintReservoir = new PaintReservoir(1, 1);
        paintReservoir.Pickup(0, 0, new Color(0.2f, 0.2f, 0.2f), 1);

        paintReservoir.Pickup(0, 0, new Color(0.8f, 0.8f, 0.8f), 2);

        Color emitted = paintReservoir.Emit(0, 0);
        Assert.AreEqual(new Color(0.6f, 0.6f, 0.6f), emitted);
    }
}
