using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestFastTexture2D
{
    Color DEFAULT_COLOR = new Color(0.804f, 0.804f, 0.804f, 0.804f);
    FastTexture2D t;

    [SetUp]
    public void Init()
    {
        t = new FastTexture2D(2, 2);
    }

    /* 
     * -----------------------------------------------------------------------------------------------------------
     * ---------------------------------------------- SetPixelFast -----------------------------------------------
     * -----------------------------------------------------------------------------------------------------------
     */

    // NOTE Using t.Texture.GetPixels to validate SetPixelFast is kind of hacky, because this is only public API to the renderer
    //      But that's probably okay, since GetPixelFast is tested later on by setting pixels via SetPixelFast

    [Test]
    public void SetPixelFast_Usual()
    {
        t.SetPixelFast(0, 0, new Color(0, 0, 0));

        // NOTE the array representation mirrors the entire coordinate system by the x axis (for us with the 180° rotated canvas at least)
        Color[] colors = t.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0, 0, 0), DEFAULT_COLOR,
                DEFAULT_COLOR,      DEFAULT_COLOR,
            },
            colors
        );

        // NOTE 0,0 means array upper left
        Color color = t.Texture.GetPixel(0, 0);
        AssertUtil.AssertColorsAreEqual(
            new Color(0, 0, 0),
            color
        );
    }

    [Test]
    public void SetPixelFast_Lower_OOB()
    {
        t.SetPixelFast(0, -1, new Color(1, 1, 0));

        Color[] colors = t.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                DEFAULT_COLOR, DEFAULT_COLOR,
                DEFAULT_COLOR, DEFAULT_COLOR,
            },
            colors
        );
    }

    [Test]
    public void SetPixelFast_Right_OOB()
    {
        t.SetPixelFast(2, 0, new Color(1, 1, 0));

        Color[] colors = t.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                DEFAULT_COLOR, DEFAULT_COLOR,
                DEFAULT_COLOR, DEFAULT_COLOR,
            },
            colors
        );
    }

    [Test]
    public void SetPixelFast_Upper_OOB()
    {
        t.SetPixelFast(0, 2, new Color(1, 1, 0));

        Color[] colors = t.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                DEFAULT_COLOR, DEFAULT_COLOR,
                DEFAULT_COLOR, DEFAULT_COLOR,
            },
            colors
        );
    }

    [Test]
    public void SetPixelFast_Left_OOB()
    {
        t.SetPixelFast(-1, 0, new Color(1, 1, 0));

        Color[] colors = t.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                DEFAULT_COLOR, DEFAULT_COLOR,
                DEFAULT_COLOR, DEFAULT_COLOR,
            },
            colors
        );
    }

    /* 
     * -----------------------------------------------------------------------------------------------------------
     * ---------------------------------------------- GetPixelFast -----------------------------------------------
     * -----------------------------------------------------------------------------------------------------------
     */

    [Test]
    public void GetPixelFast_Usual()
    {
        t.SetPixelFast(0, 0, new Color(0, 0, 0));

        Color[,] colors = new Color[t.Height, t.Width];
        for (int i = 0; i < t.Height; i++)
        {
            for (int j = 0; j < t.Width; j++)
            {
                colors[i, j] = t.GetPixelFast(j, i);
            }
        }

        AssertUtil.AssertColorsAreEqual(
            new Color[,]{
                { new Color(0, 0, 0), DEFAULT_COLOR },
                { DEFAULT_COLOR,      DEFAULT_COLOR },
            },
            colors
        );
    }

    [Test]
    public void GetPixelFast_Lower_OOB()
    {
        Color color = t.GetPixelFast(0, -1);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            color
        );
    }

    [Test]
    public void GetPixelFast_Right_OOB()
    {
        Color color = t.GetPixelFast(2, 0);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            color
        );
    }

    [Test]
    public void GetPixelFast_Upper_OOB()
    {
        Color color = t.GetPixelFast(0, 2);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            color
        );
    }

    [Test]
    public void GetPixelFast_Left_OOB()
    {
        Color color = t.GetPixelFast(-1, 0);

        AssertUtil.AssertColorsAreEqual(
            Colors.NO_PAINT_COLOR,
            color
        );
    }
}
