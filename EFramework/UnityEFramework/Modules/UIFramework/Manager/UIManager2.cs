using System;
using System.Collections;
using System.Collections.Generic;
using EFramework.Unity.Utility;
using UnityEngine;
namespace EFramework.Unity.UIFramework
{

    /// <summary>
    ///     整个 UI框架 的管理器
    ///     解析 Json 面板信息，保存到 panelPathDict 字典里
    ///     创建保存所有面板的实例，panelDict 字典
    ///     管理保存所有显示的面板，栈
    /// </summary>
    public class UIManager2 : MonoSingleton<UIManager2>
    {
        private void Awake()
        {
            Init();
        }
        public void Init()
        {
            //this.Work().AddWork(LoadUIPrefabs).Run();
            panelPoolDict = new Dictionary<int, UIPanelBase>();
            widgetPoolDict = new Dictionary<int, UIWidgetBase>();
            windowPoolDict = new Dictionary<int, UIWindowBase>();
            Debug.Log(CanvasTransform.name);
        }
        /// <summary>
        /// 加载UI预制体
        /// </summary>
        private void LoadUIPrefabs()
        {
            uiPrefabs = new List<UIBase>();
            var panels = Resources.LoadAll<UIPanelBase>("UI/Panel");
            uiPrefabs.AddRange(panels);
            panelDict = new Dictionary<int, UIPanelBase>();
            foreach (var panel in panels)
            {
                panelDict.Add(panel.name.StringStratToInt(4), panel);
            }
            var windows = Resources.LoadAll<UIWindowBase>("UI/Window");
            uiPrefabs.AddRange(windows);
            windowDict = new Dictionary<int, UIWindowBase>();
            foreach (var window in windows)
            {
                windowDict.Add(window.name.StringStratToInt(4), window);
            }
            var widgets = Resources.LoadAll<UIWidgetBase>("UI/Widget");
            uiPrefabs.AddRange(widgets);
            foreach (var widget in widgets)
            {
                widgetDict.Add(widget.name.StringStratToInt(4), widget);
            }

        }

        // 会将生成的面板放在 Canvas 的下面，用于设置一个父子关系
        [SerializeField] private Transform canvasTransform;
        [SerializeField] private Transform panelLayer_down;
        [SerializeField] private Transform widgetLayer_middle;
        [SerializeField] private Transform windowLayer_top;

        [SerializeField] private List<UIBase> uiPrefabs;
        private Dictionary<int, UIPanelBase> panelDict;
        private Dictionary<int, UIWindowBase> windowDict;
        private Dictionary<int, UIWidgetBase> widgetDict;

        private Dictionary<int, UIPanelBase> panelPoolDict;
        private Dictionary<int, UIWindowBase> windowPoolDict;
        private Dictionary<int, UIWidgetBase> widgetPoolDict;

        [SerializeField] private UIPanelBase currentPanel;
        [SerializeField] private UIWidgetBase currentWidget;
        [SerializeField] private UIWindowBase currentWindow;

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
                    currentPanel = GetUIPanel(type);
                    currentPanel?.OnEnter();
                    break;
                case 2:
                    currentPanel?.OnPause();

                    currentWidget?.OnExit();
                    currentWidget = GetUIWidget(type);
                    currentWidget?.OnEnter();
                    break;
                case 3:
                    currentPanel?.OnPause();
                    currentWidget?.OnPause();

                    currentWindow?.OnExit();
                    currentWindow = GetUIWindow(type);
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
            currentPanel?.OnExit(); // 退出面板
            currentWidget?.OnExit();
            currentWindow?.OnExit();
        }

        public UIPanelBase GetUIPanel(int type)
        {
            if (panelPoolDict.ContainsKey(type))
                return panelPoolDict[type];
            if (panelDict.ContainsKey(type))
            {
                var uiGO = Instantiate(panelDict[type], panelLayer_down, false);
                panelPoolDict.Add(type, uiGO);
                return uiGO;
            }
            Debug.LogError($"没有这个ID:{type}的UIPanel");
            return null;
        }
        public UIWidgetBase GetUIWidget(int type)
        {
            if (widgetPoolDict.ContainsKey(type))
                return widgetPoolDict[type];
            else if (widgetDict.ContainsKey(type))
            {
                var uiGO = Instantiate(widgetDict[type], widgetLayer_middle, false);
                widgetPoolDict.Add(type, uiGO);
                return uiGO;
            }
            Debug.LogError($"没有这个ID:{type}的UIWidget");
            return null;
        }
        public UIWindowBase GetUIWindow(int type)
        {
            if (windowPoolDict.ContainsKey(type))
                return windowPoolDict[type];
            else if (windowDict.ContainsKey(type))
            {
                var uiGO = Instantiate(windowDict[type], windowLayer_top, false);
                windowPoolDict.Add(type, uiGO);
                return uiGO;
            }
            Debug.LogError($"没有这个ID:{type}的UIWindow");
            return null;
        }
    }
}