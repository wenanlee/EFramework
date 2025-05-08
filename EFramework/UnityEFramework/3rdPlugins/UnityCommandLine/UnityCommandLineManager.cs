using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace EFramework.UnityCommandLine
{
    public class UnityCommandLineManager : MonoSingleton<UnityCommandLineManager>
    {
        public IEnumerable<CommandInstance> commandInstances;
        public Dictionary<string, CommandInstance> commandDict = new Dictionary<string, CommandInstance>();
        public List<string> commandLineLst = new List<string>();
        private static CommandTerminal.CommandAutocomplete Complete=new CommandTerminal.CommandAutocomplete();
        private void Awake()
        {
            commandInstances = GetAllMethods(m => m.GetCustomAttributes(typeof(RegisterCommandLine), true).Length > 0);
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
        public IEnumerable<CommandInstance> GetAllMethods(Func<MethodInfo, bool> predicate)
        {
            List<Type> types = Assembly.GetExecutingAssembly().GetTypes().ToList();

            for (int i = types.Count - 1; i >= 0; i--)
            {
                IEnumerable<MethodInfo> methodInfos = types[i]
                    .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(predicate);

                foreach (var methodInfo in methodInfos)
                {
                    yield return new CommandInstance(methodInfo);
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