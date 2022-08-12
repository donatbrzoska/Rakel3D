using System;
using UnityEngine;

public interface IMaskCalculator
{
    public Mask Calculate(int height, int width, Vector2 normal);
}
