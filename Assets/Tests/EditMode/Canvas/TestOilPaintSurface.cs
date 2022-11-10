using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestOilPaintSurface
{
    [Test]
    public void Apply()
    {
        FastTexture2DMock ft_mock = new FastTexture2DMock();
        OilPaintSurface o = new OilPaintSurface(ft_mock);

        o.Apply();

        Assert.AreEqual("Apply ", ft_mock.Log);
    }

    [Test]
    public void IsInBounds()
    {
        FastTexture2DMock ft_mock = new FastTexture2DMock();
        OilPaintSurface o = new OilPaintSurface(ft_mock);

        o.IsInBounds(1, 2);

        Assert.AreEqual("IsInBounds(1,2) ", ft_mock.Log);
    }
}

class FastTexture2DMock : LogMock, IFastTexture2D
{
    public void Apply()
    {
        Log += "Apply ";
    }

    public bool IsInBounds(int x, int y)
    {
        Log += "IsInBounds(" + x + "," + y + ") ";
        return true;
    }

    /*
     * =================== NOT NEEDED ====================
     */

    public int Height => 0;

    public int Width => 0;

    public Texture2D Texture => null;

    public Color GetPixelFast(int x, int y)
    {
        throw new System.NotImplementedException();
    }

    public void SetPixelFast(int x, int y, Color color)
    {
        throw new System.NotImplementedException();
    }

    /*
     * ================= NOT NEEDED END ==================
     */
}
