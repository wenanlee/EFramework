using System;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkManager.Instance.Registration(MessageType.Test1, HandlerEvent);
            EventData<Package>.CreateEvent(PackageDir.send,new Package((int)MessageType.Test1,new byte[11111])).Send();
            //Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        public static void HandlerEvent(EventBase eventid)
        {
            EventData<Package> data = (EventData<Package>)eventid;
            Console.WriteLine("User: "+data.args[0].GetLength());
        }
    }
}
