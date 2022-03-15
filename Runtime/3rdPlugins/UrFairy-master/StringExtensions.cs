using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{
    public static string ToColor(this string str,Color color, bool bold = false, bool italic = false)
    {
        var content = string.Format("<color={0}>{1}</color>", color.ToString(), str);
        content = bold ? string.Format("<b>{0}</b>", content) : content;
        content = italic ? string.Format("<i>{0}</i>", content) : content;

        return content;
    }
    /// <summary>
    /// 获取后缀
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="lenght">长度</param>
    /// <returns></returns>
    public static string Suffix(this string str,int lenght)
    {
        return str.Substring(str.Length - lenght - 1);
    }
    /// <summary>
    /// 获取后缀(Int)
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="lenght">长度</param>
    /// <returns></returns>
    public static int SuffixToInt(this string str, int lenght)
    {
        return int.Parse(str.Suffix(lenght));
    }

    /// <summary>
    /// 获取前缀
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="lenght">长度</param>
    /// <returns></returns>
    public static string Prefix(this string str, int lenght)
    {
        return str.Substring(0,lenght);
    }
    /// <summary>
    /// 获取前缀(int)
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="lenght">长度</param>
    /// <returns></returns>
    public static int PrefixToInt(this string str, int lenght)
    {
        return int.Parse(str.Prefix(lenght));
    }
}
