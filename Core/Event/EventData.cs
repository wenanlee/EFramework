using System.Collections;
using System;
using UnityEngine;
using PENet;

//[Serializable]
//public class EventMsg:EventBase<byte[]>
//{
//    public EventMsg(Package package)
//    {
//        this.type = package.type;
//        this.package = package;
//    }
//    public static EventMsg CreateEvent(Package package)
//    {
//        EventMsg eventBase = new EventMsg(package);
//        return eventBase;
//    }
//    public override void SendToAll()
//    {
//        NetworkManager.Instance.server.SendToAll(package);
//    }
//    public override void SendToServer()
//    {
//        NetworkManager.Instance.server.SendToAll(package);
//    }
//    public override void SendToClient(ServerMsg client)
//    {
//        NetworkManager.Instance.server.Send(client, package);
//    }
//    public override void SendToRoom()
//    {
        
//    }
//}
//[Serializable]
//public class EventData:EventBase<object>
//{
//    public object args = null;
//    public EventData(Enum eid, params object[] args)
//    {
//        this.type = eid;
//        this.args = args;
//    }
//    public static EventData CreateEvent(Enum e, params object[] args)
//    {
//        EventData eventBase = new EventData(e, args);
//        return eventBase;
//    }
//}
[Serializable]
public class EventBase
{
    public Enum type;
    //public T arg;

    //protected void EventData(Enum type,T arg)
    //{
    //    this.type = type;
    //    this.arg = arg;
    //}

    public virtual void SendToHandler()
    {
        //if (EventManager.Instance != null) EventManager.Instance.SendEvent(this);
    }

    public virtual void SendToAll()
    {
        NetworkManager.Instance.MessageServer.SendToAll(new Package { type = type,data=new byte[0] });
    }
    /// <summary>
    /// 发送给一个客户端
    /// </summary>
    public virtual void SendToClient(ServerMsg client)
    {
        NetworkManager.Instance.MessageServer.Send(client, new Package { type = type, data = new byte[0] });
    }
    /// <summary>
    /// 发送给服务器
    /// </summary>
    public virtual void SendToServer()
    {
        NetworkManager.Instance.client.Send(new Package { type = type,data=new byte[0] });
    }
    public virtual void SendToRoom()
    {

    }
}