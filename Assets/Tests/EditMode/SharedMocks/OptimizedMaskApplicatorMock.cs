using UnityEngine;

class OptimizedMaskApplicatorMock : OptimizedMaskApplicator
{
    public string Log { get; private set; }
    public override void Apply(OptimizedMask mask, Vector2Int maskPosition, Vector2 maskNormal, IOilPaintSurface oilPaintSurface, RakelPaintReservoir paintReservoir)
    {
        Log += "Apply ";
    }
}