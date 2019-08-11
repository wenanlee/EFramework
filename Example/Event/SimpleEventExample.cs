using EFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEventExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener<int>(SimpleTest.Test1, Handler);
        EventManager.AddListener(SimpleEvent.Test_1, Handler);
    }

    private void Handler(int obj)
    {
        Debug.Log("mode: " + obj);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            EventManager.SendEvent(SimpleTest.Test1, 1);
            EventManager.SendEvent(SimpleEvent.Test_1, 2);
        }
    }
}
