using System;
using Unity.Collections;
using UnityEngine;

public class FastTexture2D : IFastTexture2D
{
    public Texture2D Texture { get; private set; }
    private NativeArray<Color32> Texture_raw;
    public int Height { get; private set; }
    public int Width { get; private set; }

    public FastTexture2D(int width, int height)
    {
        Texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture.filterMode = FilterMode.Point;
        Texture_raw = Texture.GetRawTextureData<Color32>();

        Height = Texture.height;
        Width = Texture.width;
    }

    public Color GetPixelFast(int x, int y)
    {
        if (IsInBounds(x, y))
        {
            int index_1D = y * Width + x;
            return Texture_raw[index_1D];
        }
        else
        {
            return Colors.NO_PAINT_COLOR;
        }
    }

    public void SetPixelFast(int x, int y, Color color)
    {
        if (IsInBounds(x, y))
        {
            int index_1D = y * Width + x;
            Texture_raw[index_1D] = color;
        }
    }

    public bool IsInBounds(int x, int y)
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
