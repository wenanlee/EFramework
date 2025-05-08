using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ±´Èû¶û¹¤¾ßÀà
/// </summary>
public static class BezierUtils
{
    /// <summary>
    /// ÏßÐÔ±´Èû¶ûÇúÏß
    /// </summary>
    public static Vector3 BezierCurve(Vector3 p0, Vector3 p1, float t)
    {
        Vector3 B = Vector3.zero;
        B = (1 - t) * p0 + t * p1;
        return B;
    }

    /// <summary>
    /// ¶þ½×±´Èû¶ûÇúÏß
    /// </summary>
    public static Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 B = Vector3.zero;
        float t1 = (1 - t) * (1 - t);
        float t2 = 2 * t * (1 - t);
        float t3 = t * t;
        B = t1 * p0 + t2 * p1 + t3 * p2;
        return B;
    }

    /// <summary>
    /// Èý½×±´Èû¶ûÇúÏß
    /// </summary>
    public static Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 B = Vector3.zero;
        float t1 = (1 - t) * (1 - t) * (1 - t);
        float t2 = 3 * t * (1 - t) * (1 - t);
        float t3 = 3 * t * t * (1 - t);
        float t4 = t * t * t;
        B = t1 * p0 + t2 * p1 + t3 * p2 + t4 * p3;
        return B;
    }

    /// <summary>
    /// n½×±´Èû¶ûÇúÏß
    /// </summary>
    public static Vector3 BezierCurve(List<Vector3> pointList, float t)
    {
        Vector3 B = Vector3.zero;
        if (pointList == null)
        {
            return B;
        }
        if (pointList.Count < 2)
        {
            return pointList[0];
        }

        List<Vector3> tempPointList = new List<Vector3>();
        for (int i = 0; i < pointList.Count - 1; i++)
        {
            Vector3 tempPoint = BezierCurve(pointList[i], pointList[i + 1], t);
            tempPointList.Add(tempPoint);
        }
        return BezierCurve(tempPointList, t);
    }
}