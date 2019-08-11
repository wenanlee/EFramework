using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace EFramework.Network
{
    public class NetworkTools
    {
        public static byte[] Serialize<T>(T pkg) where T : MessageBase
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, pkg);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }

        public static T DeSerialize<T>(byte[] bs) where T : MessageBase
        {
            using (MemoryStream ms = new MemoryStream(bs))
            {
                BinaryFormatter bf = new BinaryFormatter();
                T pkg = (T)bf.Deserialize(ms);
                return pkg;
            }
        }

        internal static string GetLocalIP()
        {
            //TODO
            return "127.0.0.1";
        }
    }
}