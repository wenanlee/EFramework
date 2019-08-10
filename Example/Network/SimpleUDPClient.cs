using EFramework.Network;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SimpleUDPClient
{
    SocketClient client;
    public void Init(string ip,int port)
    {
        client = new SocketClient(NetworkType.Udp);
        client.OnConnected += Client_OnConnected;
        client.OnDisconnected += Client_OnDisconnected;
        client.OnReceived += Client_OnReceived;
        client.Connect(IPAddress.Parse(ip), port);
    }

    private void Client_OnReceived(object sender, SocketClientEventArgs e)
    {
        GameEventCenter.SendEvent(SimpleNetworkEvents.Init, e.Data);
    }

    private void Client_OnDisconnected(object sender, System.EventArgs e)
    {
        
    }

    private void Client_OnConnected(object sender, System.EventArgs e)
    {
        Debug.Log("UDP:OK");
    }
    public void Send(byte[] dd)
    {
        client.Send(dd);
    }
}
