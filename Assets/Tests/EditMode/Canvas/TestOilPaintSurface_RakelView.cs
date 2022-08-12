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

    private Color[,] GetPaintAll(OilPaintSurface o)
    {
        Color[,] colors = new Color[CanvasHeight, CanvasWidth];
        for (int i = 0; i < CanvasHeight; i++)
        {
            for (int j = 0; j < CanvasWidth; j++)
            {
                colors[i, j] = o.GetPaint(j, i);
            }
        }
        return colors;
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
        Color[,] colors = GetPaintAll(oilPaintSurface);

        AssertUtil.AssertColorsAreEqual(
            new Color[,]
            {
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
            },
            colors
        );
    }

    [Test]
    public void AddPaint_GetPaint()
    {
        oilPaintSurface.AddPaint(0, 0, new Color(0.2f, 0.2f, 0.2f));

        // 1. we should obtain paint
        Color[,] colors = GetPaintAll(oilPaintSurface);
        AssertUtil.AssertColorsAreEqual(
            new Color[,]
            {
                { new Color(0.2f, 0.2f, 0.2f), Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
            },
            colors
        );

        // 2. now the paint should be gone
        colors = GetPaintAll(oilPaintSurface);
        AssertUtil.AssertColorsAreEqual(
            new Color[,]
            {
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR, Colors.NO_PAINT_COLOR },
            },
            colors
        );
    }

    [Test]
    public void AddPaint_NO_PAINT_COLOR()
    {
        oilPaintSurface.AddPaint(0, 0, new Color(0.2f, 0.2f, 0.2f));

        oilPaintSurface.AddPaint(0, 0, Colors.NO_PAINT_COLOR);

        Color[,] colors = GetPaintAll(oilPaintSurface);
        AssertUtil.AssertColorsAreEqual(
            new Color[,]
            {
                { new Color(0.2f, 0.2f, 0.2f), Colors.NO_PAINT_COLOR },
                { Colors.NO_PAINT_COLOR,       Colors.NO_PAINT_COLOR },
            },
            colors
        );
    }
}