using EFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace EFramework.Unity.UIFramework
{
    /// <summary>
    /// 主面板
    /// </summary>
    public class MainMenuPanel : UIPanelBase
    {
        private Button btnTask;
        private void Awake()
        {
            btnTask = transform.Find("Task").GetComponent<Button>();
            btnTask.onClick.AddListener(OnTaskButtonClick);
        }
        private void OnTaskButtonClick()
        {
            OnPushPanel("Task");
            EventManager.SendEvent("ShowTaskPanel", "Task");
        }
        /// <summary>
        /// 将指定面板入栈
        /// </summary>
        /// <param name="panelTypeStr"></param>
        public void OnPushPanel(string panelTypeStr)
        {
            // 将字符串转换为枚举类型
            UIPanelType panelType = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeStr);
            // 将面板入栈
            UIManager.Instance.ShowUI(panelType);
        }
    }
}