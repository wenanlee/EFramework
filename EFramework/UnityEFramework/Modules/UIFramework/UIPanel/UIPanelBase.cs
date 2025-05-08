using EFramework.Tweens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : UIBase
{
    private CanvasGroup canvasGroup;
    public override void OnEnter()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.TweenFade(1, 0.5f).SetOnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }
    public override void OnExit()
    {
        canvasGroup.TweenFade(0, 0.5f).SetOnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }
    /// <summary>
    /// 暂停面板
    /// </summary>
    public override void OnPause()
    {
        // 暂停面板时，让主菜单面板不再和鼠标交互
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 继续面板
    /// </summary>
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true; // 启用鼠标交互
    }
}
