using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessManager : MonoBehaviour
{
    public Process currentProcess;
    public int currentProcessindex = -1;
    public int CurrentProcessindex { get => currentProcessindex; set { currentProcessindex = value; currentProcess = processArray[value]; } }
    public Process[] processArray;
    public static void NextProcess()
    {

        if (instance.currentProcessindex >= 0)
            instance.processArray[instance.currentProcessindex].process.Exit();
        instance.currentProcessindex += 1;
        instance.processArray[instance.currentProcessindex].process.Run();
    }
    public static void SetProcess(string processName)
    {
        for (int i = 0; i < instance.processArray.Length; i++)
        {
            if (instance.processArray[i].Name==processName)
            {
                if (instance.currentProcessindex >= 0)
                    instance.processArray[instance.currentProcessindex].process.Exit();
                instance.currentProcessindex = i;
                instance.processArray[instance.currentProcessindex].process.Run();
                return;
            }
        }
        
    }

    #region 单例
    private static ProcessManager instance;



    private void Awake()
    {
        instance = this;
    }
    #endregion
}
[System.Serializable]
public class Process
{
    public string Name;
    public ProcessInfoBase process;
}