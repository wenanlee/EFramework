using System;

namespace EFramework.Utility
{
#if UNITY_EDITOR
    public class Debuger
    {
        public static void Log(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
        public static void LogError(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }
        public static void LogException(Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }
        public static void LogWarning(string msg)
        {
            UnityEngine.Debug.LogWarning(msg);
        }
    }
#endif
    public class Debuger
    {
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
        public static void LogError(string msg)
        {
            Console.WriteLine(msg);
        }
        public static void LogException(Exception e)
        {
            Console.WriteLine(e);
        }
        public static void LogWarning(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}