using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    public bool Enable=true;
    private void Start()
    {
        Debuger.EnableLog = Enable;
        Debuger.Log("Log");
        Debuger.LogError("LogError");
        Debuger.LogWarning("LogWarning");
        Debug.Log("Test");
    }
}
