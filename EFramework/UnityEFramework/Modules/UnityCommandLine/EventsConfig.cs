using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity
{
    [CreateAssetMenu(fileName = "EventsConfig", menuName = "EFramework/UnityCommandLine/EventsConfig", order = 1)]
    public class EventsConfig : ScriptableObject
    {
        [LabelText("系统事件")]
        public List<string> systemEvents = new List<string>();
        [LabelText("游戏生命周期事件")]
        public List<string> gameLifecycleEvents = new List<string>();
        [LabelText("Unity生命周期事件")]
        public List<string> unityLifecycleEvents = new List<string>();
        [LabelText("自定义事件")]
        public List<string> customEvents = new List<string>();

        [ValueDropdown("GetAllEvents")]
        public string eventName;
        [Button("Test")]
        public void Test()
        {
            Debug.Log(eventName);
        }
        public List<string> GetAllEvents()
        {
            List<string> allEvents = new List<string>();
            allEvents.Add("None");
            allEvents.AddRange(systemEvents);
            allEvents.AddRange(gameLifecycleEvents);
            allEvents.AddRange(unityLifecycleEvents);
            allEvents.AddRange(customEvents);
            return allEvents;
        }
    }
}
