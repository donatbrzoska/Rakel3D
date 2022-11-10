﻿using System;
using UnityEngine;

public class Paint : IEquatable<Paint>
{
    public static Paint EMPTY_PAINT { get; private set; } = new Paint(Colors.NO_PAINT_COLOR, 0);

    public static Paint operator +(Paint a, Paint b)
    {
        if (a.IsEmpty() && b.IsEmpty())
        {
            return EMPTY_PAINT;
        }

        float summed_volume = a.Volume + b.Volume;
        float a_part = a.Volume / summed_volume;
        float b_part = b.Volume / summed_volume;

        return new Paint(
            new Color(
                a.Color.r * a_part + b.Color.r * b_part,
                a.Color.g * a_part + b.Color.g * b_part,
                a.Color.b * a_part + b.Color.b * b_part
            ),
            (int) summed_volume);
    }

    public Color Color { get; set; }
    public int Volume { get; set; }

    public Paint(Color color, int volume)
    {
        Color = color;
        Volume = volume;
    }

    public Paint(Paint paint)
    {
        Color = paint.Color;
        Volume = paint.Volume;
    }

    public bool IsEmpty()
    {
        return this.Equals(EMPTY_PAINT);
    }

    public bool Equals(Paint other)
    {
        return ColorsEqual(Color, other.Color) && Volume == other.Volume;
    }

    public override string ToString()
    {
        return string.Format("Paint(r={0}, g={1}, b={2}, vol={3})", Color.r, Color.g, Color.b, Volume);
    }

    private static bool ColorsEqual(Color expected, Color actual)
    {
        bool equal = FloatsEqual(expected.a, actual.a)
            && FloatsEqual(expected.r, actual.r)
            && FloatsEqual(expected.g, actual.g)
            && FloatsEqual(expected.b, actual.b);
        return equal;
    }

    private static bool FloatsEqual(float a, float b, float precision = 0.001f)
    {
        return Mathf.Abs(a - b) < precision;
    }
}
