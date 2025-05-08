using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulesBase : MonoBehaviour
{
    /// <summary>
    /// 初始化模块
    /// </summary>
    public virtual void Init() { }
    /// <summary>
    /// 释放模块
    /// </summary>
    public virtual void Release() { }
    private void OnApplicationQuit()
    {
        Release();
    }
}
