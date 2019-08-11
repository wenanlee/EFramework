using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNetworkEvents : MonoBehaviour
{
    public static Events.Event<byte[]> Init = new Events.Event<byte[]>(SimpleTest.Test1);
}
public enum SimpleNetworkType
{
    Test1, Test2
}