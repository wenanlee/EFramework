using System;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 X(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 Y(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 Z(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }

    public static Vector3 X(this Vector3 v, Func<float, float> f)
    {
        v.x = f(v.x);
        return v;
    }

    public static Vector3 Y(this Vector3 v, Func<float, float> f)
    {
        v.y = f(v.y);
        return v;
    }

    public static Vector3 Z(this Vector3 v, Func<float, float> f)
    {
        v.z = f(v.z);
        return v;
    }
    /// <summary>
    /// 角度计算带符号
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="n">轴向</param>
    /// <returns></returns>
    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }
}