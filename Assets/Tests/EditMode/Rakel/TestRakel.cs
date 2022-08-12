using System;
using NUnit.Framework;
using UnityEngine;

public class TestRakel
{
    [Test]
    public void ApplyToCanvas_Point()
    {
        FastTexture2D texture = new FastTexture2D(3, 3);
        OilPaintSurface oilPaintSurface = new OilPaintSurface(texture);
        Rakel rakel = new Rakel(1, 1, new MaskCalculator(), new MaskApplicator());
        rakel.UpdatePaint(new Color(0, 0.4f, 0.8f), 2);
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.UpdateNormal(Vector2Int.right);

        rakel.ApplyToCanvas(oilPaintSurface);

        Color[] colors = texture.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR, new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR,
            },
            colors
        );

        // now empty the reservoir
        rakel.UpdatePosition(new Vector2Int(1, 2));
        rakel.ApplyToCanvas(oilPaintSurface);
        rakel.UpdatePosition(new Vector2Int(1, 0));
        rakel.ApplyToCanvas(oilPaintSurface);
        colors = texture.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual( // Remember: written arrays are upside down coordinate systems
            new Color[]
            {
                Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,      Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR, new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR, new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
            },
            colors
        );
    }

    [Test]
    public void ApplyToCanvas_Point_EmptyRakel_PaintPickup()
    {
        FastTexture2D texture = new FastTexture2D(3, 3);
        OilPaintSurface oilPaintSurface = new OilPaintSurface(texture);
        oilPaintSurface.AddPaint(1, 1, new Color(0, 0.4f, 0.8f));
        Rakel rakel = new Rakel(1, 1, new MaskCalculator(), new MaskApplicator());
        rakel.UpdateNormal(Vector2Int.right);

        // pickup
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.ApplyToCanvas(oilPaintSurface);
        Color[] c = texture.Texture.GetPixels();
        LogUtil.Log(c, 3);

        // emit
        rakel.UpdatePosition(new Vector2Int(0, 0));
        rakel.ApplyToCanvas(oilPaintSurface);

        Color[] colors = texture.Texture.GetPixels();
        LogUtil.Log(colors, 3);
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
