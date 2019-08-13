using System;
using EFramework.Core;

namespace EFrameworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestEvent();
            //NetEvent.Instance.Dispatch(1001, new Events());
        }

        public static void TestEvent()
        {
            //NetEvent.Instance.AddListener(1001, handler);
        }

        private static void handler(Events p)
        {
            throw new NotImplementedException();
        }

        private static void handler(string p)
        {
            Debugger.Log(p);
        }

        public void TestDebug()
        {
            Debugger.Log("log");
            Debugger.LogError("error");
            Debugger.LogWarning("warning");
        }
    }
}
