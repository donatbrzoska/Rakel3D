using System;
using NUnit.Framework;
using UnityEngine;

public class TestOilPaintSurface_RendererView
{
    int CanvasWidth = 2;
    int CanvasHeight = 2;

    FastTexture2D fastTexture2D;
    OilPaintSurface oilPaintSurface;

    [SetUp]
    public void Init()
    {
        fastTexture2D = new FastTexture2D(CanvasWidth, CanvasHeight);
        oilPaintSurface = new OilPaintSurface(fastTexture2D);
    }

    // This is kind of not the public API for Rakel
    // - But it is for the rendering pipline, since it directly uses the Texture2D obejct
    [Test]
    public void InitialState()
    {
        Color[] colors = fastTexture2D.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
            },
            colors
        );
    }

    [Test]
    public void AddPaint()
    {
        oilPaintSurface.AddPaint(0, 0, new Color(0.2f, 0.2f, 0.2f));

        Color[] colors = fastTexture2D.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0.2f, 0.2f, 0.2f), Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,         Colors.CANVAS_COLOR,
            },
            colors
        );
    }

    [Test]
    public void AddPaint_NO_PAINT_COLOR_DoesNothing()
    {
        oilPaintSurface.AddPaint(0, 0, new Color(0.2f, 0.2f, 0.2f));

        oilPaintSurface.AddPaint(0, 0, Colors.NO_PAINT_COLOR);

        Color[] colors = fastTexture2D.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0.2f, 0.2f, 0.2f), Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,         Colors.CANVAS_COLOR,
            },
            colors
        );
    }

    [Test]
    public void GetPaint()
    {
        oilPaintSurface.AddPaint(0, 0, new Color(0.2f, 0.2f, 0.2f));
        Color c = oilPaintSurface.GetPaint(0, 0);

        // added paint should be gone again
        Color[] colors = fastTexture2D.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
            },
            colors
        );
    }
}
