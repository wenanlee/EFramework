using EFramework.Unity.DataTable;
using EFramework.Unity.ECS;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.Event
{
    [CreateAssetMenu(fileName = "Event", menuName = "EFramework/Unity/Event")]
    public class EventSO : ScriptableObject
    {
       public EventVolume eventVolume;
        private void OnEnable()
        {
            // 当字段为空时，创建默认实例
            if (eventVolume == null)
            {
                eventVolume = new EventVolume();
            }

            // 确保 UUID 存在（如果还未赋值，生成新的）
            if (string.IsNullOrEmpty(eventVolume.uuid))
            {
                eventVolume.uuid = UUID.New();
            }
        }
    }
    [Serializable]
    public class EventVolume : DataTableItemInfoBase<EventSO>
    {
        public string desc;
        [TableList(ShowIndexLabels = true)]
        public List<EventValueTypeInfo> argTypes;
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
