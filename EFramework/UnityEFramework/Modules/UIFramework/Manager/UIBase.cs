using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.UIFramework
{
    public abstract class UIBase : MonoBehaviour
    {
        public bool Prewarm;

        public virtual void OnEnter() { /* 初始化逻辑 */ }
        public virtual void OnExit() { /* 关闭逻辑 */ }
        public virtual void OnPause() { /* 暂停逻辑 */ }
        public virtual void OnResume() { /* 恢复逻辑 */ }
    }
}
