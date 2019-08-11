using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEvent
{
    public static Events.Event<int> Test_1 = new Events.Event<int>(SimpleTest.Test1);
}
public enum SimpleTest
{
    Test1,Test2
}