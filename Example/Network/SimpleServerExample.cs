using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleServerExample : MonoBehaviour
{
    // Start is called before the first frame update
    SimpleTCPServer tCPServer = new SimpleTCPServer();
    SimpleUDPServer uDPServer = new SimpleUDPServer();
    void Start()
    {
        tCPServer.Init();
        uDPServer.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
