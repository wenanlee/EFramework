using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <typeparam name="T">要获取或增加的组件。</typeparam>
    /// <param name="gameObject">目标对象。</param>
    /// <returns>获取或增加的组件。</returns>
    public static T GetOrAddComponentt<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <param name="gameObject">目标对象。</param>
    /// <param name="type">要获取或增加的组件类型。</param>
    /// <returns>获取或增加的组件。</returns>
    public static Component GetOrAddComponentt(this GameObject gameObject, Type type)
    {
        Component component = gameObject.GetComponent(type);
        if (component == null)
        {
            component = gameObject.AddComponent(type);
        }

        return component;
    }
    /// <summary>
    /// 在父物体及其子物体中查找指定名称的第一个匹配组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="parent">父物体</param>
    /// <param name="childName">目标物体名称</param>
    /// <returns>找到的组件，未找到返回null</returns>
    public static T GetComponentInChildrenByName<T>(this GameObject parent, string childName)
    {
        if (parent == null || string.IsNullOrEmpty(childName))
            return default;

        // 使用Unity内置的GetComponentsInChildren，性能更好
        var components = parent.GetComponentsInChildren<T>(true);

        foreach (var component in components)
        {
            var gameObject = GetGameObjectFromComponent(component);
            if (gameObject.name == childName)
                return component;
        }

        return default;
    }

    /// <summary>
    /// 从组件获取对应的GameObject
    /// </summary>
    private static GameObject GetGameObjectFromComponent<T>(T component)
    {
        return component switch
        {
            GameObject gameObject => gameObject,
            Component comp => comp.gameObject,
            _ => null
        };
    }

    /// <summary>
    /// Transform版本的扩展方法
    /// </summary>
    public static T GetComponentInChildrenByName<T>(this Transform parent, string childName)
    {
        return parent.gameObject.GetComponentInChildrenByName<T>(childName);
    }
}