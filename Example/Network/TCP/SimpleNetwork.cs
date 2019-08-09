using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNetworkEvent
{
    public static GameEvents.Event<string, string> Login = new GameEvents.Event<string, string>(SimpleNetworkTest.login);
}
public enum SimpleNetworkTest
{
    login,Test2
}