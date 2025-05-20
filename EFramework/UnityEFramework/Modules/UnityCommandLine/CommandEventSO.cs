using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFramework.Unity.Command
{
    [CreateAssetMenu(fileName = "CommandEventSO", menuName = "EFramework/CommandEventSO")]
    public class CommandEventSO : SOBase
    {
        [TableList]
        public List<commandEventArgs> commandEvents = new List<commandEventArgs>();
        public override void ReLoadSO()
        {
            GetAllMethods();
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
                            commandEvents.Add(new commandEventArgs()
                            {
                                commandName = commandName, // 使用特性中的Name而不是方法名
                                uuid = UUID.New(),
                                commandArgsType = method.GetParameters().Select(p => p.ParameterType).ToArray()
                            });
                        }
                    }
            }
        }
    }
    [Serializable]
    public class commandEventArgs
    {
        public string uuid;
        public string commandName;
        [ShowInInspector]
        public Type[] commandArgsType;

        public commandEventArgs()
        {
            uuid = UUID.New();
        }
    }
}
