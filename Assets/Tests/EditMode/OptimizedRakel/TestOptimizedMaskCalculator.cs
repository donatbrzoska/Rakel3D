using System;
using NUnit.Framework;
using UnityEngine;

public class TestOptimizedMaskCalculator
{
    OptimizedMaskCalculator o;

    [SetUp]
    public void Init()
    {
        o = new OptimizedMaskCalculator();
    }

    [Test]
    public void InitialState_Line()
    {
        OptimizedMask mask = o.Calculate(3, 1, Vector2.right);

        Assert.AreEqual(
            new int[3, 2] {
                { 0, 0 },
                { 0, 0 },
                { 0, 0 }},
            mask.coordinates);

        Assert.AreEqual(1, mask.y0_index);
    }

    [Test]
    public void InitialState_Lines()
    {
        OptimizedMask mask = o.Calculate(3, 2, Vector2.right);

        Assert.AreEqual(
            new int[3, 2] {
                { -1, 0 },
                { -1, 0 },
                { -1, 0 }},
            mask.coordinates);

        Assert.AreEqual(1, mask.y0_index);
    }

    [Test]
    public void InitialState_Rectangle()
    {
        OptimizedMask mask = o.Calculate(5, 3, Vector2.right);

        Assert.AreEqual(
            new int[5, 2] {
                { -2, /*-1, */ 0 },
                { -2, /*-1, */ 0 },
                { -2, /*-1, */ 0 },
                { -2, /*-1, */ 0 },
                { -2, /*-1, */ 0 }},
            mask.coordinates);

        Assert.AreEqual(2, mask.y0_index);
    }

    [Test]
    public void Rotated90_Line()
    {
        OptimizedMask mask = o.Calculate(3, 1, Vector2.down);

        Assert.AreEqual(
            new int[1, 2] {
                { -1, /*0, */ 1 }},
            mask.coordinates);

        Assert.AreEqual(0, mask.y0_index);
    }

    [Test]
    public void Rotated90_Lines_FacedDown_EvenRowsCase1()
    {
        OptimizedMask mask = o.Calculate(3, 2, Vector2.down);

        Assert.AreEqual(
            new int[2, 2] {
                { -1, /*0, */ 1 },
                { -1, /*0, */ 1 }},
            mask.coordinates);

        Assert.AreEqual(1, mask.y0_index);
    }

    [Test]
    public void Rotated90_Lines_FacedUp_EvenRowsCase2()
    {
        OptimizedMask mask = o.Calculate(3, 2, Vector2.up);

        LogUtil.Log(mask.coordinates);

        Assert.AreEqual(
            new int[2, 2] {
                { -1, /*0, */ 1 },
                { -1, /*0, */ 1 }},
            mask.coordinates);

        Assert.AreEqual(0, mask.y0_index);
    }

    [Test]
    public void Rotated45_Line()
    {
        OptimizedMask mask = o.Calculate(3, 1, new Vector2(1, -1));

        Assert.AreEqual(
            new int[3, 2] {
                                       { 1, 1 },
                             { 0, 0 },
                { -1, -1 }},
            mask.coordinates);

        Assert.AreEqual(1, mask.y0_index);
    }

    [Test]
    public void Rotated135_Line()
    {
        OptimizedMask mask = o.Calculate(3, 1, new Vector2(-1, -1));

        Assert.AreEqual(
            new int[3, 2] {
                { -1, -1 },
                            { 0, 0 },
                                      { 1, 1 }},
            mask.coordinates);

        Assert.AreEqual(1, mask.y0_index);
    }

    [Test]
    public void Rotated225_Line()
    {
        OptimizedMask mask = o.Calculate(3, 1, new Vector2(-1, 1));

        Assert.AreEqual(
            new int[3, 2] {
                                       { 1, 1 },
                             { 0, 0 },
                { -1, -1 }},
            mask.coordinates);

        Assert.AreEqual(1, mask.y0_index);
    }
}
