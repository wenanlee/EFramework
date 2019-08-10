using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNetworkEvents : MonoBehaviour
{
    public static GameEvents.Event<byte[]> Init = new GameEvents.Event<byte[]>(SimpleTest.Test1);
}
public enum SimpleNetworkType
{
    Test1, Test2
}