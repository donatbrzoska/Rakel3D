﻿using System;
using UnityEngine;

public interface IMaskApplicator
{
    public void Apply(
        Mask mask,
        Vector2Int maskPosition,
        Vector2 maskNormal,
        IOilPaintSurface oilPaintSurface,
        IRakelPaintReservoir paintReservoir);
}