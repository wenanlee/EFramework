using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNetworkExample : MonoBehaviour
{
    SimpleServer server;
    SimpleClient client;
    void Start()
    {
        server = new SimpleServer();
        client = new SimpleClient();
        server.Init();
        GameEventCenter.AddListener<string,string>(SimpleNetworkTest.login, Receive);
        GameEventCenter.AddListener(SimpleNetworkEvent.Login,Send);
    }

    private void Send(string username, string password)
    {
        client.Send(new SimplePackage(SimpleNetworkTest.login, username, password));
    }

    private void Receive(string username,string password)
    {
        //触发在SimpleServerSession.OnReciveMsg处
        Debug.Log("接收到:username: " + username + " password: " + password);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            client.Init();
            GameEventCenter.SendEvent(SimpleNetworkEvent.Login,"Wenan","password");
        }
    }
}
