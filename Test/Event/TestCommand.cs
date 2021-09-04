using EFramework.Core;
using UnityEngine;

public class TestCommand : MonoBehaviour
{
    public static Signal<int> signal = new Signal<int>();
    void Start()
    {
        signal.AddListener(Test0);
        signal += Test1;
        EventManager.AddListener<int>("Test2", Test2);

        signal.Invoke(1);
        EventManager.SendEvent("Test2", 2);
        EventManager.SendEvent("TestCommand.Test3", 3);
        EventManager.SendEvent("Test4", 4);
    }
    public void Test0(int a)
    {
        Debug.Log("Test0" + a);
    }
    public void Test1(int a)
    {
        Debug.Log("Test1" + a);
    }
    public void Test2(int a)
    {
        Debug.Log("Test2" + a);
    }
    //[RegisterCommand]
    public void Test3(int a)
    {
        Debug.Log("Test3" + a);
    }
    //[RegisterCommand(command: "Test4")]
    public void Test4(int a)
    {
        Debug.Log("Test4" + a);
    }
}
