using System;
using NUnit.Framework;
using UnityEngine;

public class TestPaintReservoir_OOB
{
    PaintReservoir paintReservoir;

    [SetUp]
    public void Init()
    {
        paintReservoir = new PaintReservoir(2, 2);

        for (int i = 0; i < paintReservoir.Width; i++)
        {
            for (int j = 0; j < paintReservoir.Height; j++)
            {
                paintReservoir.Set(i, j, new Paint(new Color(0.2f, 0.3f, 0.4f), 1));
            }
        }
    }

    [Test]
    public void Emit_OOB_Upper()
    {
        Paint emitted = paintReservoir.Emit(0, 2, 1);

        Assert.AreEqual(
            Paint.EMPTY_PAINT,
            emitted
        );
    }

    [Test]
    public void Emit_OOB_Lower()
    {
        Paint emitted = paintReservoir.Emit(0, -1, 1);

        Assert.AreEqual(
            Paint.EMPTY_PAINT,
            emitted
        );
    }

    [Test]
    public void Emit_OOB_Left()
    {
        Paint emitted = paintReservoir.Emit(-1, 0, 1);

        Assert.AreEqual(
            Paint.EMPTY_PAINT,
            emitted
        );
    }

    [Test]
    public void Emit_OOB_Right()
    {
        Paint emitted = paintReservoir.Emit(2, 0, 1);

        Assert.AreEqual(
            Paint.EMPTY_PAINT,
            emitted
        );
    }

    // incomplete but pretty sure enough in this case
    [Test]
    public void Get_OOB_Right()
    {
        Paint emitted = paintReservoir.Get(2, 0);

        Assert.AreEqual(
            Paint.EMPTY_PAINT,
            emitted
        );
    }
}
