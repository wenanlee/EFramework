using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EFramework.Unity.UIFramework
{
    /// <summary>
    /// UI配置数据（ScriptableObject）
    /// </summary>
    [CreateAssetMenu(fileName ="UIConfig",menuName = "EFramework/Unity/UIFramework/UIConfig")]
    public class UIConfig : SOBase
    {
        [SerializeField] private List<UIConfigInfo> uiConfigs = new();

        public IReadOnlyList<UIConfigInfo> UIConfigs => uiConfigs;
        private readonly Dictionary<string, UIConfigInfo> _guidLookup = new();

        private void OnEnable()
        {
            BuildLookupTable();
        }

        private void BuildLookupTable()
        {
            _guidLookup.Clear();
            foreach (var item in uiConfigs)
            {
                if (_guidLookup.ContainsKey(item.GUID))
                {
                    Debug.LogError($"发现重复的GUID: {item.GUID} ({item.Name})");
                    continue;
                }
                _guidLookup.Add(item.GUID, item);
            }
        }

        public bool TryGetConfig(string guid, out UIConfigInfo item)
        {
            return _guidLookup.TryGetValue(guid, out item);
        }
    }
    /// <summary>
    /// 单个UI配置项
    /// </summary>
    [Serializable]
    public class UIConfigInfo
    {
        [SerializeField] private string guid;
        [SerializeField] private UIType type;
        [SerializeField] private UIBase prefab;

        public string GUID => guid;
        public UIType Type => type;
        public UIBase Prefab => prefab;
        public string Name => prefab ? prefab.name : "Invalid Prefab";
    }
    public enum UIType
    {
        Panel,
        Widget,
        Window
    }
}
