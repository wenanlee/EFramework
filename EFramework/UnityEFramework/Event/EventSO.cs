using EFramework.Unity.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.Event
{
    public class EventSO : ScriptableObject
    {
        public string uuid;
        public string eventName;
        public object[] eventArgs;
        public string eventArgsJson;
    }
}
