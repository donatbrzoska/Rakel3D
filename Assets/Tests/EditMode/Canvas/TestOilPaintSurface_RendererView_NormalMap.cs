using System;
using NUnit.Framework;
using UnityEngine;

public class TestOilPaintSurface_RendererView_NormalMap
{
    int[,] sobel_x = new int[,]
    {
        {-1, 0, 1},
        {-2, 0, 2},
        {-1, 0, 1},
    };
    int[,] sobel_y = new int[,]
    {
        {-1, -2, -1},
        { 0,  0,  0},
        { 1,  2,  1},
    };
    int[,] volumes;

    [SetUp]
    public void Init()
    {
        volumes = new int[3, 3];
    }

    [Test]
    public void InitialState()
    {
        FastTexture2D texture = new FastTexture2D(2, 1);
        FastTexture2D normalMap = new FastTexture2D(2, 1);
        OilPaintSurface oilPaintSurface = new OilPaintSurface(texture, normalMap);

        Color[] normals = oilPaintSurface.NormalMap.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0.502f, 0.502f, 1f), new Color(0.502f, 0.502f, 1f),
            },
            normals
        );
    }


    [Test]
    public void AddPaint_LowerLeft()
    {
        FastTexture2D texture = new FastTexture2D(3, 3);
        FastTexture2D normalMap = new FastTexture2D(3, 3);
        OilPaintSurface oilPaintSurface = new OilPaintSurface(texture, normalMap);

        oilPaintSurface.AddPaint(0, 0, new Paint(new Color(0.2f, 0.4f, 0.6f), 1));

        volumes[1, 0] = 5;
        // 1. lower left normal
        // volume indices are swapped x,y!
        int source_for_filter00 = volumes[0, 0];
        int source_for_filter10 = volumes[0, 0];
        int source_for_filter20 = volumes[1, 0];

        int source_for_filter01 = volumes[0, 0];
        int source_for_filter11 = volumes[0, 0];
        int source_for_filter21 = volumes[1, 0];

        int source_for_filter02 = volumes[1, 0];
        int source_for_filter12 = volumes[1, 0];
        int source_for_filter22 = volumes[1, 1];

        // filter indices are swapped x,y!
        int x = -1 * source_for_filter20 + 1 * source_for_filter22
              + -2 * source_for_filter10 + 2 * source_for_filter12
              + -1 * source_for_filter00 + 1 * source_for_filter02;

        // filter indices are swapped x,y!
        int y = -1 * source_for_filter20 + -2 * source_for_filter21 + -1 * source_for_filter22
              +  1 * source_for_filter00 +  2 * source_for_filter01 +  1 * source_for_filter02;
             
        int z = 1;

        Vector3 normal = new Vector3(x, y, z) ;/// 2 + new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 normalized = normal.normalized;
        Vector3 halfed = (normalized + new Vector3(1, 1, 1)) / 2;

        Debug.Log(normal);
        Debug.Log(normalized);
        Debug.Log(halfed);

        //Vector3 p7_triangle_vec1 = p8 - p7;
        //Vector3 p7_triangle_vec2 = p4 - p7;
        //Vector3 p7_normal = Vector3.Cross(p7_triangle_vec1, p7_triangle_vec2);
        //Debug.Log(p7_normal);
        //Vector3 p7_normalized = p7_normal.normalized;
        //Debug.Log(p7_normalized);
        //Vector3 p7_halfed = (p7_normalized + new Vector3(1, 1, 1)) / 2;
        //Debug.Log(p7_halfed); // <- result for 0,0

        Color[] normals = oilPaintSurface.NormalMap.Texture.GetPixels();
        AssertUtil.AssertColorsAreEqual(
            new Color[]
            {
                new Color(0.83f, 0.83f, 0.67f), Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,            Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
                Colors.CANVAS_COLOR,            Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
            },
            normals
        );
    }

    //[Test]
    //public void AddPaint_EMPTY_PAINT_DoesNothing()
    //{
    //    oilPaintSurface.AddPaint(0, 0, new Paint(new Color(0.2f, 0.2f, 0.2f), 1));

    //    oilPaintSurface.AddPaint(0, 0, Paint.EMPTY_PAINT);

    //    Color[] colors = fastTexture2D.Texture.GetPixels();
    //    AssertUtil.AssertColorsAreEqual(
    //        new Color[]
    //        {
    //            new Color(0.2f, 0.2f, 0.2f), Colors.CANVAS_COLOR,
    //            Colors.CANVAS_COLOR,         Colors.CANVAS_COLOR,
    //        },
    //        colors
    //    );
    //}

    //[Test]
    //public void GetPaint()
    //{
    //    oilPaintSurface.AddPaint(0, 0, new Paint(new Color(0.2f, 0.2f, 0.2f), 1));
    //    oilPaintSurface.GetPaint(0, 0, 1);

    //    // added paint should be gone again
    //    Color[] colors = fastTexture2D.Texture.GetPixels();
    //    AssertUtil.AssertColorsAreEqual(
    //        new Color[]
    //        {
    //            Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
    //            Colors.CANVAS_COLOR, Colors.CANVAS_COLOR,
    //        },
    //        colors
    //    );
    //}
}
