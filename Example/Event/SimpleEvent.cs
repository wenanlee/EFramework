using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEvent
{
    public static GameEvents.Event<int> Test_1 = new GameEvents.Event<int>(SimpleTest.Test1);
}
public enum SimpleTest
{
    Test1,Test2
}