using UnityEngine;

class MaskApplicatorMock : LogMock, IMaskApplicator
{
    public void Apply(Mask mask, Vector2Int maskPosition, Vector2 maskNormal, IOilPaintSurface oilPaintSurface, RakelPaintReservoir paintReservoir)
    {
        Log += "Apply ";
    }
}