using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestOilPaintSurface_RakelView
{
    /* 
     * -----------------------------------------------------------------------------------------------------------
     * ------------------------------------------------- Helper --------------------------------------------------
     * -----------------------------------------------------------------------------------------------------------
     */

    private Paint[,] GetPaintAll(OilPaintSurface o)
    {
        Paint[,] paint = new Paint[CanvasHeight, CanvasWidth];
        for (int i = 0; i < CanvasHeight; i++)
        {
            for (int j = 0; j < CanvasWidth; j++)
            {
                paint[i, j] = o.GetPaint(j, i, 1);
            }
        }
        return paint;
    }

    /* 
     * -----------------------------------------------------------------------------------------------------------
     * ------------------------------------------------- Tests ---------------------------------------------------
     * -----------------------------------------------------------------------------------------------------------
     */

    int CanvasWidth = 2;
    int CanvasHeight = 2;

    OilPaintSurface oilPaintSurface;

    [SetUp]
    public void Init()
    {
        FastTexture2D fastTexture2D = new FastTexture2D(CanvasWidth, CanvasHeight);
        oilPaintSurface = new OilPaintSurface(fastTexture2D);
    }

    [Test]
    public void InitialState()
    {
        Paint[,] paint = GetPaintAll(oilPaintSurface);

        Assert.AreEqual(
            new Paint[,]
            {
                { Paint.EMPTY_PAINT, Paint.EMPTY_PAINT },
                { Paint.EMPTY_PAINT, Paint.EMPTY_PAINT },
            },
            paint
        );
    }

    [Test]
    public void AddPaint_GetPaint()
    {
        oilPaintSurface.AddPaint(0, 0, new Paint(new Color(0.2f, 0.2f, 0.2f), 1));

        // 1. we should obtain paint
        Paint[,] paint = GetPaintAll(oilPaintSurface);
        Assert.AreEqual(
            new Paint[,]
            {
                { new Paint(new Color(0.2f, 0.2f, 0.2f), 1), Paint.EMPTY_PAINT },
                { Paint.EMPTY_PAINT,                         Paint.EMPTY_PAINT },
            },
            paint
        );

        // 2. now the paint should be gone
        paint = GetPaintAll(oilPaintSurface);
        Assert.AreEqual(
            new Paint[,]
            {
                { Paint.EMPTY_PAINT, Paint.EMPTY_PAINT },
                { Paint.EMPTY_PAINT, Paint.EMPTY_PAINT },
            },
            paint
        );
    }

    [Test]
    public void AddPaint_EMPTY_PAINT_DoesNothing()
    {
        oilPaintSurface.AddPaint(0, 0, new Paint(new Color(0.2f, 0.2f, 0.2f), 1));

        oilPaintSurface.AddPaint(0, 0, Paint.EMPTY_PAINT);

        Paint[,] paint = GetPaintAll(oilPaintSurface);
        Assert.AreEqual(
            new Paint[,]
            {
                { new Paint(new Color(0.2f, 0.2f, 0.2f), 1), Paint.EMPTY_PAINT },
                { Paint.EMPTY_PAINT,                         Paint.EMPTY_PAINT },
            },
            paint
        );
    }
}