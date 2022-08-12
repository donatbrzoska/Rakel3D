using System;
using NUnit.Framework;
using UnityEngine;

public class TestRakelPaintReservoir_OOB
{
    [Test]
    public void Emit_OOB_Upper()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(2, 2);

        Color emitted = paintReservoir.Emit(0, 2);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            emitted
        );
    }

    [Test]
    public void Emit_OOB_Lower()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(2, 2);

        Color emitted = paintReservoir.Emit(0, -1);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            emitted
        );
    }

    [Test]
    public void Emit_OOB_Left()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(2, 2);

        Color emitted = paintReservoir.Emit(-1, 0);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            emitted
        );
    }

    [Test]
    public void Emit_OOB_Right()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(2, 2);

        Color emitted = paintReservoir.Emit(2, 0);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            emitted
        );
    }
}
