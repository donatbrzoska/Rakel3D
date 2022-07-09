using System;
using Unity.Collections;
using UnityEngine;

public class OilPaintTexture
{
    public Texture2D Texture { get; private set; }
    private NativeArray<Color32> Texture_raw;
    public int Height { get; private set; }
    public int Width { get; private set; }

    public OilPaintTexture(int width, int height)
    {
        Texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture.filterMode = FilterMode.Point;
        Texture_raw = Texture.GetRawTextureData<Color32>();
        Initialize();

        Height = Texture.height;
        Width = Texture.width;
    }

    private void Initialize()
    {
        for (int i = 0; i < Texture.height; i++)
        {
            for (int j = 0; j < Texture.width; j++)
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

    public void SetPixelFast(int x, int y, Color color)
    {
        if (PixelInBounds(x, y))
        {
            int index_1D = y * Width + x;
            Texture_raw[index_1D] = color;
        }
    }

    public void SetPixel(int x, int y, Color color)
    {
        if (PixelInBounds(x, y))
            Texture.SetPixel(x, y, color);
    }

    private bool PixelInBounds(int x, int y)
    {
        return x < Width
            && x >= 0
            && y < Height
            && y >= 0;
    }

    public void Apply()
    {
        Texture.Apply();
    }
}
