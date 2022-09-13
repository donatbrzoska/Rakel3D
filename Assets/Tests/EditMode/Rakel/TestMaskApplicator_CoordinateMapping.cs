using System;
using NUnit.Framework;
using UnityEngine;

public class TestMaskApplicator_CoordinateMapping
{
    IMaskCalculator mc;
    IMaskApplicator ma;

    [SetUp]
    public void Init()
    {
        mc = new MaskCalculator();
        ma = new MaskApplicator();
    }

    [Test]
    public void Apply_Point()
    {
        int rakelLength = 1;
        int rakelWidth = 1;
        Vector2 maskNormal = Vector2.down;
        Mask mask = mc.Calculate(rakelLength, rakelWidth, maskNormal);
        Vector2Int maskPosition = new Vector2Int(1, 1);
        OilPaintSurfaceMock oilPaintSurface_mock = new OilPaintSurfaceMock(3, 3);
        PaintReservoirMock paintReservoir_mock = new PaintReservoirMock(rakelLength, rakelWidth);

        ma.Apply(mask, maskPosition, oilPaintSurface_mock, paintReservoir_mock);

        // Pickup Paint Loop
        Assert.AreEqual(
            new int[,]
            {
                { 0, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 0 },
            },
            oilPaintSurface_mock.GetPaintLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 1 }
            },
            paintReservoir_mock.PickupLog
        );

        // Emit Paint Loop
        Assert.AreEqual(
            new int[,]
            {
                { 1 }
            },
            paintReservoir_mock.EmitLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 0, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 0 },
            },
            oilPaintSurface_mock.AddPaintLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 0, 0, 0 },
                { 0, 2, 0 },
                { 0, 0, 0 },
            },
            oilPaintSurface_mock.IsInBoundsLog
        );
    }

    // This was added, because somehow a case with a similar line does not work in TestRakel
    // (This test did not identify a bug, so maybe there is not much value in it)
    [Test]
    public void Apply_Line()
    {
        int rakelLength = 3;
        int rakelWidth = 1;
        Vector2 maskNormal = Vector2.right;
        Mask mask = mc.Calculate(rakelLength, rakelWidth, maskNormal);
        Vector2Int maskPosition = new Vector2Int(1, 1);
        OilPaintSurfaceMock oilPaintSurface_mock = new OilPaintSurfaceMock(3, 3);
        PaintReservoirMock paintReservoir_mock = new PaintReservoirMock(rakelLength, rakelWidth);

        ma.Apply(mask, maskPosition, oilPaintSurface_mock, paintReservoir_mock);

        // Pickup Paint Loop
        Assert.AreEqual(
            new int[,]
            {
                { 0, 1, 0 },
                { 0, 1, 0 },
                { 0, 1, 0 },
            },
            oilPaintSurface_mock.GetPaintLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 1 },
                { 1 },
                { 1 }
            },
            paintReservoir_mock.PickupLog
        );

        // Emit Paint Loop
        Assert.AreEqual(
            new int[,]
            {
                { 1 },
                { 1 },
                { 1 }
            },
            paintReservoir_mock.EmitLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 0, 1, 0 },
                { 0, 1, 0 },
                { 0, 1, 0 },
            },
            oilPaintSurface_mock.AddPaintLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 0, 2, 0 },
                { 0, 2, 0 },
                { 0, 2, 0 },
            },
            oilPaintSurface_mock.IsInBoundsLog
        );
    }

    [Test]
    public void Apply_Rectangle()
    {
        int rakelLength = 3;
        int rakelWidth = 2;
        Vector2 maskNormal = Vector2.right;
        Mask mask = mc.Calculate(rakelLength, rakelWidth, maskNormal);
        Vector2Int maskPosition = new Vector2Int(2, 1);
        OilPaintSurfaceMock oilPaintSurface_mock = new OilPaintSurfaceMock(3, 3);
        PaintReservoirMock paintReservoir_mock = new PaintReservoirMock(rakelLength, rakelWidth);

        ma.Apply(mask, maskPosition, oilPaintSurface_mock, paintReservoir_mock);

        // Pickup Paint Loop
        Assert.AreEqual(
            new int[,]
            {
                { 0, 1, 1 },
                { 0, 1, 1 },
                { 0, 1, 1 },
            },
            oilPaintSurface_mock.GetPaintLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 1, 1 },
                { 1, 1 },
                { 1, 1 },
            },
            paintReservoir_mock.PickupLog
        );

        // Emit Paint Loop
        Assert.AreEqual(
            new int[,]
            {
                { 1, 1 },
                { 1, 1 },
                { 1, 1 },
            },
            paintReservoir_mock.EmitLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 0, 1, 1 },
                { 0, 1, 1 },
                { 0, 1, 1 },
            },
            oilPaintSurface_mock.AddPaintLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 0, 2, 2 },
                { 0, 2, 2 },
                { 0, 2, 2 },
            },
            oilPaintSurface_mock.IsInBoundsLog
        );
    }

    [Test]
    public void Apply_Rectangle_Rotated90_LargerTextureAsymmetricCase()
    {
        int rakelLength = 3;
        int rakelWidth = 2;
        Vector2 maskNormal = Vector2.down;
        Mask mask = mc.Calculate(rakelLength, rakelWidth, maskNormal);
        Vector2Int maskPosition = new Vector2Int(1, 4);
        OilPaintSurfaceMock oilPaintSurface_mock = new OilPaintSurfaceMock(3, 6);
        PaintReservoirMock paintReservoir_mock = new PaintReservoirMock(rakelLength, rakelWidth);

        ma.Apply(mask, maskPosition, oilPaintSurface_mock, paintReservoir_mock);

        // Pickup Paint Loop
        Assert.AreEqual(
            // REMEMBER: arrays are upside down coordinate systems
            new int[,]
            {
                { 0, 0, 0},
                { 0, 0, 0},
                { 0, 0, 0},
                { 0, 0, 0},
                { 1, 1, 1},
                { 1, 1, 1},
            },
            oilPaintSurface_mock.GetPaintLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 1, 1 },
                { 1, 1 },
                { 1, 1 },
            },
            paintReservoir_mock.PickupLog
        );

        // Emit Paint Loop
        Assert.AreEqual(
            new int[,]
            {
                { 1, 1 },
                { 1, 1 },
                { 1, 1 },
            },
            paintReservoir_mock.EmitLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 0, 0, 0},
                { 0, 0, 0},
                { 0, 0, 0},
                { 0, 0, 0},
                { 1, 1, 1},
                { 1, 1, 1},
            },
            oilPaintSurface_mock.AddPaintLog
        );
        Assert.AreEqual(
            new int[,]
            {
                { 0, 0, 0},
                { 0, 0, 0},
                { 0, 0, 0},
                { 0, 0, 0},
                { 2, 2, 2},
                { 2, 2, 2},
            },
            oilPaintSurface_mock.IsInBoundsLog
        );
    }
}

class OilPaintSurfaceMock : IOilPaintSurface
{
    public int[,] IsInBoundsLog { get; private set; }
    public int[,] GetPaintLog { get; private set; }
    public int[,] AddPaintLog { get; private set; }

    public OilPaintSurfaceMock(int width, int height)
    {
        IsInBoundsLog = new int[height, width];
        GetPaintLog = new int[height, width];
        AddPaintLog = new int[height, width];
    }

    public bool IsInBounds(int x, int y)
    {
        IsInBoundsLog[y, x] += 1;
        return true;
    }

    public Color GetPaint(int x, int y)
    {
        GetPaintLog[y, x] += 1;
        return new Color(0, 0, 0);
    }

    public void AddPaint(int x, int y, Color color)
    {
        AddPaintLog[y, x] += 1;
    }

    public void Apply()
    {

    }
}

class PaintReservoirMock : IRakelPaintReservoir
{
    public int[,] PickupLog { get; private set; }
    public int[,] EmitLog { get; private set; }

    public int Height { get;  private set; }
    public int Width { get; private set; }

    public PaintReservoirMock(int height, int width)
    {
        PickupLog = new int[height, width];
        EmitLog = new int[height, width];

        Width = width;
        Height = height;
    }

    public void Pickup(int x, int y, Color color, int volume)
    {
        PickupLog[y, x] += 1;
    }

    public Color Emit(int x, int y)
    {
        EmitLog[y, x] += 1;
        return new Color(0, 0, 0);
    }

    public void Fill(Color color, int volume)
    {
        throw new NotImplementedException();
    }
}