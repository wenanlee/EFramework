using System;
using EFramework.Core;
using EFramework.Utility;

namespace EFrameworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestEvent();
            Student wenan = new Student();
            wenan.id = 1001;
            wenan.name = "Wenan";
            Console.WriteLine();
            //Debuger.Log("");
        }

        public static void TestEvent()
        {
            EventManager.AddListener("test", Handler);
            EventManager.SendEvent("test");
        }

        private static void Handler()
        {
            Debuger.Log("Test");

        }


        public void TestDebug()
        {
            Debuger.Log("log");
            Debuger.LogError("error");
            Debuger.LogWarning("warning");
        }
    }
    public class Student
    {
        public int id;
        public string name;
    }
}
