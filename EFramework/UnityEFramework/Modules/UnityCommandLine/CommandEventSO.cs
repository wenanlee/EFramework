using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using CommandTerminal;
using EditorAttributes;
using EFramework.Core;
using EFramework.Unity.Command;
using EFramework.Unity.Utility;
using UnityEngine;

namespace EFramework.Unity.Command
{
    [CreateAssetMenu(fileName = "CommandEventSO", menuName = "EFramework/CommandEventSO")]
    public class CommandEventSO : SOBase
    {
        //[DataTable(true,true)]
        public List<CommandEventArgs> commandEvents = new List<CommandEventArgs>();


        //public void RegisterAllCommands()
        //{
        //    foreach (var commandEvent in commandEvents)
        //    {
        //        commandEvent.AddEventListener();
        //    }
        //}
        public override void ReLoadSO()
        {
            //GetAllMethods();
        }
        /// <summary>
        /// 获取所有带CommandInstance的方法
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [Button("获取所有命令")]
        public void GetAllMethods()
        {
            CommandHelp.CacheMethodsWithAttributes(new Type[] { typeof(RegisterCommandLine), typeof(ButtonAttribute) });
            CommandHelp.MethodCacheByAttributeType[typeof(RegisterCommandLine)].ForEach(method =>
            {
                CommandHelp.AttributeCacheByMethod[method].ForEach(attribute =>
                {
                    string commandName = string.IsNullOrEmpty((attribute as RegisterCommandLine).Name) ? method.DeclaringType.FullName + "." + method.Name : (attribute as RegisterCommandLine).Name;
                    commandEvents.Add(new CommandEventArgs(commandName, method));
                });
            });
        }
        [Button("注册所有命令")]
        public void test(int a, string b)
        {

        }
    }
    [Serializable]

    /// <summary>
    /// 表示包含方法调用信息的命令事件参数
    /// </summary>
    public class CommandEventArgs
    {
        /// <summary>
        /// 获取命令名称
        /// </summary>
        [ShowInInspector, Rename("命令名称")] public string CommandName;
        /// <summary>
        /// 获取命令的唯一标识符
        /// </summary>
        [ShowInInspector] public string Uuid;
        /// <summary>
        /// 获取声明方法的类型全限定名称
        /// </summary>
        [ShowInInspector] public string CommandTypeStr;
        /// <summary>
        /// 获取要调用的方法名称
        /// </summary>
        [ShowInInspector] public string CommandMethodStr;
        /// <summary>
        /// 获取方法参数类型的全限定名称数组
        /// </summary>
        [ShowInInspector] public string[] CommandArgsTypeStrs;
        private MethodInfo CommandMethod { get; set; }
        private Dictionary<object, Component> CommandInstanceDict { get; set; } = new Dictionary<object, Component>(); // 实例列表
        private Type CommandType { get; set; }
        [ShowInInspector]
        public object[] CommandArgs { get; set; }
        public CommandEventArgs(string commandName, MethodInfo method)
        {
            Uuid = UUID.New();
            CommandName = commandName;
            CommandMethodStr = method.Name;
            CommandTypeStr = method.DeclaringType.AssemblyQualifiedName;
            CommandArgsTypeStrs = method.GetParameters()
                                        .Select(p => p.ParameterType.AssemblyQualifiedName)
                                        .ToArray();
            CreateParameterInstances();
        }

        private void AddEventListener()
        {
            if (EventManager.CheckHaveListener(CommandName) == false)
                EventManager.AddListener<GameObject, object[]>(CommandName, Invoke);
        }

        public void InvokeStatic(params object[] args)
        {
            Invoke(null, args);
        }
        public void Invoke(object targetInstance, params object[] args)
        {
            try
            {
                if (CommandType == null)
                    CommandType = Type.GetType(CommandTypeStr);

                //Component instance = null;
                //if (targetInstance != null)
                //{
                //    if (CommandInstanceDict.ContainsKey(targetInstance) == false)
                //    {

                //        if (targetInstance is GameObject go && typeof(MonoBehaviour).IsAssignableFrom(CommandType))
                //        {
                //            // 尝试获取组件
                //            instance = go.GetComponent(CommandType);
                //        }
                //    }
                //}
                GetMethodInfo();

                // 静态方法不能有实例
                if (CommandMethod.IsStatic == false && targetInstance == null)
                    targetInstance = Activator.CreateInstance(CommandType);

                // 6. 调用方法并返回结果
                CommandMethod.Invoke(targetInstance, args);
            }
            catch (Exception ex)
            {
                // 处理反射错误
                Debug.LogError("反射错误: " + ex);
            }
        }
        public object[] CreateParameterInstances()
        {
            if (CommandArgsTypeStrs == null || CommandArgsTypeStrs.Length == 0)
                return null;
            if (CommandArgs == null)
            {
                var paramTypes = CommandArgsTypeStrs.Select(t => Type.GetType(t)).ToArray();
                CommandArgs = paramTypes.Select(p => p == typeof(string) ? string.Empty : Activator.CreateInstance(p)).ToArray();
            }
            return CommandArgs;
        }
        public Type GetTypeFromStr(string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr))
                return null;
            Type type = Type.GetType(typeStr);
            if (type == null)
            {
                Debug.LogError($"无法找到类型: {typeStr}");
            }
            return type;
        }
        public MethodInfo GetMethodInfo()
        {
            if (CommandMethod == null)
            {
                // 如果没有找到方法，尝试获取
                CommandMethod = CommandType.GetMethod(CommandMethodStr, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, null, CommandArgsTypeStrs.Select(t => Type.GetType(t)).ToArray(), null);
            }
            return CommandMethod;
        }
        //public void Invoke(GameObject go, object[] args)
        //{
        //    ReflectionHelper.InvokeMethod(path[0], path[1], path[2], go, args);
        //}
    }
}
public static class CommandHelp
{
    #region 这里是用来把命令注册成委托的
    private static readonly ConcurrentDictionary<string, MethodInfo> _addListenerMethodCache = new();
    /// <summary>
    /// 注册事件监听器到EventManager中,使所有的调用方法统一
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eid"></param>
    /// <param name="method"></param>
    /// <param name="forceInstanceCreation"></param>
    public static void RegisterEventListener<T>(T eid, MethodInfo method, bool forceInstanceCreation = false)
    {
        if (method.IsStatic)
        {
            RegisterStaticEventListener(eid, method);
        }
        else
        {
            RegisterDynamicEventListener(eid, method, forceInstanceCreation);
        }
    }
    /// <summary>
    /// 把反射出来的方法注册到EventManager中,使所有的调用方法统一
    /// </summary>
    /// <param name="eid">命令名称全局唯一</param>
    /// <param name="method">反射出来的方法</param>
    public static void RegisterStaticEventListener<T>(T eid, MethodInfo method)
    {
        if(method.IsStatic == false)
        {
            throw new InvalidOperationException("方法必须是静态的");
        }
        // 1. 获取委托参数类型
        ParameterInfo[] parameters = method.GetParameters();
        Type[] paramTypes = new Type[parameters.Length];
        string key = $"{eid.GetType().FullName}_{parameters.Length}";

        // 直接填充类型数组避免LINQ
        for (int i = 0; i < parameters.Length; i++)
        {
            paramTypes[i] = parameters[i].ParameterType;
        }

        // 2. 从缓存获取或创建AddListener泛型方法
        if (!_addListenerMethodCache.TryGetValue(key, out MethodInfo genericMethod))
        {
            lock (_addListenerMethodCache)
            {
                if (!_addListenerMethodCache.TryGetValue(key, out genericMethod))
                {
                    // 获取所有候选方法
                    var candidates = typeof(EventManager)
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .Where(m =>
                            m.Name == "AddListener" &&
                            m.IsGenericMethodDefinition &&
                            m.GetParameters().Length == 2 &&
                            m.GetParameters()[0].ParameterType == eid.GetType())
                        .ToList();

                    // 找到泛型参数数量匹配的方法
                    genericMethod = candidates.FirstOrDefault(m =>
                        m.GetGenericArguments().Length == parameters.Length);

                    if (genericMethod == null)
                    {
                        throw new MissingMethodException("没有发现AddListener方法");
                    }

                    _addListenerMethodCache[key] = genericMethod;
                }
            }
        }

        // 3. 优化委托创建
        Type delegateType;
        if (parameters.Length <= 4)
        {
            // 使用预定义的Action类型
            Type actionType = parameters.Length switch
            {
                0 => typeof(Action),
                1 => typeof(Action<>),
                2 => typeof(Action<,>),
                3 => typeof(Action<,,>),
                4 => typeof(Action<,,,>),
                _ => null
            };

            delegateType = actionType.MakeGenericType(paramTypes);
        }
        else
        {
            // 对于5个以上参数使用Expression
            delegateType = Expression.GetActionType(paramTypes);
        }

        // 4. 创建委托
        Delegate handler;
        if (method.IsStatic)
        {
            // 静态方法直接创建委托
            handler = Delegate.CreateDelegate(delegateType, method);
        }
        else
        {
            throw new InvalidOperationException("只允许注册静态方法");
        }
        try
        {
            MethodInfo concreteMethod = genericMethod.MakeGenericMethod(paramTypes);
            concreteMethod.Invoke(null, new object[] { eid, handler });
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException(
                $"Failed to instantiate generic method for {parameters.Length} parameters. " +
                $"Generic arguments: {string.Join(", ", paramTypes.Select(t => t.Name))}", ex);
        }
    }
    /// <summary>
    /// 注册动态事件监听器
    /// </summary>
    /// <typeparam name="T">事件ID类型</typeparam>
    /// <param name="eid">事件ID</param>
    /// <param name="method">事件对应的方法</param>
    /// <param name="forceInstanceCreation">在没有实例的时候,是否强制创建实例.</param>
    public static void RegisterDynamicEventListener<T>(T eid, MethodInfo method, bool forceInstanceCreation = false)
    {
        if(method.IsStatic)
        {
            throw new InvalidOperationException("只允许注册实例方法");
        }
        // 1. 获取委托参数类型
        ParameterInfo[] parameters = method.GetParameters();
        Type[] paramTypes = new Type[parameters.Length];
        string key = $"{eid.GetType().FullName}_{parameters.Length}";

        // 直接填充类型数组避免LINQ
        for (int i = 0; i < parameters.Length; i++)
        {
            paramTypes[i] = parameters[i].ParameterType;
        }

        // 2. 从缓存获取或创建AddListener泛型方法
        if (!_addListenerMethodCache.TryGetValue(key, out MethodInfo genericMethod))
        {
            lock (_addListenerMethodCache)
            {
                if (!_addListenerMethodCache.TryGetValue(key, out genericMethod))
                {
                    // 获取所有候选方法
                    var candidates = typeof(EventManager)
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .Where(m =>
                            m.Name == "AddListener" &&
                            m.IsGenericMethodDefinition &&
                            m.GetParameters().Length == 2 &&
                            m.GetParameters()[0].ParameterType == eid.GetType())
                        .ToList();

                    // 找到泛型参数数量匹配的方法
                    genericMethod = candidates.FirstOrDefault(m =>
                        m.GetGenericArguments().Length == parameters.Length);

                    if (genericMethod == null)
                    {
                        throw new MissingMethodException("没有发现AddListener方法");
                    }

                    _addListenerMethodCache[key] = genericMethod;
                }
            }
        }

        // 3. 优化委托创建
        Type delegateType;
        if (parameters.Length <= 4)
        {
            // 使用预定义的Action类型
            Type actionType = parameters.Length switch
            {
                0 => typeof(Action),
                1 => typeof(Action<>),
                2 => typeof(Action<,>),
                3 => typeof(Action<,,>),
                4 => typeof(Action<,,,>),
                _ => null
            };

            delegateType = actionType.MakeGenericType(paramTypes);
        }
        else
        {
            // 对于5个以上参数使用Expression
            delegateType = Expression.GetActionType(paramTypes);
        }

        // 4. 创建委托
        Delegate handler = null;
        if (method.IsStatic == false)
        {
            var instances = UnityEngine.Object.FindObjectsByType(method.DeclaringType,FindObjectsInactive.Include,FindObjectsSortMode.None);
            if (instances.Length == 0)
            {
                if(forceInstanceCreation)
                {
                    // 强制创建实例
                    handler = Delegate.CreateDelegate(delegateType, Activator.CreateInstance(method.DeclaringType), method);
                }
                else
                {
                    throw new InvalidOperationException($"没有找到实例: {method.DeclaringType.FullName}");
                }
            }
            else 
            {
                for (int i = 0; i < instances.Length; i++)
                {
                    handler = Delegate.CreateDelegate(delegateType, instances[i], method);
                }
            }
        }
        try
        {
            if (handler != null)
                return;
            MethodInfo concreteMethod = genericMethod.MakeGenericMethod(paramTypes);
            concreteMethod.Invoke(null, new object[] { eid, handler });
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException(
                $"Failed to instantiate generic method for {parameters.Length} parameters. " +
                $"Generic arguments: {string.Join(", ", paramTypes.Select(t => t.Name))}", ex);
        }
    }
    #endregion


    // 缓存字典：按特性类型索引方法列表
    public static readonly ConcurrentDictionary<Type, List<MethodInfo>> MethodCacheByAttributeType =
        new ConcurrentDictionary<Type, List<MethodInfo>>();

    // 缓存字典：按方法索引特性列表
    public static readonly ConcurrentDictionary<MethodInfo, List<Attribute>> AttributeCacheByMethod =
        new ConcurrentDictionary<MethodInfo, List<Attribute>>();

    /// <summary>
    /// 收集所有标记了指定特性类型的方法及其特性
    /// </summary>
    /// <param name="targetAttributeTypes">要扫描的目标特性类型数组</param>
    public static void CacheMethodsWithAttributes(Type[] targetAttributeTypes)
    {
        // 清空缓存
        MethodCacheByAttributeType.Clear();
        AttributeCacheByMethod.Clear();

        // 初始化特性类型对应的空方法列表
        foreach (var attributeType in targetAttributeTypes)
        {
            MethodCacheByAttributeType.TryAdd(attributeType, new List<MethodInfo>());
        }

        // 将目标特性类型转换为HashSet提高查找效率
        var targetTypesSet = new HashSet<Type>(targetAttributeTypes);

        // 扫描当前应用程序域中所有程序集
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // 跳过系统/Unity相关程序集
            string assemblyName = assembly.FullName;
            if (assemblyName.Contains("UnityEngine") ||
                assemblyName.Contains("mscorlib") ||
                assemblyName.Contains("System") ||
                assemblyName.Contains("Unity.") ||
                assemblyName.Contains("UnityEditor"))
            {
                continue;
            }

            // 扫描程序集中的所有类型
            foreach (var type in assembly.GetTypes())
            {
                // 获取类型的所有方法（包括公有/私有/静态/实例）
                foreach (MethodInfo method in type.GetMethods(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static))
                {
                    // 一次性获取方法的所有特性
                    Attribute[] allAttributes = Attribute.GetCustomAttributes(method, true);
                    bool foundTargetAttribute = false;

                    // 检查是否存在目标特性
                    foreach (Attribute attribute in allAttributes)
                    {
                        Type attributeType = attribute.GetType();

                        // 检查特性是否属于目标类型或其派生类型
                        if (targetTypesSet.Any(targetType => targetType.IsAssignableFrom(attributeType)))
                        {
                            foundTargetAttribute = true;
                            break;
                        }
                    }

                    if (!foundTargetAttribute) continue;

                    // 处理找到的目标特性
                    ProcessMethodAttributes(method, allAttributes, targetTypesSet);
                }
            }
        }
    }

    /// <summary>
    /// 处理方法特性并更新缓存
    /// </summary>
    private static void ProcessMethodAttributes(
        MethodInfo method,
        Attribute[] attributes,
        HashSet<Type> targetTypesSet)
    {
        // 确保方法在特性缓存中存在条目
        List<Attribute> attributeList = AttributeCacheByMethod.GetOrAdd(method, _ => new List<Attribute>());

        foreach (Attribute attribute in attributes)
        {
            Type attributeType = attribute.GetType();

            // 过滤非目标特性
            if (!targetTypesSet.Any(t => t.IsAssignableFrom(attributeType))) continue;

            // 添加到特性缓存（去重）
            if (!attributeList.Contains(attribute))
            {
                attributeList.Add(attribute);
            }

            // 更新方法缓存
            foreach (Type targetType in targetTypesSet)
            {
                if (targetType.IsAssignableFrom(attributeType))
                {
                    List<MethodInfo> methodList = MethodCacheByAttributeType[targetType];
                    if (!methodList.Contains(method))
                    {
                        methodList.Add(method);
                    }
                }
            }
        }
    }
}