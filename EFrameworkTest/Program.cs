using System;
using EFramework.Core;
using EFramework.Utility;
using EFramework.Network;
using System.Text;
using System.Linq;
using System.IO;

namespace EFrameworkTest
{
    class Program
    {
        static byte[] buffer;
        static void Main(string[] args)
        {
            //半包测试
            //
            //string msg = "001200141234";
            //粘包测试
            string[] msg = new string[] {
                "001200141234",
                "56001200141234",
                "560012001412345600120014",
                "12345600120014123456"
            };
            foreach (var item in msg)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(item);
                Handler(bytes);
            }
        }
        static PackagePlus package;
        public static void Handler(byte[] bytes)
        {
            ReceiveData(bytes,0,bytes.Length);
        }
        static MemoryStream memoryStream = new MemoryStream();
        /// <summary>
        /// 解决粘包
        /// </summary>
        public static void ReceiveData(byte[] bytes, int offset, int size)
        {
            memoryStream.Write(bytes, offset, size);
            byte[] getData = memoryStream.ToArray();
            int StartIndex = 0;
            while (true)
            {
                if (bytes.Length > 0)
                {
                    int HeadLength = getData.Length - StartIndex < 8 ? -1 : int.Parse(Encoding.UTF8.GetString(getData, StartIndex + 4, 4));
                    if (getData.Length - StartIndex < HeadLength || HeadLength == -1)
                    {
                        memoryStream.Close();
                        memoryStream.Dispose();
                        memoryStream = new MemoryStream();
                        memoryStream.Write(getData, StartIndex, getData.Length - StartIndex);//从新将接受的消息写入内存流
                        break;
                    }
                    else
                    {
                        Console.WriteLine(Encoding.UTF8.GetString(getData, StartIndex, HeadLength)) ;
                        StartIndex += HeadLength;
                    }
                }
            }
        }
    }
}
