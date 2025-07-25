#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.UIFramework
{

    /// <summary>
    /// UI管理系统（GUID + SO版）
    /// </summary>
    public class UIMgr : MonoSingleton<UIMgr>
    {

        //#region Serialized Fields
        //[Header("UI配置")]
        //[SerializeField] private UIConfig _uiConfig;

//[Header("UI层级")]
//[SerializeField] private Transform _panelLayer;
//[SerializeField] private Transform _widgetLayer;
//[SerializeField] private Transform _windowLayer;
//#endregion

//#region Private Fields
//private readonly Dictionary<UIType, Transform> _layerMapping = new();
//private readonly Dictionary<string, UIBase> _instancePool = new();
//private readonly Stack<UIBase> _uiStack = new(10);
//#endregion

//#region Lifecycle
//protected override void Awake()
//{
//    base.Awake();
//    Initialize();
//}

//private void Initialize()
//{
//    ValidateConfig();
//    SetupLayerMapping();
//    PrewarmPool();
//}

//private void ValidateConfig()
//{
//    if (_uiConfig == null)
//        throw new NullReferenceException("UIConfig未配置");

//    if (_panelLayer == null || _widgetLayer == null || _windowLayer == null)
//        throw new NullReferenceException("UI层级未完整配置");
//}

//private void SetupLayerMapping()
//{
//    _layerMapping.Add(UIType.Panel, _panelLayer);
//    _layerMapping.Add(UIType.Widget, _widgetLayer);
//    _layerMapping.Add(UIType.Window, _windowLayer);
//}
//#endregion

//#region Public API
///// <summary> 显示指定GUID的UI </summary>
//public void ShowUI(string guid)
//{
//    if (string.IsNullOrEmpty(guid))
//    {
//        Debug.LogError("GUID不能为空");
//        return;
//    }

//    if (!_uiConfig.TryGetConfig(guid, out var config))
//    {
//        Debug.LogError($"找不到GUID对应的UI配置: {guid}");
//        return;
//    }

//    HandleUIState(config);
//    var instance = GetOrCreateInstance(config);
//    instance.OnEnter();
//    _uiStack.Push(instance);
//}

///// <summary> 关闭最顶层的UI </summary>
//public void HideTopUI()
//{
//    if (_uiStack.Count == 0)
//    {
//        Debug.LogWarning("UI栈为空");
//        return;
//    }

//    var topUI = _uiStack.Pop();
//    topUI.OnExit();

//    if (_uiStack.TryPeek(out var newTop))
//        newTop.OnResume();
//}
//#endregion

//#region Core Logic
//private void HandleUIState(UIConfigItem config)
//{
//    if (_uiStack.TryPeek(out var current))
//    {
//        if (current.GetType() == config.Prefab.GetType())
//            current.OnExit();
//        else
//            current.OnPause();
//    }
//}

//private UIBase GetOrCreateInstance(UIConfigItem config)
//{
//    if (_instancePool.TryGetValue(config.GUID, out var instance))
//    {
//        instance.gameObject.SetActive(true);
//        return instance;
//    }

//    var newInstance = Instantiate(config.Prefab, _layerMapping[config.Type]);
//    _instancePool.Add(config.GUID, newInstance);
//    return newInstance;
//}

//private void PrewarmPool()
//{
//    foreach (var config in _uiConfig.Items)
//    {
//        if (config.Prefab.Prewarm)
//            GetOrCreateInstance(config).gameObject.SetActive(false);
//    }
//}
//#endregion
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public Dictionary<string, UIBase> uiDict { get; private set; } = new Dictionary<string, UIBase>();
        private void Awake()
        {
            // 初始化UI管理器
            Init();
        }

        private void Init()
        {
            foreach (var item in GetComponentsInChildren<UIBase>(true))
            {
                uiDict.Add(item.Uuid, item);
            }
        }

        public virtual void ShowUI(string uuid, object arg = null)
        {
            
        }
    }
}