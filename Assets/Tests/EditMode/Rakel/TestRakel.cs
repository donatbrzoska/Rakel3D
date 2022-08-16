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
        Rakel rakel = new Rakel(1, 1, 0, new MaskCalculator(), new MaskApplicator());
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
        Rakel rakel = new Rakel(1, 1, 1, new MaskCalculator(), new MaskApplicator());
        rakel.UpdateNormal(Vector2Int.right);

        oilPaintSurface.AddPaint(1, 1, new Color(0, 0.4f, 0.8f));

        // pickup
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.ApplyToCanvas(oilPaintSurface);

        // emit
        rakel.UpdatePosition(new Vector2Int(0, 0));
        rakel.ApplyToCanvas(oilPaintSurface);

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
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.ApplyToCanvas(oilPaintSurface);

        // emit
        rakel.UpdatePosition(new Vector2Int(0, 0));
        rakel.ApplyToCanvas(oilPaintSurface);

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
    public void ApplyToCanvas_Point_EmptyRakel_PaintPickup_OOB()
    {
        FastTexture2D texture = new FastTexture2D(3, 3);
        OilPaintSurface oilPaintSurface = new OilPaintSurface(texture);
        Rakel rakel = new Rakel(1, 1, 1, new MaskCalculator(), new MaskApplicator());
        rakel.UpdateNormal(Vector2Int.right);

        oilPaintSurface.AddPaint(1, 1, new Color(0, 0.4f, 0.8f));

        // pickup
        rakel.UpdatePosition(new Vector2Int(1, 1));
        rakel.ApplyToCanvas(oilPaintSurface);

        // emit OOB should not emit picked up paint
        rakel.UpdatePosition(new Vector2Int(3, 3));
        rakel.ApplyToCanvas(oilPaintSurface);

        // emit in bounds should emit
        rakel.UpdatePosition(new Vector2Int(0, 0));
        rakel.ApplyToCanvas(oilPaintSurface);

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
