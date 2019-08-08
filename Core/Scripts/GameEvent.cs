using UnityEngine;
using System.Collections;
using System;

//public static class GameEvent
//{
//    public static GameEvents.Event TestEvent = new GameEvents.Event("TestEvent");
//}
//public static class TestEvent
//{
//    public static GameEvents.Event<Vector3, GameObject> test = new GameEvents.Event<Vector3, GameObject>("test");
//}
public class Events
{
    public static GameEvents.Event<string, string> Login = new GameEvents.Event<string, string>(MessageType.login); 
}
public interface IHandler
{
    void Handler(Enum eid,byte[] bytes);
}