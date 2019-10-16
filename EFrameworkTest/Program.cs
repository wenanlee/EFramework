using System;
using EFramework.Core;

namespace EFrameworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestEvent();
            
        }

        public static void TestEvent()
        {
            EventManager.AddListener("test", Handler);
            EventManager.SendEvent("test");
        }

        private static void Handler()
        {
            Debugger.Log("Test");
        }


        public void TestDebug()
        {
            Debugger.Log("log");
            Debugger.LogError("error");
            Debugger.LogWarning("warning");
        }
    }
}
