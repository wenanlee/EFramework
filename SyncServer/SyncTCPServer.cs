using EFramework.Network;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SyncTCPServer
{
    SocketServer server;
    public void Init()
    {
        server = new SocketServer(NetworkType.Tcp, IPAddress.Any, 60100);
        server.OnStart += Server_OnStart;
        server.OnStop += Server_OnStop;
        server.OnConnected += Server_OnConnected;
        server.OnDisconnected += Server_OnDisconnected;
        server.OnReceived += Server_OnReceived;
        server.Start();
    }

    private void Server_OnReceived(object sender, SocketServerDataEventArgs e)
    {
        
    }

    private void Server_OnDisconnected(object sender, SocketServerClientEventArgs e)
    {
       
    }

    private void Server_OnConnected(object sender, SocketServerClientEventArgs e)
    {
        Debug.Log(e.Client.RemoteIP);
    }

    private void Server_OnStop(object sender, System.EventArgs e)
    {
       
    }

    private void Server_OnStart(object sender, System.EventArgs e)
    {
        Debug.Log("tcpServer");
    }
}
