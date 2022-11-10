using System;
using NUnit.Framework;
using UnityEngine;

public class TestRakelDrawer_INT
{
    FastTexture2D texture;
    OilPaintSurface oilPaintSurface;
    RakelPaintReservoir rakelPaintReservoir;
    Rakel rakel;
    RakelDrawer rakelDrawer;

    [SetUp]
    public void Init()
    {
        texture = new FastTexture2D(3, 3);
        oilPaintSurface = new OilPaintSurface(texture);
        rakelPaintReservoir = new RakelPaintReservoir(3, 1);
        rakel = new Rakel(3, 1, rakelPaintReservoir, new MaskCalculator(), new MaskApplicator());
        rakelDrawer = new RakelDrawer(rakel);
    }

    [Test]
    public void LineRakel_Rectangle()
    {
        rakelPaintReservoir.Fill(new Paint(new Color(0, 0.4f, 0.8f), 2));

        rakelDrawer.NewStroke();
        rakelDrawer.AddNode(oilPaintSurface, new Vector2Int(0, 1), Vector2Int.right);
        rakelDrawer.AddNode(oilPaintSurface, new Vector2Int(1, 1), Vector2Int.right);

        Color[] colors = texture.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
                new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
                new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), Colors.CANVAS_COLOR,
            },
            colors
        );
    }
}
