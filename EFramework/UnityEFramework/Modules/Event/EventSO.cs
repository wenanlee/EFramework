using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.Event
{
    [CreateAssetMenu(fileName = "Event", menuName = "EFramework/Unity/Event")]
    public class EventSO : ScriptableObject
    {
        [ReadOnly]
        public string uuid;
        public string desc;
        [TableList(ShowIndexLabels=true)]
        public List<EventValueTypeInfo> argTypes;
        public EventSO()
        {
            uuid = UUID.New();
            argTypes = new List<EventValueTypeInfo>();
        }
    }
    [Serializable]
    public class EventValueTypeInfo
    {
        public EventValueType argType;
        public string argDesc;
    }
    public enum EventValueType
    {
        None,
        UUID,
        String,
        Int,
        Float,
        Bool,
        GameObject,
        Vector2,
        Vector3,
        Color,
        Sprite,
        Texture,
        AudioClip,
        VideoClip,
        Material,
        Transform,
        RectTransform
    }
}
