using System;
using NUnit.Framework;
using UnityEngine;

public class TestRakelPaintReservoir
{
    /* 
     * -----------------------------------------------------------------------------------------------------------
     * ------------------------------------------------- Helper --------------------------------------------------
     * -----------------------------------------------------------------------------------------------------------
     */

    private Color[,] EmitAll(RakelPaintReservoir paintReservoir)
    {
        Color[,] emitted = new Color[paintReservoir.Height, paintReservoir.Width];
        for (int i = 0; i < paintReservoir.Height; i++)
        {
            for (int j = 0; j < paintReservoir.Width; j++)
            {
                emitted[i, j] = paintReservoir.Emit(j, i);
            }
        }
        return emitted;
    }

    /* 
     * -----------------------------------------------------------------------------------------------------------
     * ------------------------------------------------- Tests ---------------------------------------------------
     * -----------------------------------------------------------------------------------------------------------
     */

    [Test]
    public void Emit_EmptyApplicationReservoir_EmptyPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);

        Color emitted = paintReservoir.Emit(0, 0);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            emitted
        );
    }

    // This is about
    // 1. correct color emission
    // 2. paint volume reduction
    [Test]
    public void Emit_FilledApplicationReservoir_EmptyPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);
        paintReservoir.Fill(new Color(0.2f, 0.3f, 0.4f), 2);

        Color[,] emitted = EmitAll(paintReservoir);
        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
            },
            emitted
        );

        // empty application reservoir
        emitted = EmitAll(paintReservoir);
        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
            },
            emitted
        );

        // now it should be empty
        emitted = EmitAll(paintReservoir);
        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
            },
            emitted
        );
    }

    // This is about
    // 1. correct color emission
    // 2. paint volume reduction
    [Test]
    public void Emit_EmptyApplicationReservoir_FilledPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);
        int pickupUpVolume = 2;
        paintReservoir.Pickup(0, 0, new Color(0.4f, 0.5f, 0.6f), pickupUpVolume);

        // first piece of paint is emitted
        Color[,] emitted = EmitAll(paintReservoir);
        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { new Color(0.4f, 0.5f, 0.6f), Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
            },
            emitted
        );

        // second piece of paint is emitted
        emitted = EmitAll(paintReservoir);
        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { new Color(0.4f, 0.5f, 0.6f), Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
            },
            emitted
        );

        // now it should be empty, so no paint is emitted
        emitted = EmitAll(paintReservoir);
        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
            },
            emitted
        );
    }

    // This is about color mixing
    [Test]
    public void Emit_FilledApplicationReservoir_FilledPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);
        paintReservoir.Fill(new Color(0.2f, 0.3f, 0.4f), 1);
        paintReservoir.Pickup(0, 0, new Color(0.4f, 0.5f, 0.6f), 1);

        Color[,] emitted = EmitAll(paintReservoir);

        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { new Color(0.3f, 0.4f, 0.5f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
            },
            emitted
        );
    }


    // This is about
    // 1. color mixing in the pickup reservoir
    [Test]
    public void Pickup_EmptyApplicationReservoir_FilledPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);
        paintReservoir.Pickup(0, 0, new Color(0.2f, 0.2f, 0.2f), 1);

        paintReservoir.Pickup(0, 0, new Color(0.8f, 0.8f, 0.8f), 2);

        Color[,] emitted = EmitAll(paintReservoir);
        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { new Color(0.6f, 0.6f, 0.6f), Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
            },
            emitted
        );
    }

    // TODO naming: ..._DoesNotDeletePaint
    [Test]
    public void Pickup_NO_PAINT_COLOR_FilledApplicationReservoir_FilledPickupReservoir_DoesNothing()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);
        paintReservoir.Fill(new Color(0.2f, 0.3f, 0.4f), 1);
        paintReservoir.Pickup(0, 0, new Color(0.4f, 0.5f, 0.6f), 1);

        paintReservoir.Pickup(0, 0, Colors.NO_PAINT_COLOR, 1);

        Color[,] emitted = EmitAll(paintReservoir);
        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { new Color(0.3f, 0.4f, 0.5f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
            },
            emitted
        );

        // TODO is this even test case scope?
        // now pickup reservoir should be empty
        emitted = EmitAll(paintReservoir);
        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
            },
            emitted
        );
    }

    [Test]
    public void Pivot()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);

        Vector2Int pivot = paintReservoir.Pivot;

        Assert.AreEqual(
            new Vector2Int(1, 1),
            pivot
        );
    }

    [Test]
    public void Pivot_Even_Height()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(2, 2);

        Vector2Int pivot = paintReservoir.Pivot;

        Assert.AreEqual(
            new Vector2Int(1, 1),
            pivot
        );
    }
}
