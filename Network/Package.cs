using PENet;
using System;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
[Serializable]
public class Package : PEMsg
{
    public Enum type;
    public byte[] data;                                  //真正的消息
    public Package()
    {
        type = null;
        data = null;
    }
    /// <summary>
    /// 初始化包
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="port">端口号</param>
    /// <param name="type">消息类型</param>
    /// <param name="msg">消息体</param>
    public Package(Enum type, byte[] msg)
    {
        Format(type, msg);
    }
    /// <summary>
    /// 初始化包
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="port">端口号</param>
    /// <param name="type">消息类型</param>
    /// <param name="msg">消息体</param>
    public Package(Enum type, string msg)
    {
        Format(type, Encoding.UTF8.GetBytes(msg));
    }
    /// <summary>
    /// 使用多个string初始化包
    /// </summary>
    /// <param name="type"></param>
    /// <param name="msgs"></param>
    public Package(Enum type, params string[] msgs)
    {
        Format(type, Encoding.UTF8.GetBytes(string.Join("|", msgs)));
    }
    /// <summary>
    /// 初始化包
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="port">端口号</param>
    /// <param name="type">消息类型</param>
    /// <param name="msg">消息体</param>
    public Package(Enum type, int msg)
    {
        Format(type, BitConverter.GetBytes(msg));
    }
    /// <summary>
    /// 格式化消息
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="port">端口号</param>
    /// <param name="type">消息类型</param>
    /// <param name="msg">消息体</param>
    private void Format(Enum type, byte[] msg)
    {
        this.type = type;
        this.data = msg;
    }
    public string GetString()
    {
        if (data == null)
            return "";
        return Encoding.UTF8.GetString(data).TrimEnd('\0');
    }
    public string[] GetStrings()
    {
        if (data == null)
            return new string[0];
        return Encoding.UTF8.GetString(data).TrimEnd('\0').Split('|');
    }
    public string GetStrings(int index)
    {
        string tmp = GetStrings()[index];
        return tmp;
    }
    public int GetInt()
    {
        if (data == null)
            return 0;
        return BitConverter.ToInt32(data, 0);
    }
}