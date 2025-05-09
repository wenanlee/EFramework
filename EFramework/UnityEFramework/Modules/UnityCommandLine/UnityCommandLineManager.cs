using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
namespace EFramework.UnityCommandLine
{
    public class UnityCommandLineManager : MonoSingleton<UnityCommandLineManager>
    {
        public IEnumerable<CommandInstance> commandInstances;
        public Dictionary<string, CommandInstance> commandDict = new Dictionary<string, CommandInstance>();
        public List<string> commandLineLst = new List<string>();
        private static CommandTerminal.CommandAutocomplete Complete = new CommandTerminal.CommandAutocomplete();
        private void Awake()
        {
            //commandInstances = GetAllMethods(m => m.GetCustomAttributes(typeof(RegisterCommandLine), true).Length > 0);
            Debug.Log("Command count:" + commandInstances.ToArray().Length);
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="commandLine">命令</param>
        public void CommandParser(string commandLine)
        {
            Debug.Log("command: " + commandLine);
            var args = commandLine.Split(' ');
            if (commandDict.ContainsKey(args[0]) == false)
            {
                Debug.LogError("Command Not Found");
                return;
            }
            commandDict[args[0]].Invoke(args.Skip(1).ToArray());
        }


        /// <summary>
        /// 获取所有带CommandInstance的方法
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [Button("获取所有命令")]
        public void GetAllMethods()
        {
            commandLineLst.Clear();
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
                        // 查找带有特定特性的方法，例如ButtonAttribute
                        if (method.GetCustomAttributes(typeof(RegisterCommandLine), true).Length > 0)
                        {
                            Debug.Log($"找到带RegisterCommandLine特性的方法: {type.FullName}.{method.Name}");
                            commandLineLst.Add($"{type.FullName}.{method.Name}");
                        }
                    }
            }
        }
        [SerializeField]
        public class CommandInstance
        {
            public string Name { get; set; }
            public MethodInfo MethodInfo { get; set; }
            public object[] InstanceLst { get; set; }
            public RegisterCommandLine _Attribute { get; set; }
            public CommandInstance(MethodInfo method)
            {
                MethodInfo = method;
                _Attribute = method.GetCustomAttribute<RegisterCommandLine>();
                string hint = "None";
                foreach (var item in method.GetParameters())
                {
                    if (hint == "None")
                        hint = $"{item.ParameterType.Name} : {item.Name}  ";
                    else
                        hint += $"{item.ParameterType.Name} : {item.Name}  ";
                }
                _Attribute.Hint = hint;
                Name = string.IsNullOrEmpty(_Attribute.Name) ? MethodInfo.Name.ToLower() : _Attribute.Name.ToLower();
                if (!MethodInfo.IsStatic)
                    InstanceLst = UnityEngine.Object.FindObjectsOfType(method.ReflectedType);
                Instance.commandDict.Add(Name, this);
                Instance.commandLineLst.Add(Name);
            }
            public void Invoke(params string[] args)
            {
                if (MethodInfo.IsStatic)
                {
                    MethodInfo.Invoke(null, StringArrayToObjectArray(args));
                    return;
                }

                foreach (var item in InstanceLst)
                {
                    MethodInfo.Invoke(item, StringArrayToObjectArray(args));
                }
            }
            public object[] StringArrayToObjectArray(string[] args)
            {
                var objs = new object[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    objs[i] = Convert.ChangeType(args[i], MethodInfo.GetParameters()[i].ParameterType);
                }
                return objs;
            }
        }
    }
}