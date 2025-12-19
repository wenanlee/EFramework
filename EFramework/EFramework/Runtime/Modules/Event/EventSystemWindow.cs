using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif
using UnityEngine;
using EFramework.Core;
#if UNITY_EDITOR
public class EventMonitorWindow : OdinEditorWindow
{
    [MenuItem("Tools/事件监控窗口")]
    private static void OpenWindow()
    {
        var window = GetWindow<EventMonitorWindow>();
        window.titleContent = new GUIContent("事件监控");
        window.Show();
    }

    [TabGroup("事件监听器")]
    [ShowInInspector]
    [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
    private List<EventListenerInfo> eventListeners = new List<EventListenerInfo>();

    [TabGroup("事件统计")]
    [ShowInInspector]
    [DisplayAsString]
    [HideLabel]
    private string eventStatistics;

    [TabGroup("事件测试")]
    [ShowInInspector]
    private EventTestData eventTestData = new EventTestData();

    [TabGroup("设置")]
    [ShowInInspector]
    private MonitorSettings settings = new MonitorSettings();

    private DateTime lastRefreshTime;
    private bool isAutoRefresh = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshEventData();
        lastRefreshTime = DateTime.Now;
    }
    protected override void OnImGUI()
    {
        if (isAutoRefresh && (DateTime.Now - lastRefreshTime).TotalSeconds >= settings.refreshInterval)
        {
            RefreshEventData();
            lastRefreshTime = DateTime.Now;
        }
        base.OnImGUI();
    }

    [Button("刷新数据", ButtonSizes.Large)]
    [PropertyOrder(-1)]
    private void RefreshEventData()
    {
        CollectEventListeners();
        UpdateStatistics();
        Repaint();
    }

    [Button("清除所有事件", ButtonSizes.Medium)]
    [PropertyOrder(-1)]
    private void ClearAllEvents()
    {
        if (EditorUtility.DisplayDialog("确认清除", "确定要清除所有事件监听器吗？", "确定", "取消"))
        {
            // 这里需要根据实际的EventManager实现来清除事件
            // 由于EventManager是静态类，可能需要通过反射来清除
            Debug.Log("清除所有事件监听器");
            RefreshEventData();
        }
    }

    private void CollectEventListeners()
    {
        eventListeners.Clear();

        try
        {
            // 通过反射获取所有EventAgent实例
            var eventManagerType = typeof(EventManager);
            var allTypes = Assembly.GetAssembly(eventManagerType).GetTypes();

            foreach (var type in allTypes)
            {
                if (type.IsGenericType && type.Name.StartsWith("EventAgent"))
                {
                    CollectEventAgentListeners(type);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"收集事件监听器时出错: {ex.Message}");
        }
    }

    private void CollectEventAgentListeners(Type eventAgentType)
    {
        try
        {
            var instanceProperty = eventAgentType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            if (instanceProperty == null) return;

            var instance = instanceProperty.GetValue(null);
            if (instance == null) return;

            // 获取事件字典
            var eventDicField = eventAgentType.GetField("eventDic", BindingFlags.NonPublic | BindingFlags.Instance);
            if (eventDicField == null) return;

            var eventDic = eventDicField.GetValue(instance) as System.Collections.IDictionary;
            if (eventDic == null) return;

            foreach (System.Collections.DictionaryEntry entry in eventDic)
            {
                var eventId = entry.Key;
                var delegateObj = entry.Value as Delegate;

                if (delegateObj != null)
                {
                    var invocations = delegateObj.GetInvocationList();
                    foreach (var invocation in invocations)
                    {
                        var listenerInfo = new EventListenerInfo
                        {
                            EventId = eventId?.ToString() ?? "Null",
                            EventType = eventId?.GetType().Name ?? "Unknown",
                            ListenerTarget = invocation.Target,
                            MethodName = invocation.Method.Name,
                            ParameterTypes = GetParameterTypes(invocation.Method),
                            IsAlive = IsListenerAlive(invocation.Target)
                        };

                        eventListeners.Add(listenerInfo);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"收集EventAgent监听器时出错: {ex.Message}");
        }
    }

    private string GetParameterTypes(MethodInfo method)
    {
        var parameters = method.GetParameters();
        if (parameters.Length == 0) return "无参数";

        return string.Join(", ", parameters.Select(p => p.ParameterType.Name));
    }

    private bool IsListenerAlive(object target)
    {
        if (target == null) return false;

        // 如果是UnityEngine.Object，检查是否被销毁
        if (target is UnityEngine.Object unityObj)
        {
            return unityObj != null;
        }

        return true;
    }

    private void UpdateStatistics()
    {
        var totalListeners = eventListeners.Count;
        var aliveListeners = eventListeners.Count(l => l.IsAlive);
        var deadListeners = totalListeners - aliveListeners;

        var eventsByType = eventListeners
            .GroupBy(l => l.EventType)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count);

        eventStatistics = $"事件统计 (更新时间: {DateTime.Now:HH:mm:ss})\n\n";
        eventStatistics += $"总监听器数量: {totalListeners}\n";
        eventStatistics += $"存活监听器: {aliveListeners}\n";
        eventStatistics += $"失效监听器: {deadListeners}\n\n";

        eventStatistics += "按事件类型统计:\n";
        foreach (var stat in eventsByType)
        {
            eventStatistics += $"{stat.Type}: {stat.Count}\n";
        }
    }

    [Serializable]
    public class EventListenerInfo
    {
        [DisplayAsString]
        [LabelText("事件ID")]
        public string EventId { get; set; }

        [DisplayAsString]
        [LabelText("事件类型")]
        public string EventType { get; set; }

        [DisplayAsString]
        [LabelText("监听目标")]
        public object ListenerTarget { get; set; }

        [DisplayAsString]
        [LabelText("方法名称")]
        public string MethodName { get; set; }

        [DisplayAsString]
        [LabelText("参数类型")]
        public string ParameterTypes { get; set; }

        [DisplayAsString]
        [LabelText("状态")]
        [ShowInInspector]
        public string Status => IsAlive ? "正常" : "已失效";

        [HideInInspector]
        public bool IsAlive { get; set; }

        [Button("查看详情")]
        private void ShowDetails()
        {
            if (ListenerTarget is UnityEngine.Object targetObj)
            {
                EditorGUIUtility.PingObject(targetObj);
                Selection.activeObject = targetObj;
            }
            else
            {
                Debug.Log($"监听器目标: {ListenerTarget}, 方法: {MethodName}, 参数: {ParameterTypes}");
            }
        }
    }

    [Serializable]
    public class EventTestData
    {
        [ValueDropdown("GetEventTypes")]
        [LabelText("事件类型")]
        public string EventType = "Enum";

        [ShowIf("EventType", "Enum")]
        [LabelText("枚举事件")]
        public TestEnum enumEvent = TestEnum.TestEvent1;

        [ShowIf("EventType", "String")]
        [LabelText("字符串事件")]
        public string stringEvent = "TestEvent";

        [ShowIf("EventType", "Int")]
        [LabelText("整数事件")]
        public int intEvent = 1;

        [ShowIf("EventType", "Object")]
        [LabelText("对象事件")]
        public UnityEngine.Object objectEvent;

        [Button("发送事件")]
        private void SendTestEvent()
        {
            try
            {
                switch (EventType)
                {
                    case "Enum":
                        EventManager.SendEvent(enumEvent);
                        break;
                    case "String":
                        EventManager.SendEvent(stringEvent);
                        break;
                    case "Int":
                        EventManager.SendEvent(intEvent);
                        break;
                    case "Object":
                        if (objectEvent != null)
                            EventManager.SendEvent(objectEvent);
                        else
                            Debug.LogWarning("对象事件目标不能为空");
                        break;
                }
                Debug.Log($"测试事件已发送: {GetEventId()}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"发送测试事件时出错: {ex.Message}");
            }
        }

        private object GetEventId()
        {
            return EventType switch
            {
                "Enum" => enumEvent,
                "String" => stringEvent,
                "Int" => intEvent,
                "Object" => objectEvent,
                _ => "Unknown"
            };
        }

        private List<string> GetEventTypes()
        {
            return new List<string> { "Enum", "String", "Int", "Object" };
        }

        public enum TestEnum
        {
            TestEvent1,
            TestEvent2,
            TestEvent3
        }
    }

    [Serializable]
    public class MonitorSettings
    {
        [LabelText("自动刷新")]
        [Tooltip("启用后自动定期刷新事件数据")]
        public bool autoRefresh = true;

        [LabelText("刷新间隔(秒)")]
        [ShowIf("autoRefresh")]
        [MinValue(1)]
        public float refreshInterval = 2f;

        [LabelText("显示失效监听器")]
        [Tooltip("是否在列表中显示已失效的监听器")]
        public bool showDeadListeners = true;

        [LabelText("事件过滤")]
        [Tooltip("根据事件ID过滤显示")]
        public string eventFilter = "";
    }
}
#endif