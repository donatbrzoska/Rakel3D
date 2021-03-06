using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestBasicMaskCalculator
{
    [Test]
    public void InitialState_Line()
    {
        BasicMaskCalculator r = new BasicMaskCalculator();
        bool[,] mask = r.Calculate(3, 1, Vector2.right);

        //Debug.Log("InitialState_Line\n" + r.ToString());

        Assert.AreEqual(
            new bool[7, 7] {
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, true,  false, false, false },
                { false, false, false, true,  false, false, false },
                { false, false, false, true,  false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false }},
            mask);
    }

    [Test]
    public void InitialState_Lines()
    {
        BasicMaskCalculator r = new BasicMaskCalculator();
        bool[,] mask = r.Calculate(3, 2, Vector2.right);

        //Debug.Log("InitialState_Lines\n" + r.ToString());

        Assert.AreEqual(
            new bool[7, 7] {
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, true,  true,  false, false, false },
                { false, false, true,  true,  false, false, false },
                { false, false, true,  true,  false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false }},
            mask);
    }

    [Test]
    public void InitialState_Rectangle()
    {
        BasicMaskCalculator r = new BasicMaskCalculator();
        bool[,] mask = r.Calculate(5, 3, Vector2.right);

        Assert.AreEqual(
            new bool[11, 11] {
                { false, false, false, false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false, false, false, false },
                { false, false, false, true,  true,  true,  false, false, false, false, false },
                { false, false, false, true,  true,  true,  false, false, false, false, false },
                { false, false, false, true,  true,  true,  false, false, false, false, false },
                { false, false, false, true,  true,  true,  false, false, false, false, false },
                { false, false, false, true,  true,  true,  false, false, false, false, false },
                { false, false, false, false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false, false, false, false }},
            mask);
    }

    [Test]
    public void Rotated90_Line()
    {
        BasicMaskCalculator r = new BasicMaskCalculator();
        bool[,] mask = r.Calculate(3, 1, Vector2.down);

        Assert.AreEqual(
            new bool[7, 7] {
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, true,  true,  true,  false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false }},
            mask);
    }

    [Test]
    public void Rotated90_Lines()
    {
        BasicMaskCalculator r = new BasicMaskCalculator();
        bool[,] mask = r.Calculate(3, 2, Vector2.down);

        //Debug.Log("Rotated90_Lines\n" + r.ToString());

        Assert.AreEqual(
            new bool[7, 7] {
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, true,  true,  true,  false, false },
                { false, false, true,  true,  true,  false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false }},
            mask);
    }

    [Test]
    public void Rotated45_Line()
    {
        BasicMaskCalculator r = new BasicMaskCalculator();
        bool[,] mask = r.Calculate(3, 1, new Vector2(1, -1));

        Assert.AreEqual(
            new bool[7, 7] {
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, false, true,  false, false },
                { false, false, false, true,  false, false, false },
                { false, false, true,  false, false, false, false },
                { false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false }},
            mask);
    }

    //[Test]
    //public void InitialState()
    //{
    //    RectangleFootprint r = new RectangleFootprint(3, 1, Vector2.right);
    //    List<Vector2Int> mask = r.GenerateMask();

    //    Assert.AreEqual(
    //        new List<Vector2Int> {
    //            new Vector2Int(0, 1),
    //            new Vector2Int(0, 0),
    //            new Vector2Int(0, -1), },
    //        mask);
    //}
}
