using System;
using UnityEngine;

public class OilPaintSurface : IOilPaintSurface
{
    public IFastTexture2D Texture { get; private set; }

    private PaintReservoir PaintReservoir;

    public OilPaintSurface(IFastTexture2D texture)
    {
        Texture = texture;
        PaintReservoir = new PaintReservoir(texture.Height, texture.Width);

        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < Texture.Height; i++)
        {
            for (int j = 0; j < Texture.Height; j++)
            {
                Texture.SetPixelFast(j, i, Colors.CANVAS_COLOR);
            }
        }
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

    public void Apply()
    {
        Texture.Apply();
    }
}
