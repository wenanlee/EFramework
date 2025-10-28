using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework
{
    public static class UUIDExtensions
    {
        public static string GetUUID(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                Debug.LogError("获取UUID失败，字符串为空");
                return string.Empty;
            }
            if (str.Length < 10)
            {
                Debug.LogError($"{str} 获取UUID失败，字符串长度小于10");
                return string.Empty;
            }
            if (str.Contains("UUID") == false)
            {
                Debug.LogError($"{str} 中找不到UUID");
                return string.Empty;
            }
            return str.Substring(str.IndexOf("UUID") + 4, 6);
        }
        public static string GetUUID(this GameObject go)
        {
            if(go.name.Contains("UUID") == false)
            {
                Debug.LogError($"{go.name} 中找不到UUID");
                return string.Empty;
            }
            return go.name.GetUUID();
        }
    }
}
