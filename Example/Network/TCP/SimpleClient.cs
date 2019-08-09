using EFramework;
using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Network;

public class SimpleClient
{
    EFramework.Network.TCPSocket<SimpleClientSession, SimplePackage> skt = null;

    public void Init()
    {
        skt = new TCPSocket<SimpleClientSession, SimplePackage>();
        skt.StartAsClient(IPCfg.srvIP, IPCfg.srvPort);

        skt.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
    }
    public void Send(SimplePackage package)
    {
        skt.session.SendMsg(package); 
    }
}
public class SimpleClientSession : PESession<SimplePackage>
{
    protected override void OnConnected()
    {
        Debug.Log("Client: 连接到服务器成功");
    }

    protected override void OnReciveMsg(SimplePackage msg)
    {
        //PETool.LogMsg("Server Response:" + msg.GetString());
        //EventData<Package>.CreateEvent(msg.type,msg).SendToHandler();
    }

    protected override void OnDisConnected()
    {
        Debug.Log("断开连接");
    }
}