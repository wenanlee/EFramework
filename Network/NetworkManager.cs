using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    public Server MessageServer;
    public Server ImageServer;
    public Client client;
    protected override void InitSingleton()
    {
        MessageServer = new Server();
        client = new Client();
        MessageServer.Init();
        client.Init();
        GameEventCenter.AddListener(Events.Login, Login);
    }

    public void SendToHandler()
    {
       
    }

    public void SendToAll(Package package)
    {
        MessageServer.SendToAll(package);
    }
    /// <summary>
    /// 发送给一个客户端
    /// </summary>
    public void SendToClient(ServerMsg client, Package package)
    {
        //NetworkManager.Instance.MessageServer.Send(client, new Package { type = type, data = new byte[0] });
    }
    /// <summary>
    /// 发送给服务器
    /// </summary>
    public void SendToServer(Package package)
    {
        client.Send(package);
        //NetworkManager.Instance.client.Send(new Package { type = type, data = new byte[0] });
    }
    public void SendToRoom(int id,Package package)
    {

    }
    private void Login(string username,string password)
    {
        Debug.Log(username+password);
        SendToServer(new Package(MessageType.login,username,password));
    }
}
