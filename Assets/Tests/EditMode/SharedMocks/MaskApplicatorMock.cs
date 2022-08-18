using UnityEngine;

class MaskApplicatorMock : LogMock, IMaskApplicator
{
    public void Apply(Mask mask, Vector2Int maskPosition, IOilPaintSurface oilPaintSurface, IRakelPaintReservoir paintReservoir)
    {
        Log += "Apply ";
    }
}