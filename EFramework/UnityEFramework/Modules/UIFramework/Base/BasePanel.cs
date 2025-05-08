using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面板的基类，所有的面板继承自他
///     管理面板的 4 种状态（生命周期）
/// </summary>
public class BasePanel : MonoBehaviour {

    /// <summary>
    /// 面板进入
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// 面板暂停
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// 面板恢复
    /// </summary>
    public virtual void OnResume()
    {

    }

    /// <summary>
    /// 面板退出
    /// </summary>
    public virtual void OnExit()
    {

    }
}
