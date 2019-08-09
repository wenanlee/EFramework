using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEventExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEventCenter.AddListener<int>(SimpleTest.Test1, Handler);
        GameEventCenter.AddListener(SimpleEvent.Test_1, Handler);
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
            GameEventCenter.SendEvent(SimpleTest.Test1, 1);
            GameEventCenter.SendEvent(SimpleEvent.Test_1, 2);
        }
    }
}
