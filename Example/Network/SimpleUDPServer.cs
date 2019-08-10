using EFramework.Network;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class SimpleUDPServer
{
    SocketServer server;
    public void Init()
    {
        server = new SocketServer(NetworkType.Udp, IPAddress.Any, 60000);
        server.OnStart += Server_OnStart;
        server.OnStop += Server_OnStop;
        server.OnConnected += Server_OnConnected;
        server.OnDisconnected += Server_OnDisconnected;
        server.OnReceived += Server_OnReceived;
        server.OnError += Server_OnError;
        server.Start();
    }

    private void Server_OnError(object sender, System.IO.ErrorEventArgs e)
    {
        Debug.Log(e.ToString());
    }

    private void Server_OnReceived(object sender, SocketServerDataEventArgs e)
    {
        Debug.Log(e.Client.RemoteIP);
        e.Client.Send(Encoding.UTF8.GetBytes("127.0.0.1"));
    }

    private void Server_OnDisconnected(object sender, SocketServerClientEventArgs e)
    {
       
    }

    private void Server_OnConnected(object sender, SocketServerClientEventArgs e)
    {
        Debug.Log("连接成功");
    }

    private void Server_OnStop(object sender, System.EventArgs e)
    {
        
    }

    private void Server_OnStart(object sender, System.EventArgs e)
    {
        Debug.Log("udpServer");
    }
}
