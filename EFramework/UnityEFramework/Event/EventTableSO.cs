using EFramework.Unity.DataTable;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFramework.Unity.Event
{
    public class EventTableSO : TableSOBase
    {
        [TableList]
        public List<EventSO> SOList;
        [Button]
        public override void Add()
        {
            SOList.Add(new EventSO());
        }

        public override void Refresh()
        {
            SOList = ScriptableObjectUtility.FindAllScriptableObjects<EventSO>().ToList();
        }
    }
}
