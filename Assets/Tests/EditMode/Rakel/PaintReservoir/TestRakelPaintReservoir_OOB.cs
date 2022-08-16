using System;
using NUnit.Framework;
using UnityEngine;

public class TestRakelPaintReservoir_OOB
{
    RakelPaintReservoir paintReservoir;

    [SetUp]
    public void Init()
    {
        paintReservoir = new RakelPaintReservoir(2, 2, 0);
    }

    [Test]
    public void Emit_OOB_Upper()
    {
        Color emitted = paintReservoir.Emit(0, 2);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            emitted
        );
    }

    [Test]
    public void Emit_OOB_Lower()
    {
        Color emitted = paintReservoir.Emit(0, -1);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            emitted
        );
    }

    [Test]
    public void Emit_OOB_Left()
    {
        Color emitted = paintReservoir.Emit(-1, 0);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            emitted
        );
    }

    [Test]
    public void Emit_OOB_Right()
    {
        Color emitted = paintReservoir.Emit(2, 0);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            emitted
        );
    }
}
