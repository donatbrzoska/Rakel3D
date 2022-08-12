using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestOilPaintSurface_Apply
{
    [Test]
    public void Apply()
    {
        FastTexture2DMock ft_mock = new FastTexture2DMock();
        OilPaintSurface o = new OilPaintSurface(ft_mock);

        o.Apply();

        Assert.AreEqual(1, ft_mock.ApplyCounter);
    }
}

class FastTexture2DMock : IFastTexture2D
{
    public int ApplyCounter { get; private set; }

    public void Apply()
    {
        ApplyCounter++;
    }

    /*
     * =================== NOT NEEDED ====================
     */

    public int Height => 0;

    public int Width => 0;

    public Color GetPixelFast(int x, int y)
    {
        throw new System.NotImplementedException();
    }

    public void SetPixelFast(int x, int y, Color color)
    {
        throw new System.NotImplementedException();
    }

    public bool PixelInBounds(int x, int y)
    {
        throw new System.NotImplementedException();
    }

    /*
     * ================= NOT NEEDED END ==================
     */
}
