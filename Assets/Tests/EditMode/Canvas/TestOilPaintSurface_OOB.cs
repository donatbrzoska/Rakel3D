using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestOilPaintSurface_OOB
{
    int CanvasWidth = 2;
    int CanvasHeight = 2;

    OilPaintSurface oilPaintSurface;

    [SetUp]
    public void Init()
    {
        FastTexture2D fastTexture2D = new FastTexture2D(CanvasWidth, CanvasHeight);
        oilPaintSurface = new OilPaintSurface(fastTexture2D);

        AddPaintAll(oilPaintSurface, new Color(0.2f, 0.3f, 0.4f));
    }

    private void AddPaintAll(OilPaintSurface o, Color c)
    {
        for (int i = 0; i < CanvasHeight; i++)
        {
            for (int j = 0; j < CanvasWidth; j++)
            {
                o.AddPaint(j, i, c);
            }
        }
    }

    [Test]
    public void AddPaint_GetPaint_OOB_Upper()
    {
        Color c = oilPaintSurface.GetPaint(0, 3);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            c
        );
    }

    [Test]
    public void AddPaint_GetPaint_OOB_Lower()
    {
        Color c = oilPaintSurface.GetPaint(0, -1);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            c
        );
    }

    [Test]
    public void AddPaint_GetPaint_OOB_Left()
    {
        Color c = oilPaintSurface.GetPaint(-1, 0);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            c
        );
    }

    [Test]
    public void AddPaint_GetPaint_OOB_Right()
    {
        Color c = oilPaintSurface.GetPaint(3, 0);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            c
        );
    }
}