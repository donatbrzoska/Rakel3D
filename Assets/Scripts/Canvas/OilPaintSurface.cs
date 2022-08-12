using System;
using UnityEngine;

public class OilPaintSurface : IOilPaintSurface
{
    public IFastTexture2D Texture { get; private set; }

    public OilPaintSurface(IFastTexture2D texture)
    {
        Texture = texture;
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
        return Texture.PixelInBounds(x, y);
    }

    public void AddPaint(int x, int y, Color color)
    {
        if (!color.Equals(Colors.NO_PAINT_COLOR))
        {
            Texture.SetPixelFast(x, y, color);
        }
    }

    public Color GetPaint(int x, int y)
    {
        Color color = Texture.GetPixelFast(x, y);
        if (color.Equals(Colors.CANVAS_COLOR))
        {
            color = Colors.NO_PAINT_COLOR;
        }
        Texture.SetPixelFast(x, y, Colors.CANVAS_COLOR);
        return color;
    }

    public void Apply()
    {
        Texture.Apply();
    }
}
