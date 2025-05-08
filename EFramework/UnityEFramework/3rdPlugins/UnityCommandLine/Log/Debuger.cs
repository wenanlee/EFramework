using EFramework.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuger
{
    public static void Log(string msg, string stackTrace = "")
    {
        Console.Instance.HandleLogThreaded(msg, stackTrace, LogType.Log);
    }
}
