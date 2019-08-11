using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncServer : MonoBehaviour
{
    SyncTCPServer TCPServer = new SyncTCPServer();
    SyncUDPServer UDPServer = new SyncUDPServer();
    void Start()
    {
        TCPServer.Init();
        UDPServer.Init();
    }

    void Update()
    {
        
    }
}
