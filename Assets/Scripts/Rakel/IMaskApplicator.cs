using System;
using UnityEngine;

public interface IMaskApplicator
{
    public void Apply(
        Mask mask,
        Vector2Int maskPosition,
        IOilPaintSurface oilPaintSurface,
        IRakelPaintReservoir paintReservoir);
}
