#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
namespace EFramework.Unity.UIFramework
{

    /// <summary>
    /// UI管理系统（GUID + SO版）
    /// </summary>
    public class UIMgr : MonoSingleton<UIMgr>
    {

  
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public Dictionary<string, UIBase> uiDict { get; private set; } = new Dictionary<string, UIBase>();
        public Dictionary<Type, UIBase> uiDictByType = new();

        public virtual void ShowUI(string uuid, object arg = null)
        {
            
        }
        public virtual void HideUI(string uuid)
        {

        }

        public void ShowUI<T>(object arg) where T : UIBase
        {
            if(uiDictByType.ContainsKey(typeof(T)) ==false)
            {
                T t=GetComponentInChildren<T>();
                uiDictByType.Add(typeof(T), t);
            }
            uiDictByType[typeof(T)].OnShow(arg);
        }

        public void HideUI<T>() where T : UIBase
        {
            if (uiDictByType.ContainsKey(typeof(T)) == false)
            {
                return;
            }
            uiDictByType[typeof(T)].OnHide();
        }
    }
}