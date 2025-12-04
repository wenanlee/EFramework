using EFramework.Unity.DataTable;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.Event
{
    [CreateAssetMenu(fileName ="Event",menuName = "EFramework/Unity/Event")]
    public class EventSO : ScriptableObject
    {
        [ReadOnly]
        public string uuid;
        public string desc;
        public EventValueType valueType;
        public EventSO() 
        {
            uuid = UUID.New();
        }
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
