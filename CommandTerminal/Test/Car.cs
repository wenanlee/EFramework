using CommandTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    void Start()
    {
        Terminal.Shell.AddCommand("test", Test, 0, 1, "测试非静态", "提示");
        //自动补全
        Terminal.Autocomplete.Register("test");
    }
    [RegisterCommand(Help = "开车", Hint = "car 0/1/2/3", MaxArgCount = 1, MinArgCount = 1, Name = "set.car")]
    public static void Run(CommandArg[] args)
    {
        switch (args[0].Int)
        {
            case 0:
                Debug.Log("前");
                break;
            case 1:
                Debug.Log("后");
                break;
            case 2:
                Debug.Log("左");
                break;
            case 3:
                Debug.Log("右");
                break;
            default:
                break;
        }
    }
    public void Test(CommandArg[] args)
    {
        Terminal.Log(name);
    }
}
