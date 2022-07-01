using System;
using UnityEngine;

public class Rakel
{
    protected int Length;
    protected int Width;
    private Color Color;
    protected Vector2Int Position;
    protected Vector2 Normal;

    public Rakel(int length, int width)
    {
        this.Length = length; // TODO always keep uneven so there is a center to rotate around
        this.Width = width;
    }

    public void UpdateColor(Color color)
    {
        this.Color = color;
    }

    virtual public void UpdateNormal(Vector2 normal)
    {
        this.Normal = normal;
    }

    virtual public void UpdatePosition(Vector2Int position)
    {
        this.Position = position;
    }

    virtual public void ApplyToCanvas(OilPaintTexture texture)
    {
        bool[,] mask = new RectangleFootprint(Length, Width, Normal).GenerateMask();
        ApplyMask(mask, Position, texture);
    }

    protected void ApplyMask(bool[,] mask, Vector2Int maskPosition, OilPaintTexture texture)
    {
        for (int i = 0; i < mask.GetLength(0); i++)
        {
            for (int j = 0; j < mask.GetLength(1); j++) // NOTE mask.GetLength(0) would also work, since the mask is a perfect square
            {
                if (mask[i, j] == true)
                {
                    // NOTE the i needs to be converted to coordinate system space because MaskSpaceToTextureSpace assumes this format
                    Vector2Int tc = MaskSpaceToTextureSpace(j, mask.GetLength(0) - 1 - i, mask.GetLength(0), maskPosition);
                    //Debug.Log("Setting pixel at [" + tc.x + "," + tc.y + "]");
                    texture.SetPixel(tc.x, tc.y, Color);
                }
            }
        }
        texture.Apply();
    }

    /* Mask:
     *    |
     *    0 0 0
     *    0 1 0
     * ---0 0 0-----
     *    |
     *    
     * Texture + Mask with maskPosition M on texture:
     *    |
     *    # # # # # # #
     *    # # # 0 0 0 #
     *    # # # 0 M 0 #
     *    # # # 0 0 0 #
     *    # # # # # # #
     * ---# # # # # # #--
     *    |
     * 
     * MaskPoint 1,1 is now TexturePoint 4,3
     *
     */
    private Vector2Int MaskSpaceToTextureSpace(int maskX, int maskY, int maskSize, Vector2Int maskPosition)
    {
        int maskX0OnTexture = maskPosition.x - maskSize / 2;
        int maskY0OnTexture = maskPosition.y - maskSize / 2;
        return new Vector2Int(maskX0OnTexture + maskX, maskY0OnTexture + maskY);
    }
}
