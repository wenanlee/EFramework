using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System;

using UnityEngine;
using EFramework.Core;

/// <summary>
/// 提供命令注册和特性扫描的工具类
/// </summary>
public static class CommandHelper
{
    #region 事件注册模块
    /// <summary>
    /// 事件注册管理器
    /// </summary>
    private static class EventRegistrar
    {
        // 缓存AddListener泛型方法（Key: 事件ID类型全名_参数数量）
        private static readonly ConcurrentDictionary<string, MethodInfo> _addListenerMethodCache = new();

        /// <summary>
        /// 注册事件监听器（自动处理静态/动态方法）
        /// </summary>
        public static void Register<T>(T eventId, MethodInfo method, bool forceInstanceCreation = false)
        {
            if (method.IsStatic)
                RegisterStatic(eventId, method);
            else
                RegisterDynamic(eventId, method, forceInstanceCreation);
        }

        /// <summary>
        /// 注册静态方法监听器
        /// </summary>
        private static void RegisterStatic<T>(T eventId, MethodInfo method)
        {
            if (!method.IsStatic)
                throw new InvalidOperationException("只允许注册静态方法");

            var paramTypes = GetParameterTypes(method);
            var delegateType = CreateDelegateType(paramTypes);
            var handler = CreateDelegate(method, delegateType);

            InvokeAddListener(eventId, paramTypes, handler);
        }

        /// <summary>
        /// 注册实例方法监听器
        /// </summary>
        private static void RegisterDynamic<T>(T eventId, MethodInfo method, bool forceInstanceCreation)
        {
            if (method.IsStatic)
                throw new InvalidOperationException("只允许注册实例方法");

            var paramTypes = GetParameterTypes(method);
            var delegateType = CreateDelegateType(paramTypes);
            var instances = FindMethodInstances(method, forceInstanceCreation);

            foreach (var instance in instances)
            {
                var handler = CreateDelegate(method, delegateType, instance);
                InvokeAddListener(eventId, paramTypes, handler);
            }
        }

        /// <summary>
        /// 获取方法的参数类型数组
        /// </summary>
        private static Type[] GetParameterTypes(MethodInfo method)
        {
            return method.GetParameters()
                .Select(p => p.ParameterType)
                .ToArray();
        }

        /// <summary>
        /// 创建对应的委托类型
        /// </summary>
        private static Type CreateDelegateType(Type[] paramTypes)
        {
            // 使用预定义Action类型优化性能
            return paramTypes.Length switch
            {
                0 => typeof(Action),
                1 => typeof(Action<>).MakeGenericType(paramTypes),
                2 => typeof(Action<,>).MakeGenericType(paramTypes),
                3 => typeof(Action<,,>).MakeGenericType(paramTypes),
                4 => typeof(Action<,,,>).MakeGenericType(paramTypes),
                _ => Expression.GetActionType(paramTypes) // 处理5+参数
            };
        }

        /// <summary>
        /// 创建方法委托
        /// </summary>
        private static Delegate CreateDelegate(MethodInfo method, Type delegateType, object target = null)
        {
            return method.IsStatic
                ? Delegate.CreateDelegate(delegateType, method)
                : Delegate.CreateDelegate(delegateType, target, method);
        }

        /// <summary>
        /// 查找方法所属的实例对象
        /// </summary>
        private static IEnumerable<object> FindMethodInstances(MethodInfo method, bool forceCreate)
        {
            var type = method.DeclaringType;
            var instances = UnityEngine.Object.FindObjectsByType(
                type, FindObjectsInactive.Include, FindObjectsSortMode.None
            );

            if (instances.Length > 0) return instances;

            return forceCreate
                ? new[] { Activator.CreateInstance(type) }
                : throw new InvalidOperationException($"找不到类型实例: {type.FullName}");
        }

        /// <summary>
        /// 调用EventManager的AddListener方法
        /// </summary>
        private static void InvokeAddListener<T>(T eventId, Type[] paramTypes, Delegate handler)
        {
            var cacheKey = $"{typeof(T).FullName}_{paramTypes.Length}";
            var addListenerMethod = _addListenerMethodCache.GetOrAdd(cacheKey, key =>
            {
                return typeof(EventManager).GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(m => m.Name == "AddListener" &&
                                m.IsGenericMethodDefinition &&
                                m.GetParameters().Length == 2 &&
                                m.GetParameters()[0].ParameterType == typeof(T))
                    .FirstOrDefault(m => m.GetGenericArguments().Length == paramTypes.Length)
                    ?? throw new MissingMethodException("未找到匹配的AddListener方法");
            });

            addListenerMethod
                .MakeGenericMethod(paramTypes)
                .Invoke(null, new object[] { eventId, handler });
        }
    }
    #endregion

    #region 特性扫描模块
    /// <summary>
    /// 特性扫描管理器
    /// </summary>
    private static class AttributeScanner
    {
        // 按特性类型缓存方法列表
        public static readonly ConcurrentDictionary<Type, List<MethodInfo>> MethodsByAttribute =
            new();

        // 按方法缓存特性列表
        public static readonly ConcurrentDictionary<MethodInfo, List<Attribute>> AttributesByMethod =
            new();

        /// <summary>
        /// 扫描并缓存带有指定特性的方法
        /// </summary>
        public static void CacheMethodsWithAttributes(params Type[] targetAttributes)
        {
            ClearCache();
            var attributeSet = new HashSet<Type>(targetAttributes);

            foreach (var assembly in GetRelevantAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    ScanTypeMethods(type, attributeSet);
                }
            }
        }

        /// <summary>
        /// 获取需要扫描的程序集
        /// </summary>
        private static IEnumerable<Assembly> GetRelevantAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(asm => !asm.FullName.Contains("UnityEngine") &&
                              !asm.FullName.Contains("mscorlib") &&
                              !asm.FullName.Contains("System") &&
                              !asm.FullName.Contains("Unity.") &&
                              !asm.FullName.Contains("UnityEditor"));
        }

        /// <summary>
        /// 扫描类型中的方法
        /// </summary>
        private static void ScanTypeMethods(Type type, HashSet<Type> targetAttributes)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                     BindingFlags.Instance | BindingFlags.Static;

            foreach (var method in type.GetMethods(flags))
            {
                var attributes = Attribute.GetCustomAttributes(method, true);
                CacheValidAttributes(method, attributes, targetAttributes);
            }
        }

        /// <summary>
        /// 缓存有效的特性关联
        /// </summary>
        private static void CacheValidAttributes(
            MethodInfo method, Attribute[] attributes, HashSet<Type> targetAttributes)
        {
            var validAttributes = attributes
                .Where(attr => targetAttributes.Any(t => t.IsAssignableFrom(attr.GetType())))
                .ToList();

            if (validAttributes.Count == 0) return;

            // 缓存方法到特性映射
            AttributesByMethod.GetOrAdd(method, _ => new List<Attribute>())
                .AddRange(validAttributes);

            // 缓存特性到方法映射
            foreach (var attrType in targetAttributes)
            {
                if (validAttributes.Any(a => attrType.IsAssignableFrom(a.GetType())))
                {
                    MethodsByAttribute.GetOrAdd(attrType, _ => new List<MethodInfo>())
                        .Add(method);
                }
            }
        }

        /// <summary>
        /// 清空特性缓存
        /// </summary>
        private static void ClearCache()
        {
            MethodsByAttribute.Clear();
            AttributesByMethod.Clear();
        }
    }
    #endregion

    // =============== 公共接口 =============== //
    #region 事件注册接口
    /// <summary>
    /// 注册事件监听器
    /// </summary>
    /// <typeparam name="T">事件ID类型</typeparam>
    /// <param name="eventId">事件标识符</param>
    /// <param name="method">监听方法</param>
    /// <param name="forceInstanceCreation">是否强制创建实例</param>
    public static void RegisterListener<T>(T eventId, MethodInfo method, bool forceInstanceCreation = false)
    {
        EventRegistrar.Register(eventId, method, forceInstanceCreation);
    }
    #endregion

    #region 特性扫描接口
    /// <summary>
    /// 获取指定特性的方法缓存
    /// </summary>
    public static ConcurrentDictionary<Type, List<MethodInfo>> MethodCache =>
        AttributeScanner.MethodsByAttribute;

    /// <summary>
    /// 获取方法特性缓存
    /// </summary>
    public static ConcurrentDictionary<MethodInfo, List<Attribute>> AttributeCache =>
        AttributeScanner.AttributesByMethod;

    /// <summary>
    /// 扫描并缓存程序集中带有指定特性的方法
    /// </summary>
    /// <param name="targetAttributes">目标特性类型</param>
    public static void CacheAttributeMethods(params Type[] targetAttributes)
    {
        AttributeScanner.CacheMethodsWithAttributes(targetAttributes);
    }
    #endregion
}