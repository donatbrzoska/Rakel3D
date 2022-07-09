using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestOilPaintTexture
{
    OilPaintTexture t;

    [SetUp]
    public void Init()
    {
        t = new OilPaintTexture(2, 2);
    }

    [Test]
    public void InitialState()
    {
        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );
    }

    // NOTE this is just for understanding how the arrays are organized
    [Test]
    public void SetPixel_Usual()
    {
        t.SetPixel(0, 0, new Color(0, 0, 0));

        // NOTE the array representation mirrors the entire coordinate system by the x axis (for us with the 180° rotated canvas at least)
        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0, 0, 0),    new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );

        // NOTE 0,0 means array upper left
        Color color = t.GetPixel(0, 0);
        AssertUtil.AssertColorsAreEqual(
            new Color(0, 0, 0),
            color
        );
    }

    [Test]
    public void SetPixel_Upper_OOB()
    {
        t.SetPixel(0, -1, new Color(1, 1, 0));

        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );
    }

    [Test]
    public void SetPixel_Right_OOB()
    {
        t.SetPixel(2, 0, new Color(1, 1, 0));

        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );
    }

    [Test]
    public void SetPixel_Lower_OOB()
    {
        t.SetPixel(0, 2, new Color(1, 1, 0));

        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );
    }

    [Test]
    public void SetPixel_Left_OOB()
    {
        t.SetPixel(-1, 0, new Color(1, 1, 0));

        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );
    }

    [Test]
    public void SetPixelFast_Usual()
    {
        t.SetPixelFast(0, 0, new Color(0, 0, 0));

        // NOTE the array representation mirrors the entire coordinate system by the x axis (for us with the 180° rotated canvas at least)
        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0, 0, 0),    new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );

        // NOTE 0,0 means array upper left
        Color color = t.GetPixel(0, 0);
        AssertUtil.AssertColorsAreEqual(
            new Color(0, 0, 0),
            color
        );
    }

    [Test]
    public void SetPixelFast_Upper_OOB()
    {
        t.SetPixelFast(0, -1, new Color(1, 1, 0));

        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );
    }

    [Test]
    public void SetPixelFast_Right_OOB()
    {
        t.SetPixelFast(2, 0, new Color(1, 1, 0));

        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );
    }

    [Test]
    public void SetPixelFast_Lower_OOB()
    {
        t.SetPixelFast(0, 2, new Color(1, 1, 0));

        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );
    }

    [Test]
    public void SetPixelFast_Left_OOB()
    {
        t.SetPixelFast(-1, 0, new Color(1, 1, 0));

        Color[] colors = t.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
                new Color(1, 1, 1, 1), new Color(1, 1, 1, 1),
            },
            colors
        );
    }

}
