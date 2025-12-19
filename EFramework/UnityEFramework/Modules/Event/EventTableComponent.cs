using EFramework.Unity.DataTable;
using EFramework.Unity.Entity;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFramework.Unity.Event
{
    [LabelText("事件表组件")]
    [Serializable]
    public class EventTableComponent : DataTableBaseComponent
    {
        public override string tableName => "事件表";
        [ShowInInspector, TableList(NumberOfItemsPerPage = 20, IsReadOnly = true, ShowPaging = true), Searchable]
        public List<EventTableItemInfo> SOLst = new();
        public Dictionary<string, EventTableItemInfo> SODict => SOLst.ToDictionary(i => i.uuid);

        [Button]
        public override void Add()
        {
#if UNITY_EDITOR
            CreateScriptableObjectWindow.OpenWindow<EventSO>(
                ProjectConfig.Instance.soDataPath + "Events/",
                (soName) =>
                {
                    Refresh();
                }
            );
#endif
        }

        [Button]
        public override void Refresh()
        {
            SOLst.Clear();
            var eventSOLst = ScriptableObjectUtility.FindAllScriptableObjects<EventSO>();
            foreach (var item in eventSOLst)
            {
                EventTableItemInfo eventTableItem = new EventTableItemInfo(item);
                SOLst.Add(eventTableItem);
            }
        }

        public override void ExportToJson()
        {
            var json = JsonUtility.ToJson(SOLst);
            Debug.Log(json);
            
        }

        public override void GenerateToEnumFile()
        {
            EFramework.Unity.Utility.FileUtility.GenerateConstantsFile(ProjectConfig.Instance.projectParentPath,"EventItems",SOLst.ToDictionary(x=>$"{x.eventName}_{x.desc}_{x.uuid}",x=> x.uuid));
        }
    }
}
