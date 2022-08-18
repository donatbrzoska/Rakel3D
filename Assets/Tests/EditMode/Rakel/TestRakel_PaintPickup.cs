using System;
using NUnit.Framework;
using UnityEngine;

public class TestRakelPaintPickup
{
    FastTexture2D texture;
    OilPaintSurface oilPaintSurface;
    Rakel rakel;

    [SetUp]
    public void Init()
    {
        texture = new FastTexture2D(3, 3);
        oilPaintSurface = new OilPaintSurface(texture);
        rakel = new Rakel(1, 1, 1, oilPaintSurface, new MaskCalculator(), new MaskApplicator());
        rakel.UpdateNormal(Vector2Int.right);
    }

    [Test]
    public void PointRakel_EmptyRakel_PaintPickup()
    {
        oilPaintSurface.AddPaint(1, 1, new Color(0, 0.4f, 0.8f));

        // pickup
        rakel.ApplyAt(new Vector2Int(1, 1));
        // emit
        rakel.ApplyAt(new Vector2Int(0, 0));

        Color[] colors = texture.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
            },
            colors
        );


        // repeating the process should yield the same result

        oilPaintSurface.AddPaint(1, 1, new Color(0, 0.8f, 0.8f));

        // pickup
        rakel.ApplyAt(new Vector2Int(1, 1));
        // emit
        rakel.ApplyAt(new Vector2Int(0, 0));

        colors = texture.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0, 0.8f, 0.8f), Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
            },
            colors
        );
    }

    [Test]
    public void PointRakel_EmptyRakel_PaintPickup_OOB()
    {
        oilPaintSurface.AddPaint(1, 1, new Color(0, 0.4f, 0.8f));

        // pickup
        rakel.ApplyAt(new Vector2Int(1, 1));
        // emit OOB should not emit picked up paint
        rakel.ApplyAt(new Vector2Int(3, 3));
        // emit in bounds should emit
        rakel.ApplyAt(new Vector2Int(0, 0));

        Color[] colors = texture.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
            },
            colors
        );
    }
}
