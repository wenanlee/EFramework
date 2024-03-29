﻿using EFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EFramework.Core.Events;
using EFramework.Tweening;

public class EventsDemo : MonoBehaviour
{
    public Signal<string, string> Login = new Signal<string, string>();
    private void Start()
    {
        Login += Loginn;
        Login.InvokeSafe("wenan", "123321123");
        EventManager.AddListener<string, string>(1, Loginn);
        EventManager.SendEvent(1,"wenan1","456654456");
    }
    private void Loginn(string name, string password)
    {
        Debug.Log(name + "   " + password);
        TweeningRotate.Begin(gameObject, Vector3.zero, Vector3.up * 360, 10);
    }
}
