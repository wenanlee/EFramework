using EFramework.Unity.DataTable;
using EFramework.Unity.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Event
{
    [Serializable]
    public class EventTableItemInfo:DataTableItemInfoBase
    {
        [ReadOnly]
        public string eventName;
        public string desc;
        [ReadOnly]
        public List<EventValueTypeInfo> argTypes;
        [ReadOnly]
        public EventSO asset;
        public EventTableItemInfo(EventSO eventSO)
        {
            uuid = eventSO.uuid;
            desc = eventSO.desc;
            eventName = eventSO.name;
            asset = eventSO;
            argTypes = eventSO.argTypes;
        }
    }
}
