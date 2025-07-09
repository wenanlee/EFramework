using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transform 类的扩展方法集合
/// </summary>
public static class TransformExtensions
{
    /// <summary>
    /// 存储Transform的变换状态（位置/旋转/缩放）
    /// </summary>
    public class Transformation
    {
        public Vector3 P;  // 位置
        public Quaternion R;  // 旋转
        public Vector3 S;  // 缩放
    }

    /// <summary>
    /// 重置Transform的本地变换为初始状态
    /// </summary>
    /// <param name="t">目标Transform</param>
    public static void Identity(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

    /// <summary>
    /// 临时修改Transform并在执行操作后自动还原
    /// </summary>
    /// <param name="t">目标Transform</param>
    /// <param name="f">需要执行的操作</param>
    /// <example>transform.AutoRestore(() => { transform.position = new Vector3(1,2,3); });</example>
    public static void AutoRestore(this Transform t, Action f)
    {
        var r = t.Replicate();  // 保存当前状态
        f();                    // 执行自定义操作
        t.Restore(r);           // 还原原始状态
    }

    /// <summary>
    /// 创建当前Transform状态的副本
    /// </summary>
    /// <returns>包含当前变换数据的Transformation对象</returns>
    public static Transformation Replicate(this Transform t)
    {
        return new Transformation
        {
            P = t.localPosition,
            R = t.localRotation,
            S = t.localScale
        };
    }

    /// <summary>
    /// 从Transformation对象还原变换状态
    /// </summary>
    /// <param name="rep">包含目标状态的Transformation对象</param>
    public static void Restore(this Transform t, Transformation rep)
    {
        t.localPosition = rep.P;
        t.localRotation = rep.R;
        t.localScale = rep.S;
    }

    // 以下方法通过委托函数修改Transform属性
    public static void LocalPosition(this Transform t, Func<Vector3, Vector3> f) => t.localPosition = f(t.localPosition);
    public static void LocalRotation(this Transform t, Func<Quaternion, Quaternion> f) => t.localRotation = f(t.localRotation);
    public static void LocalEulerAngles(this Transform t, Func<Vector3, Vector3> f) => t.localEulerAngles = f(t.localEulerAngles);
    public static void LocalScale(this Transform t, Func<Vector3, Vector3> f) => t.localScale = f(t.localScale);
    public static void Position(this Transform t, Func<Vector3, Vector3> f) => t.position = f(t.position);
    public static void Rotation(this Transform t, Func<Quaternion, Quaternion> f) => t.rotation = f(t.rotation);
    public static void EulerAngles(this Transform t, Func<Vector3, Vector3> f) => t.eulerAngles = f(t.eulerAngles);

    /// <summary>
    /// 获取所有子级Transform（支持递归获取后代）
    /// </summary>
    /// <param name="includesDescendants">是否包含后代对象（默认为false）</param>
    /// <returns>子级/后代Transform的迭代器</returns>
    public static IEnumerable<Transform> Children(this Transform t, bool includesDescendants = false)
    {
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            yield return child;

            if (includesDescendants)
            {
                // 递归遍历所有后代
                foreach (Transform descendant in child.Children(true))
                {
                    yield return descendant;
                }
            }
        }
    }

    /// <summary>
    /// 在后代中递归查找指定名称的Transform
    /// </summary>
    /// <param name="name">目标Transform的名称</param>
    /// <returns>找到的Transform，未找到则返回null</returns>
    public static Transform FindDescendant(this Transform t, string name)
    {
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            if (child.name == name) return child;

            Transform found = child.FindDescendant(name);
            if (found != null) return found;
        }
        return null;
    }

    /// <summary>
    /// 使物体背面朝向目标（用于相机等需要反向观察的场景）
    /// </summary>
    /// <param name="target">目标Transform</param>
    public static void BackwardLookAt(this Transform t, Transform target)
    {
        // 计算目标前方远处点实现反向观察
        t.LookAt(target.position + target.forward * 1000f);
    }

    /// <summary>
    /// 2D平面朝向方法（忽略Y轴高度差）
    /// </summary>
    /// <param name="lookAtPoint2D">二维目标点</param>
    /// <remarks>假设物体初始前方向量为Vector3.up</remarks>
    public static void LookAt2D(this Transform transform, Vector2 lookAtPoint2D)
    {
        Vector3 direction = (Vector3)lookAtPoint2D - transform.position;
        direction.y = 0f;  // 忽略垂直方向差异

        if (direction.magnitude > 0.001f)
        {
            // 在XZ平面创建朝向，保持Y轴向上
            transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }
    }
}