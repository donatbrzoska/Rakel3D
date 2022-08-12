using System;
using UnityEngine;

public interface IFastTexture2D
{
    public int Height { get; }
    public int Width { get; }
    public bool PixelInBounds(int x, int y);
    public Color GetPixelFast(int x, int y);
    public void SetPixelFast(int x, int y, Color color);
    public void Apply();
}
