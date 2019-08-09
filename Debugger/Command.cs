using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Debugger
{
    public class Command
    {
        public static void RunCommand(string commandStr)
        {
            string[] str = commandStr.Split(' ');
            switch (str[0])
            {
                case "showconsole":
                    //GetComponent<Console>().IsShowUI = bool.Parse(str[1]);
                    break;
                case "showwarning":
                    //GetComponent<Console>().IsShowWarning = bool.Parse(str[1]);
                    break;
                case "print":
                    Debug.Log(str[1]);
                    break;
                default:
                    Debug.Log("命令无法识别");
                    break;
            }
        }
    }
}

