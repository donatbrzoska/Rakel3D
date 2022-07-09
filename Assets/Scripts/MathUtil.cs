using System;
using UnityEngine;

public class MathUtil
{
    private MathUtil(){ }

    // inspired by https://answers.unity.com/questions/1229302/rotate-a-vector2-around-the-z-axis-on-a-mathematic.html
    public static Vector2 RotateAroundOrigin(Vector2 vec, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float s = Mathf.Sin(rad);
        float c = Mathf.Cos(rad);
        Vector2 result = new Vector2(
            vec.x * c + vec.y * s,
            vec.y * c - vec.x * s);
        return result;
    }

    // inspired by https://answers.unity.com/questions/1229302/rotate-a-vector2-around-the-z-axis-on-a-mathematic.html
    public static Vector2Int RotateAroundOrigin(Vector2Int vec, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float s = Mathf.Sin(rad);
        float c = Mathf.Cos(rad);
        Vector2Int result = new Vector2Int(
            Mathf.RoundToInt(vec.x * c + vec.y * s),
            Mathf.RoundToInt(vec.y * c - vec.x * s));
        return result;
    }

    // increasing clockwise
    public static float Angle360(Vector2 from, Vector2 to)
    {
        float angle = Vector2.SignedAngle(from, to);

        if (angle <= 0) // SignedAngle delivers negative values for the first 180° clockwise
        {
            angle = -angle;
        }
        else // .. and positive values for the first 180° counter clockwise
        {
            angle = 360 - angle;
        }

        return angle;
    }
}
