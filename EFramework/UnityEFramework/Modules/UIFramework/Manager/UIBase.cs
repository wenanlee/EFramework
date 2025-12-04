using EFramework.Unity.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.UIFramework
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIBase : GameEntity
    {
        protected CanvasGroup canvasGroup;
        protected bool isShow=false;
        public bool IsShow => isShow;
        public virtual void OnShow(object obj = null) 
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isShow = true;

        }
        public virtual void OnHide(object obj = null) 
        {
            
            if(canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Debug.Log($"关闭UI {name} {canvasGroup.alpha} {canvasGroup.interactable} {canvasGroup.blocksRaycasts}");
            isShow = false;
        }
        public virtual void OnPause() { /* 暂停逻辑 */ }
        public virtual void OnResume() { /* 恢复逻辑 */ }
    }
}
