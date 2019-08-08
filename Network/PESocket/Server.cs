using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PENet;

public class Server
{
    PESocket<ServerMsg, Package> sks = null;
    public void Init()
    {
        sks = new PENet.PESocket<ServerMsg, Package>();
        sks.StartAsServer(IPCfg.srvIP, IPCfg.srvPort);
        sks.GetSesstionLst();

    }
    public void SendToAll(Package msg)
    {
        foreach (var item in sks.GetSesstionLst())
        {
            Send(item,msg);
        }
    }
    public void Send(ServerMsg client, Package msg)
    {
        client.SendMsg(msg);
    }
}

public class ServerMsg:PESession<Package>
{
    protected override void OnConnected()
    {
        Debug.Log("连接成功");
    }
    protected override void OnReciveMsg(Package msg)
    {
        //MessageEvent.CreateEvent(msg).SendToHandler();
        Debug.Log(msg.GetString());
    }
    protected override void OnDisConnected()
    {
        Debug.Log("断开连接");
    }
}