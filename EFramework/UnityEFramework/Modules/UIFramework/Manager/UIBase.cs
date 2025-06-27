using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.UIFramework
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIBase : MonoBehaviour
    {
        public bool Prewarm;
        protected CanvasGroup canvasGroup;
        public virtual void OnShow(object obj = null) 
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        public virtual void OnHide() 
        { 
            if(canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        public virtual void OnPause() { /* 暂停逻辑 */ }
        public virtual void OnResume() { /* 恢复逻辑 */ }
    }
}
