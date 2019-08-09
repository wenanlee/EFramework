using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Network;
using EFramework;

public class SimpleServer
{
    TCPSocket<SimpleServerSession, SimplePackage> sks = null;
    public void Init()
    {
        sks = new TCPSocket<SimpleServerSession, SimplePackage>();
        sks.StartAsServer(IPCfg.srvIP, IPCfg.srvPort);
        sks.GetSesstionLst();

    }
    public void SendToAll(SimplePackage msg)
    {
        foreach (var item in sks.GetSesstionLst())
        {
            Send(item,msg);
        }
    }
    public void Send(SimpleServerSession client, SimplePackage msg)
    {
        client.SendMsg(msg);
    }
}

public class SimpleServerSession:PESession<SimplePackage>
{
    protected override void OnConnected()
    {
        Debug.Log("Server: 客户端连接成功");
    }
    protected override void OnReciveMsg(SimplePackage msg)
    {
        GameEventCenter.SendEvent(msg.type, msg.GetString()[0],msg.GetString()[1]);
    }
    protected override void OnDisConnected()
    {
        Debug.Log("断开连接");
    }
}