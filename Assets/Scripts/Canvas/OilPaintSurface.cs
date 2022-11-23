using System;
using UnityEngine;

public class OilPaintSurface : IOilPaintSurface
{
    public IFastTexture2D Texture { get; private set; }
    public IFastTexture2D NormalMap { get; private set; }

    private PaintReservoir PaintReservoir;

    public OilPaintSurface(IFastTexture2D texture, IFastTexture2D normalMap)
    {
        Texture = texture;
        NormalMap = normalMap;
        PaintReservoir = new PaintReservoir(texture.Height, texture.Width);

        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < Texture.Height; i++)
        {
            for (int j = 0; j < Texture.Width; j++)
            {
                Texture.SetPixelFast(j, i, Colors.CANVAS_COLOR);
                NormalMap.SetPixelFast(j, i, new Color(0.5f, 0.5f, 1));
            }
        }

        Texture.Apply();
        NormalMap.Apply();
    }

    public bool IsInBounds(int x, int y)
    {
        return Texture.IsInBounds(x, y);
    }

    public void AddPaint(int x, int y, Paint paint)
    {
        PaintReservoir.Pickup(x, y, paint);
        UpdateTexture(x, y);
    }

    public Paint GetPaint(int x, int y, int volume)
    {
        Paint emitted = PaintReservoir.Emit(x, y, volume);
        UpdateTexture(x, y);

        return emitted;
    }

    private void UpdateTexture(int x, int y)
    {
        Paint reservoirPaint = PaintReservoir.Get(x, y);
        if (reservoirPaint.IsEmpty())
        {
            Texture.SetPixelFast(x, y, Colors.CANVAS_COLOR);
        }
        else
        {
            Texture.SetPixelFast(x, y, reservoirPaint.Color);
        }
    }

    // inspired by https://stackoverflow.com/a/26357357
    public void UpdateNormal(int x, int y)
    {
        int source_for_filter00 = 0;
        int source_for_filter10 = 0;
        int source_for_filter20 = 0;

        int source_for_filter01 = 0;
        int source_for_filter11 = 0;
        int source_for_filter21 = 0;

        int source_for_filter02 = 0;
        int source_for_filter12 = 0;
        int source_for_filter22 = 0;

        // calculate sources for sobel calculation
        if (x == 0 && y == 0) // lower left special case
        {
            source_for_filter00 = PaintReservoir.Get(0, 0).Volume;
            source_for_filter10 = PaintReservoir.Get(0, 0).Volume;
            source_for_filter20 = PaintReservoir.Get(0, 1).Volume;

            source_for_filter01 = PaintReservoir.Get(0, 0).Volume;
            source_for_filter11 = PaintReservoir.Get(0, 0).Volume;
            source_for_filter21 = PaintReservoir.Get(0, 1).Volume;

            source_for_filter02 = PaintReservoir.Get(0, 1).Volume;
            source_for_filter12 = PaintReservoir.Get(0, 2).Volume;
            source_for_filter22 = PaintReservoir.Get(1, 1).Volume;
        }
        else // normal case
        {
            source_for_filter00 = PaintReservoir.Get(x - 1, y - 1).Volume;
            source_for_filter10 = PaintReservoir.Get(x,     y - 1).Volume;
            source_for_filter20 = PaintReservoir.Get(x + 1, y - 1).Volume;

            source_for_filter01 = PaintReservoir.Get(x - 1, y).Volume;
            source_for_filter11 = PaintReservoir.Get(x,     y).Volume;
            source_for_filter21 = PaintReservoir.Get(x + 1, y).Volume;

            source_for_filter02 = PaintReservoir.Get(x - 1, y + 1).Volume;
            source_for_filter12 = PaintReservoir.Get(x,     y + 1).Volume;
            source_for_filter22 = PaintReservoir.Get(x + 1, y + 1).Volume;
        }


        // do sobel calculation
        int normal_x = -1 * source_for_filter20 + 1 * source_for_filter22
                     + -2 * source_for_filter10 + 2 * source_for_filter12
                     + -1 * source_for_filter00 + 1 * source_for_filter02;

        int normal_y = -1 * source_for_filter20 + -2 * source_for_filter21 + -1 * source_for_filter22
                     +  1 * source_for_filter00 +  2 * source_for_filter01 +  1 * source_for_filter02;

        int normal_z = 1;

        float scale = 0.025f;
        // calculate normal
        Vector3 normal = new Vector3(scale * normal_x, scale * - normal_y, normal_z);/// 2 + new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 normalized = normal.normalized;
        Vector3 halfed = (normalized + new Vector3(1, 1, 1)) / 2;

        // update normal
        Color c = new Color(halfed.x, halfed.y, halfed.z);
        //Debug.Log("old normal was" + NormalMap.GetPixelFast(texture_x, texture_y));
        //Debug.Log("new normal is" + c);
        NormalMap.SetPixelFast(x, y, c);
    }

    public void Apply()
    {
        Texture.Apply();
        NormalMap.Apply(); // TODO TEST THIS
    }
}
