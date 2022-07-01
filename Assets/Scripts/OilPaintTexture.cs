using System;
using UnityEngine;

public class OilPaintTexture
{
    public Texture2D Texture { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }

    public OilPaintTexture(int width, int height)
    {
        Texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture.filterMode = FilterMode.Point;
        Initialize();

        Height = Texture.height;
        Width = Texture.width;
    }

    private void Initialize()
    {
        for (int i=0; i<Texture.height; i++)
        {
            for (int j=0; j<Texture.width; j++)
            {
                Texture.SetPixel(j, i, new Color(1, 1, 1, 1));
            }
        }
    }

    // NOTE the array representation mirrors the entire coordinate system by the x axis (for us with the 180° rotated canvas at least)
    public Color[] GetPixels()
    {
        return Texture.GetPixels();
    }

    public Color GetPixel(int x, int y)
    {
        return Texture.GetPixel(x, y);
    }

    public void SetPixel(int x, int y, Color color)
    {
        if (x < Texture.width
            && x >= 0
            && y < Texture.height
            && y >= 0)
            Texture.SetPixel(x, y, color);
    }

    public void Apply()
    {
        Texture.Apply();
    }
}
