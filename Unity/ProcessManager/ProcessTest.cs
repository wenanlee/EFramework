using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ProcessManager.NextProcess();
        }
    }
}
