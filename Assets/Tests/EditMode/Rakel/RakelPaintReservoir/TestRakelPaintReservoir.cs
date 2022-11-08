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

    [Test]
    public void Emit_FilledApplicationReservoir_EmptyPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);
        paintReservoir.Fill(new Color(0.2f, 0.3f, 0.4f), 1);

        Color[,] emitted = EmitAll(paintReservoir);

        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
                { new Color(0.2f, 0.3f, 0.4f), new Color(0.2f, 0.3f, 0.4f) },
            },
            emitted
        );
    }

    [Test]
    public void Emit_EmptyApplicationReservoir_FilledPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);
        paintReservoir.Pickup(0, 0, new Color(0.4f, 0.5f, 0.6f), 1);

        Color[,] emitted = EmitAll(paintReservoir);

        AssertUtil.AssertColorsAreEqual(
            new Color[,] {
                { new Color(0.4f, 0.5f, 0.6f), Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
            },
            emitted
        );
    }

    [Test]
    public void Emit_FilledApplicationReservoir_FilledPickupReservoir_ResultsInColorMixing()
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
