using EFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public bool enabledLog = false;
    private void Start()
    {
        RegisterCommands();
        //Debug.Log("<<<< "+Assembly.Load("Assembly-CSharp").GetName()+"  "+Assembly.GetCallingAssembly().GetName());
    }
    public void RegisterCommands()
    {
        var method_flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        foreach (var type in Assembly.Load("Assembly-CSharp").GetTypes())
        {
            //Debug.Log(type.Name);
            //Debug.LogError((GameObject.FindObjectsOfType(type)[0].name));
            foreach (var method in type.GetMethods(method_flags))
            {
                var attribute = Attribute.GetCustomAttribute(method, typeof(RegisterCommandAttribute), false) as RegisterCommandAttribute;
                if (attribute == null)
                {
                    continue;
                    //if (method.Name.StartsWith("FRONTCOMMAND", StringComparison.CurrentCultureIgnoreCase))
                    //{
                    //    attribute = new RegisterCommandAttribute();
                    //}
                    //else
                    //{
                    //    continue;
                    //}
                }
                if (string.IsNullOrEmpty(attribute.Command))
                    attribute.Command = type.Name + "." + method.Name;
                foreach (UnityEngine.Object go in FindObjectsOfType(type))
                {
                    //if (enabledLog)
                        Debug.Log((">>> Ìí¼Ó¶©ÔÄ: " + attribute.Command).ToColor(Color.red)+"  From: "+go.name);
                    AddCommand(attribute.Command, attribute.Description, go, method, attribute.ParameterNames);
                }
                //if(attribute.Description==string.Empty)
                //    attribute.Description = method.


            }
        }
    }

    private static void AddCommand(string command, string description, UnityEngine.Object targetComponent, MethodInfo method, string[] parameterNames)
    {
        if (string.IsNullOrEmpty(command))
        {
            Debug.LogError("Command name can't be empty!");
            return;
        }

        command = command.Trim();
        if (command.IndexOf(' ') >= 0)
        {
            Debug.LogError("Command name can't contain whitespace: " + command);
            return;
        }

        ParameterInfo[] parameters = method.GetParameters();
        if (parameters == null)
            parameters = new ParameterInfo[0];
        Type[] parameterTypes = new Type[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType.IsByRef)
            {
                Debug.LogError("Command can't have 'out' or 'ref' parameters");
                return;
            }
            parameterTypes[i] = parameters[i].ParameterType;
        }
        //Debug.Log(command + "    " + parameterTypes.Length + "    " + parameterTypes[0].Name);
        EventAgentDelegate<string>.Instance.AddListener(command, GetActionDelegate(method, targetComponent, parameterTypes));
    }
    private static Delegate GetActionDelegate(MethodInfo methodInfo, object targetComponent, Type[] parameterTypes)
    {
        Type genericType;

        switch (parameterTypes.Length)
        {
            case 0:
                return new Action(() => methodInfo.Invoke(targetComponent, null));

            case 1:
                genericType = typeof(Action<>);
                break;

            case 2:
                genericType = typeof(Action<,>);
                break;

            case 3:
                genericType = typeof(Action<,,>);
                break;

            case 4:
                genericType = typeof(Action<,,,>);
                break;

            case 5:
                genericType = typeof(Action<,,,,>);
                break;

            default:
                return null;
        }

        var makeGeneric = genericType.MakeGenericType(parameterTypes);
        return Delegate.CreateDelegate(makeGeneric, targetComponent, methodInfo);
    }
}
