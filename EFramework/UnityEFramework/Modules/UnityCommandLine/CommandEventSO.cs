using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CommandTerminal;
using EFramework.Core;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFramework.Unity.Command
{
    [CreateAssetMenu(fileName = "CommandEventSO", menuName = "EFramework/CommandEventSO")]
    public class CommandEventSO : SOBase
    {
        [TableList]
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
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.Contains("UnityEngine") ||
                    assembly.FullName.Contains("mscorlib") ||
                    assembly.FullName.Contains("System") ||
                    assembly.FullName.Contains("Unity.") ||
                    assembly.FullName.Contains("UnityEditor"))
                    continue;
                foreach (var type in assembly.GetTypes())
                    foreach (MethodInfo method in type.GetMethods(
                               BindingFlags.Public |
                               BindingFlags.NonPublic |
                               BindingFlags.Instance |
                               BindingFlags.Static))
                    {
                        // 获取所有RegisterCommandLine特性
                        var attributes = method.GetCustomAttributes(typeof(RegisterCommandLine), true);
                        if (attributes.Length > 0)
                        {
                            // 假设第一个特性是我们需要的
                            RegisterCommandLine attribute = (RegisterCommandLine)attributes[0];
                            string commandName = string.IsNullOrEmpty(attribute.Name) ? type.FullName + "." + method.Name : attribute.Name;
                            commandEvents.Add(new CommandEventArgs(commandName, method));
                        }
                    }
            }
        }
    }
    [Serializable]

    /// <summary>
    /// 表示包含方法调用信息的命令事件参数
    /// </summary>
    public class CommandEventArgs
    {
        /// <summary>
        /// 获取命令的唯一标识符
        /// </summary>
        [ShowInInspector] public string Uuid;

        /// <summary>
        /// 获取命令名称
        /// </summary>
        [ShowInInspector] public string CommandName;

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
            Uuid = Guid.NewGuid().ToString();
            CommandName = commandName;
            CommandMethodStr = method.Name;
            CommandTypeStr = method.DeclaringType.AssemblyQualifiedName;
            CommandArgsTypeStrs = method.GetParameters()
                                        .Select(p => p.ParameterType.AssemblyQualifiedName)
                                        .ToArray();
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
