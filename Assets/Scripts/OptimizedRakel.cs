using System;
using UnityEngine;

public class OptimizedRakel : Rakel
{
    private Vector2Int PreviousPosition;
    private Vector2 PreviousNormal;
    private bool[,] LatestMask;

    public OptimizedRakel(int length, int width) : base(length, width)
    {

    }

    override public void UpdateNormal(Vector2 normal)
    {
        PreviousNormal = Normal;
        Normal = normal;
    }

    override public void UpdatePosition(Vector2Int position)
    {
        PreviousPosition = Position;
        Position = position;
    }

    override public void ApplyToCanvas(OilPaintTexture texture)
    {
        bool recalculateMask = !PreviousNormal.Equals(Normal);
        if (recalculateMask)
        {
            LatestMask = new RectangleFootprint(Length, Width, Normal).GenerateMask();
        }

        bool reapplyMask = recalculateMask || !PreviousPosition.Equals(Position);
        if (reapplyMask)
        {
            ApplyMask(LatestMask, Position, texture);
        }
    }
}
