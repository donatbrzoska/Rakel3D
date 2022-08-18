using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TestRakelDrawer
{
    RakelMock RakelMock;
    RakelDrawer RakelDrawer;
    Vector2 DEFAULT_NORMAL = Vector2.right;

    [SetUp]
    public void Init()
    {
        RakelMock = new RakelMock();
        RakelDrawer = new RakelDrawer(RakelMock);
    }

    [Test]
    public void PositionInterpolation_SpecialCase_OnePosition()
    {
        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(0, 0), DEFAULT_NORMAL);

        Assert.AreEqual(
            new List<Vector2Int>
            {
                new Vector2Int(0, 0),
            },
            RakelMock.ApplyPositionLog
        );
    }

    [Test]
    public void PositionInterpolation_ThreePositions()
    {
        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(0, 0), DEFAULT_NORMAL);
        RakelDrawer.AddNode(new Vector2Int(2, 2), DEFAULT_NORMAL);

        Assert.AreEqual(
            new List<Vector2Int>
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 1),
                new Vector2Int(2, 2)
            },
            RakelMock.ApplyPositionLog
        );
    }

    [Test]
    public void PositionInterpolation_MultiLine()
    {
        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(0, 0), DEFAULT_NORMAL);
        RakelDrawer.AddNode(new Vector2Int(0, 1), DEFAULT_NORMAL);

        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(2, 0), DEFAULT_NORMAL);
        RakelDrawer.AddNode(new Vector2Int(2, 1), DEFAULT_NORMAL);

        Assert.AreEqual(
            new List<Vector2Int>
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),

                new Vector2Int(2, 0),
                new Vector2Int(2, 1),
            },
            RakelMock.ApplyPositionLog
        );
    }

    [Test]
    public void NormalInterpolation_SpecialCase_OnePosition()
    {
        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(0, 0), DEFAULT_NORMAL);

        Assert.AreEqual(
            new List<Vector2>
            {
                DEFAULT_NORMAL,
            },
            RakelMock.NormalLog
        );
    }

    [Test]
    public void NormalInterpolation_SpecialCase_OnePositionTwice_ShouldUpdateNormal()
    {
        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(0, 0), Vector2.right);
        RakelDrawer.AddNode(new Vector2Int(0, 0), Vector2.down);

        Assert.AreEqual(
            new List<Vector2>
            {
                Vector2.right,
                Vector2.down
            },
            RakelMock.NormalLog
        );
    }

    [Test]
    public void NormalInterpolation_SpecialCase_TwoPositions()
    {
        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(0, 0), Vector2.right);
        RakelDrawer.AddNode(new Vector2Int(0, 1), Vector2.down);

        LogUtil.Log(RakelMock.NormalLog);
        Assert.AreEqual(
            new List<Vector2>
            {
                Vector2.right,
                Vector2.down
            },
            RakelMock.NormalLog
        );
    }

    [Test]
    public void NormalInterpolation_ThreePositions()
    {
        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(0, 0), Vector2.right);
        RakelDrawer.AddNode(new Vector2Int(0, 2), Vector2.down);

        Assert.AreEqual(
            new List<Vector2>
            {
                Vector2.right,
                (Vector2.right + Vector2.down).normalized,
                Vector2.down
            },
            RakelMock.NormalLog
        );
    }

    [Test]
    public void MultiApply_SamePosition_DifferingNormals_ShouldReapply()
    {
        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(0, 0), Vector2.right);
        RakelDrawer.AddNode(new Vector2Int(0, 0), Vector2.down);

        Assert.AreEqual(
            new List<Vector2Int>
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 0)
            },
            RakelMock.ApplyPositionLog
        );
    }

    [Test]
    public void MultiApply_SamePosition_SameNormal_ShouldNotReapply()
    {
        RakelDrawer.NewStroke();
        RakelDrawer.AddNode(new Vector2Int(0, 0), Vector2.right);
        RakelDrawer.AddNode(new Vector2Int(0, 0), Vector2.right);

        Assert.AreEqual(
            new List<Vector2Int>
            {
                new Vector2Int(0, 0)
            },
            RakelMock.ApplyPositionLog
        );
    }
}

class RakelMock : IRakel
{
    public List<Vector2Int> ApplyPositionLog { get; private set; }
    public List<Vector2> NormalLog { get; private set; }

    public RakelMock()
    {
        ApplyPositionLog = new List<Vector2Int>();
        NormalLog = new List<Vector2>();
    }

    public void UpdateNormal(Vector2 normal, bool logMaskCalcTime = false)
    {
        NormalLog.Add(normal);
    }

    public void ApplyAt(Vector2Int position, bool logMaskApplyTime = false)
    {
        ApplyPositionLog.Add(position);
    }

    public void UpdatePaint(Color color, int volume)
    {
        throw new NotImplementedException();
    }
    public int Length => throw new NotImplementedException();
    public int Width => throw new NotImplementedException();
}