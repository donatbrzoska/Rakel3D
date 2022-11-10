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

    private Paint[,] EmitAll(RakelPaintReservoir paintReservoir)
    {
        Paint[,] emitted = new Paint[paintReservoir.Height, paintReservoir.Width];
        for (int i = 0; i < paintReservoir.Height; i++)
        {
            for (int j = 0; j < paintReservoir.Width; j++)
            {
                emitted[i, j] = paintReservoir.Emit(j, i, 1, 1);
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
    public void Fill()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(2, 1);

        paintReservoir.Fill(new Paint(new Color(0.2f, 0.4f, 0.6f), 1));

        Paint[,] emitted = EmitAll(paintReservoir);
        Assert.AreEqual(
            new Paint[,] {
                { new Paint(new Color(0.2f, 0.4f, 0.6f), 1) },
                { new Paint(new Color(0.2f, 0.4f, 0.6f), 1) },
            },
            emitted
        );
    }

    [Test]
    public void Emit_EmptyApplicationReservoir_EmptyPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);

        Paint emitted = paintReservoir.Emit(0, 0, 1, 1);

        Assert.AreEqual(
            Paint.EMPTY_PAINT,
            emitted
        );
    }

    [Test]
    public void Emit_FilledApplicationReservoir_EmptyPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);
        paintReservoir.Fill(new Paint(new Color(0.2f, 0.3f, 0.4f), 1));

        Paint[,] emitted = EmitAll(paintReservoir);

        Assert.AreEqual(
            new Paint[,] {
                { new Paint(new Color(0.2f, 0.3f, 0.4f), 1), new Paint(new Color(0.2f, 0.3f, 0.4f), 1) },
                { new Paint(new Color(0.2f, 0.3f, 0.4f), 1), new Paint(new Color(0.2f, 0.3f, 0.4f), 1) },
                { new Paint(new Color(0.2f, 0.3f, 0.4f), 1), new Paint(new Color(0.2f, 0.3f, 0.4f), 1) },
            },
            emitted
        );
    }

    [Test]
    public void Emit_EmptyApplicationReservoir_FilledPickupReservoir()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);
        paintReservoir.Pickup(0, 0, new Paint(new Color(0.4f, 0.5f, 0.6f), 1));

        Paint[,] emitted = EmitAll(paintReservoir);

        Assert.AreEqual(
            new Paint[,] {
                { new Paint(new Color(0.4f, 0.5f, 0.6f), 1), Paint.EMPTY_PAINT },
                { Paint.EMPTY_PAINT,                         Paint.EMPTY_PAINT },
                { Paint.EMPTY_PAINT,                         Paint.EMPTY_PAINT },
            },
            emitted
        );
    }

    [Test]
    public void Emit_FilledApplicationReservoir_FilledPickupReservoir_ResultsInColorMixing()
    {
        RakelPaintReservoir paintReservoir = new RakelPaintReservoir(3, 2);

        paintReservoir.Fill(new Paint(new Color(0.2f, 0.3f, 0.4f), 1));
        paintReservoir.Pickup(0, 0, new Paint(new Color(0.4f, 0.5f, 0.6f), 2));

        Paint[,] emitted = EmitAll(paintReservoir);
        Assert.AreEqual(
            new Paint[,] {
                { new Paint(new Color(0.3f, 0.4f, 0.5f), 2), new Paint(new Color(0.2f, 0.3f, 0.4f), 1) },
                { new Paint(new Color(0.2f, 0.3f, 0.4f), 1), new Paint(new Color(0.2f, 0.3f, 0.4f), 1) },
                { new Paint(new Color(0.2f, 0.3f, 0.4f), 1), new Paint(new Color(0.2f, 0.3f, 0.4f), 1) },
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
