using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SimpleClientExample : MonoBehaviour
{
    SimpleTCPClient tCPClient = new SimpleTCPClient();
    SimpleUDPClient uDPClient = new SimpleUDPClient();
    void Start()
    {

        GameEventCenter.AddListener(SimpleNetworkEvents.Init, handler);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            uDPClient.Init("127.0.0.1", 60000);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            uDPClient.Send(new byte[10]);
        }
    }

    private void handler(byte[] bytes)
    {
        Debug.Log("消息:" + Encoding.Default.GetString(bytes));
        tCPClient.Init(Encoding.Default.GetString(bytes), 60001);
    }
}
