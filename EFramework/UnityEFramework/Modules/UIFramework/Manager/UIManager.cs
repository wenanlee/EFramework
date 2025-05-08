using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.UIFramework
{
    /// <summary>
    ///     整个 UI框架 的管理器
    ///     解析 Json 面板信息，保存到 panelPathDict 字典里
    ///     创建保存所有面板的实例，panelDict 字典
    ///     管理保存所有显示的面板，栈
    /// </summary>
    public class UIManager : MonoSingleton<UIManager>
    {

        private void Awake()
        {
            Init();
        }
        public void Init()
        {
            //this.Work().AddWork(LoadUIPrefabs).Run();
            uiPrefabs = new List<UIBase>();
            uiPoolDict = new Dictionary<int, UIBase>();
            uiDict = new Dictionary<int, UIBase>();
            Debug.Log(CanvasTransform.name);
        }
        /// <summary>
        /// 加载UI预制体
        /// </summary>
        private void LoadUIPrefabs()
        {
            var panels = Resources.LoadAll<UIPanelBase>("UI/Panel");
            uiPrefabs.AddRange(panels);
            foreach (var panel in panels)
            {
                uiDict.Add(panel.name.StringStratToInt(4), panel);
            }
            var windows = Resources.LoadAll<UIWindowBase>("UI/Window");
            uiPrefabs.AddRange(windows);
            foreach (var window in windows)
            {
                uiDict.Add(window.name.StringStratToInt(4), window);
            }
            var widgets = Resources.LoadAll<UIWidgetBase>("UI/Widget");
            uiPrefabs.AddRange(widgets);
            foreach (var widget in widgets)
            {
                uiDict.Add(widget.name.StringStratToInt(4), widget);
            }

        }

        // 会将生成的面板放在 Canvas 的下面，用于设置一个父子关系
        [SerializeField] private Transform canvasTransform;
        [SerializeField] private Transform panelLayer_down;
        [SerializeField] private Transform widgetLayer_middle;
        [SerializeField] private Transform windowLayer_top;

        [SerializeField] private List<UIBase> uiPrefabs;
        private Dictionary<int, UIBase> uiDict;
        private Dictionary<int, UIBase> uiPoolDict;

        [SerializeField] private UIBase currentPanel;
        [SerializeField] private UIBase currentWidget;
        [SerializeField] private UIBase currentWindow;

        public Transform CanvasTransform
        {
            get
            {
                if (canvasTransform == null)
                {
                    canvasTransform = FindObjectOfType<GameRoot>().transform;
                    panelLayer_down = canvasTransform.Find("Down");
                    widgetLayer_middle = canvasTransform.Find("Middle");
                    windowLayer_top = canvasTransform.Find("Top");
                }
                return canvasTransform;
            }
        }

        public void ShowUI(Enum type)
        {
            ShowUI(type.GetHashCode());
        }
        public void ShowUI(int type)
        {
            switch (type.GetHashCode() / 1000)
            {
                case 1:
                    currentPanel?.OnExit();
                    var ui = GetUIObj(type);
                    if (currentPanel != ui)
                    {
                        currentPanel = ui;
                        currentPanel?.OnEnter();
                    }
                    break;
                case 2:
                    currentPanel?.OnPause();

                    currentWidget?.OnExit();
                    currentWidget = GetUIObj(type);
                    currentWidget?.OnEnter();
                    break;
                case 3:
                    currentPanel?.OnPause();
                    currentWidget?.OnPause();

                    currentWindow?.OnExit();
                    currentWindow = GetUIObj(type);
                    currentWindow?.OnEnter();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 把某个页面出栈，栈顶面板出栈，启用第二个面板
        /// </summary>
        public void HideUI()
        {
            currentPanel?.OnExit();
            currentWidget?.OnExit();
            currentWindow?.OnExit();
        }

        public UIBase GetUIObj(int type)
        {
            if (uiPoolDict.ContainsKey(type))
                return uiPoolDict[type];
            else if (uiDict.ContainsKey(type))
            {
                var uiGO = Instantiate(uiDict[type], windowLayer_top, false);
                uiPoolDict.Add(type, uiGO);
                return uiGO;
            }
            Debug.LogError($"没有这个ID:{type}的UIWindow");
            return null;
        }
    }
}