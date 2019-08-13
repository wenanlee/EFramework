using System.Collections.Generic;
using System.Diagnostics;

namespace EFramework.Core
{
    public enum LogType
    {
        Error,
        Assert,
        Warning,
        Log,
        Exception
    }
    public struct Log
    {
        public int count;
        public string message;
        public string stackTrace;
        public LogType type;

        private const int MaxMessageLength = 16382;
        public Log(int count, string message, string stackTrace, LogType type)
        {
            this.count = count;
            this.message = message;
            this.stackTrace = stackTrace;
            this.type = type;
        }
        public bool Equals(Log log)
        {
            return message == log.message && stackTrace == log.stackTrace && type == log.type;
        }

        public string GetTruncatedMessage()
        {
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }

            return message.Length <= MaxMessageLength ? message : message.Substring(0, MaxMessageLength);
        }
    }

    public class Debugger
    {
        public static List<Log> LogList = new List<Log>();
        public static bool Enabled = true;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID
    public static void Log(params string[] msg)
    {
        UnityEngine.Debug.Log("<color=white> >>> " + System.String.Join("", msg)+ "</color>");
    }
    public static void LogError(params string[] msg)
    {
        UnityEngine.Debug.LogError("<color=red> >>> " + System.String.Join("", msg) + "</color>");
    }
    public static void LogWarning(params string[] msg)
    {
        UnityEngine.Debug.LogWarning("<color=yellow> >>> " + System.String.Join("", msg) + "</color>");
    }

#else
        public static void Log(params string[] msg)
        {
            Print(System.ConsoleColor.White, LogType.Log, System.String.Join("", msg));
        }
        public static void LogError(params string[] msg)
        {
            Print(System.ConsoleColor.Red, LogType.Error, System.String.Join("", msg));
        }
        public static void LogWarning(params string[] msg)
        {
            Print(System.ConsoleColor.Yellow, LogType.Warning, System.String.Join("", msg));
        }
        private static void Print(System.ConsoleColor color, LogType type, string logs)
        {
            if (Enabled)
            {
                //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                //System.Diagnostics.StackFrame[] sfs = st.GetFrames();
                //string[] methodName = new string[sfs.Length];
                //for (int i = 0; i < sfs.Length; i++)
                //{
                //    methodName[i] = sfs[i].GetFileName() + sfs[i].GetType().Name + sfs[i].GetFileLineNumber() + sfs[i].GetMethod().Name;
                //}
                //LogList.Add(new Log(1, logs, string.Join("\n", methodName), type));
                System.Console.ForegroundColor = color;
                System.Console.WriteLine(System.DateTime.Now.TimeOfDay + " >>> " + logs);
                System.Console.ForegroundColor = System.ConsoleColor.White;
            }
        }
#endif
    }
}
