using UnityEngine;

class MaskApplicatorMock : MaskApplicator
{
    public string Log { get; private set; }
    public override void Apply(Mask mask, Vector2Int maskPosition, Vector2 maskNormal, IOilPaintSurface oilPaintSurface, RakelPaintReservoir paintReservoir)
    {
        Log += "Apply ";
    }
}