using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Tweens;
using EFramework.Core;
namespace EFramework.Unity.UIFramework
{
    /// <summary>
    /// 任务面板
    /// </summary>
    public class TaskPanel : UIPanelBase
    {

        private void Awake()
        {
            EventManager.AddListener<string>("ShowTaskPanel", ShowTaskPanel);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<string>("ShowTaskPanel", ShowTaskPanel);
        }

        private void ShowTaskPanel(string panelTypeStr)
        {
            print(panelTypeStr);
        }


        /// <summary>
        /// 关闭当前面板，关闭按钮点击事件
        /// </summary>
        public void OnClosePanel()
        {
            UIManager.Instance.HideUI(); // 出栈
        }
    }
}