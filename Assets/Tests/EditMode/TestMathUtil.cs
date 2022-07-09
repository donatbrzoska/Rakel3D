using System;
using NUnit.Framework;
using UnityEngine;

public class TestMathUtil
{
    [Test]
    public void Angle360_0()
    {
        Vector2 from = Vector2.right;
        Vector2 to = Vector2.right;

        Assert.AreEqual(0, MathUtil.Angle360(from, to));
    }

    [Test]
    public void Angle360_90()
    {
        Vector2 from = Vector2.right;
        Vector2 to = Vector2.down;

        Assert.AreEqual(90, MathUtil.Angle360(from, to));
    }

    [Test]
    public void Angle360_270()
    {
        Vector2 from = Vector2.right;
        Vector2 to = Vector2.up;

        Assert.AreEqual(270, MathUtil.Angle360(from, to));
    }
}
