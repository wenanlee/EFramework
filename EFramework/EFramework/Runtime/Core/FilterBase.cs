using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace EFramework.Core
{
    /// <summary>
    /// 筛选器基类（函数式编程风格）
    /// </summary>
    /// <typeparam name="T">筛选元素类型</typeparam>
    public class Filter<T>
    {
        private readonly IEnumerable<T> _source;

        public Filter(IEnumerable<T> source)
        {
            _source = source;
        }

        /// <summary>
        /// 应用筛选逻辑
        /// </summary>
        public Filter<T> Apply(Func<IEnumerable<T>, IEnumerable<T>> filter)
        {
            return new Filter<T>(filter(_source));
        }

        /// <summary>
        /// 获取第一个结果
        /// </summary>
        public T First()
        {
            return _source.First();
        }

        /// <summary>
        /// 转换为数组
        /// </summary>
        public T[] ToArray()
        {
            return _source.ToArray();
        }
    }

    /// <summary>
    /// 筛选器扩展方法
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// 初始化筛选器（支持任意集合类型）
        /// </summary>
        public static Filter<T> InitFilter<T>(this IEnumerable<T> source)
        {
            return new Filter<T>(source);
        }
    }
}

//public static class TransformFilterExtensions
//{ 
//    /// <summary>
//    /// 距离排序扩展方法
//    /// </summary>
//    /// <param name="referencePoint">参考点位置</param>
//    /// <param name="ascending">是否升序排序</param>
//    public static Filter<Transform> OrderByDistance(
//        this Filter<Transform> filter,
//        Vector3 referencePoint,
//        bool ascending = true)
//    {
//        return filter.Apply(source => ascending ?
//            source.OrderBy(t => Vector3.SqrMagnitude(t.position - referencePoint)) :
//            source.OrderByDescending(t => Vector3.SqrMagnitude(t.position - referencePoint)));
//    }

//    /// <summary>
//    /// 距离筛选扩展方法
//    /// </summary>
//    /// <param name="referencePoint">参考点位置</param>
//    /// <param name="min">最小距离（含）</param>
//    /// <param name="max">最大距离（含）</param>
//    public static Filter<Transform> WhereInDistance(
//        this Filter<Transform> filter,
//        Vector3 referencePoint,
//        float min,
//        float max)
//    {
//        float minSqr = min * min;
//        float maxSqr = max * max;

//        return filter.Apply(source => source.Where(t =>
//        {
//            float sqrMagnitude = Vector3.SqrMagnitude(t.position - referencePoint);
//            return sqrMagnitude >= minSqr && sqrMagnitude <= maxSqr;
//        }));
//    }

//    /// <summary>
//    /// 可见性筛选扩展方法
//    /// </summary>
//    /// <param name="camera">使用的摄像机（默认主摄像机）</param>
//    public static Filter<Transform> WhereVisible(
//        this Filter<Transform> filter,
//        Camera camera = null)
//    {
//        camera = camera ?? Camera.main;
//        if (camera == null) return filter;

//        return filter.Apply(source => source.Where(t =>
//        {
//            Renderer renderer = t.GetComponent<Renderer>();
//            if (renderer == null) return false;

//            return GeometryUtility.TestPlanesAABB(
//                GeometryUtility.CalculateFrustumPlanes(camera),
//                renderer.bounds
//            );
//        }));
//    }
//}

//// 使用示例
//public class FilterExample : MonoBehaviour
//{
//    public Transform[] targets;
//    public Transform player;
//    public float searchRadius = 10f;

//    void Start()
//    {
//        // 链式调用示例
//        Transform nearestVisible = targets
//            .InitFilter()
//            .WhereInDistance(player.position, 2f, searchRadius)
//            .OrderByDistance(player.position)
//            .WhereVisible()
//            .First();

//        Debug.Log("Nearest visible target: " + nearestVisible.name);
//    }
//}