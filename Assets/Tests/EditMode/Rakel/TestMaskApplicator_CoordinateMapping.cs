using System;
using NUnit.Framework;
using UnityEngine;

// Paint Transfer is tested in TestOptimizedRakel
public class TestMaskApplicator_CoordinateMapping
{
    MaskCalculator mc;
    MaskApplicator ma;

    [SetUp]
    public void Init()
    {
        mc = new MaskCalculator();
        ma = new MaskApplicator();
    }

    // TODO out of bounds GetPaint?
    // TODO unlucky cases

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

        ma.Apply(mask, maskPosition, maskNormal, oilPaintSurface_mock, paintReservoir_mock);

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
                { 0, 1, 0 },
                { 0, 0, 0 },
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

        ma.Apply(mask, maskPosition, maskNormal, oilPaintSurface_mock, paintReservoir_mock);

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
                { 0, 1, 1 },
                { 0, 1, 1 },
                { 0, 1, 1 },
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

        ma.Apply(mask, maskPosition, maskNormal, oilPaintSurface_mock, paintReservoir_mock);

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
                { 1, 1, 1},
                { 1, 1, 1},
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

class PaintReservoirMock : RakelPaintReservoir
{
    public int[,] PickupLog { get; private set; }
    public int[,] EmitLog { get; private set; }

    public PaintReservoirMock(int height, int width) : base(height, width)
    {
        PickupLog = new int[height, width];
        EmitLog = new int[height, width];
    }

    public override void Pickup(int x, int y, Color color, int volume)
    {
        PickupLog[y, x] += 1;
    }

    public override Color Emit(int x, int y)
    {
        EmitLog[y, x] += 1;
        return new Color(0, 0, 0);
    }
}




//using System;
//using NUnit.Framework;
//using UnityEngine;

//// Paint Transfer is tested in TestOptimizedRakel
//public class TestOptimizedMaskApplicator_PositionMapping
//{
//    OptimizedMaskCalculator mc;
//    OptimizedMaskApplicator ma;

//    [SetUp]
//    public void Init()
//    {
//        mc = new OptimizedMaskCalculator();
//        ma = new OptimizedMaskApplicator();
//    }

//    [Test]
//    public void Apply_Point()
//    {
//        int rakelLength = 1;
//        int rakelWidth = 1;
//        Vector2 maskNormal = Vector2.down;
//        OptimizedMask mask = mc.Calculate(rakelLength, rakelWidth, maskNormal);
//        Vector2Int maskPosition = new Vector2Int(1, 1);
//        OilPaintTextureMock oilPaintTexture_mock = new OilPaintTextureMock(3, 3);
//        PaintReservoirMock paintReservoir_mock = new PaintReservoirMock(rakelLength, rakelWidth);

//        ma.Apply(mask, maskPosition, maskNormal, oilPaintTexture_mock, paintReservoir_mock);

//        Assert.AreEqual(
//            "GetPaint(1,1) " // Pickup Paint Loop
//            +
//            "AddPaint(1,1) ", // Emit Paint Loop
//            oilPaintTexture_mock.Log
//        );

//        Assert.AreEqual(
//            "Pickup(0,0) " // Pickup Paint Loop
//            +
//            "Emit(0,0) ", // Emit Paint Loop
//            paintReservoir_mock.Log
//        );
//    }

//    // TODO out of bounds GetPaint

//    [Test]
//    public void Apply_Rectangle()
//    {
//        int rakelLength = 3;
//        int rakelWidth = 2;
//        Vector2 maskNormal = Vector2.right;
//        OptimizedMask mask = mc.Calculate(rakelLength, rakelWidth, maskNormal);
//        Vector2Int maskPosition = new Vector2Int(2, 1);
//        OilPaintTextureMock oilPaintTexture_mock = new OilPaintTextureMock(3, 3);
//        PaintReservoirMock paintReservoir_mock = new PaintReservoirMock(rakelLength, rakelWidth);

//        ma.Apply(mask, maskPosition, maskNormal, oilPaintTexture_mock, paintReservoir_mock);

//        Assert.AreEqual(
//            "GetPaint(1,2) GetPaint(2,2) " +
//            "GetPaint(1,1) GetPaint(2,1) " +
//            "GetPaint(1,0) GetPaint(2,0) "
//            +
//            "AddPaint(1,2) AddPaint(2,2) " +
//            "AddPaint(1,1) AddPaint(2,1) " +
//            "AddPaint(1,0) AddPaint(2,0) ",
//            oilPaintTexture_mock.Log
//        );

//        Assert.AreEqual(
//            "Pickup(0,2) Pickup(0,2) " +
//            "Pickup(0,1) Pickup(0,1) " +
//            "Pickup(0,0) Pickup(0,0) "
//            +
//            "Emit(0,2) Emit(0,2) " +
//            "Emit(0,1) Emit(0,1) " +
//            "Emit(0,0) Emit(0,0) ",
//            paintReservoir_mock.Log
//        );
//    }

//    //[Test]
//    //public void Apply_Rectangle_Rotated90_LargerTextureAsymmetricCase()
//    //{
//    //    OptimizedMask mask = mc.Calculate(3, 2, Vector2.down);
//    //    Vector2Int maskPosition = new Vector2Int(1, 4);
//    //    OilPaintTexture t = new OilPaintTexture(3, 6);

//    //    ma.Apply(mask, maskPosition, t, new Color(0, 0.4f, 0.8f));

//    //    Color[] colors = t.GetPixels();
//    //    //LogUtil.Log(colors, 6);
//    //    AssertUtil.AssertColorsAreEqual(
//    //        // NOTE the written array representation is the real coordinate system but mirrored by the x axis!
//    //        new Color[]
//    //        {
//    //            new Color(1, 1, 1),       new Color(1, 1, 1),       new Color(1, 1, 1),
//    //            new Color(1, 1, 1),       new Color(1, 1, 1),       new Color(1, 1, 1),
//    //            new Color(1, 1, 1),       new Color(1, 1, 1),       new Color(1, 1, 1),
//    //            new Color(1, 1, 1),       new Color(1, 1, 1),       new Color(1, 1, 1),
//    //            new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
//    //            new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f), new Color(0, 0.4f, 0.8f),
//    //        },
//    //        colors
//    //    );
//    //}
//}

//class OilPaintTextureMock : OilPaintTexture
//{
//    public string Log { get; private set; }

//    public OilPaintTextureMock(int width, int height) : base(width, height) { }

//    public override Color GetPaint(int x, int y)
//    {
//        Log += string.Format("GetPaint({0},{1}) ", x, y);
//        return new Color(0, 0, 0);
//    }

//    public override void AddPaint(int texture_x, int texture_xy, Color color)
//    {
//        Log += string.Format("AddPaint({0},{1}) ", texture_x, texture_xy);
//    }
//}

//class PaintReservoirMock : PaintReservoir
//{
//    public string Log { get; private set; }

//    public PaintReservoirMock(int height, int width) : base(height, width) { }

//    public override void Pickup(int reservoir_x, int reservoir_y, Color color)
//    {
//        Log += string.Format("Pickup({0},{1}) ", reservoir_x, reservoir_y);
//    }

//    public override Color Emit(int reservoir_x, int reservoir_y)
//    {
//        Log += string.Format("Emit({0},{1}) ", reservoir_x, reservoir_y);
//        return new Color(0, 0.4f, 0.8f);
//    }
//}