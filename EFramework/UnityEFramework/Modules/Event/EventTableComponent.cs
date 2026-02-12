using EFramework.Unity.DataTable;
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
    public class EventTableComponent : DataTableBaseComponent<EventSO, EventVolume>
    {
        public override string TableName => "事件表";
        public override string Name => "事件表组件";

        public override string parentPath => "Events";
        public override void ExportToJson()
        {
            var json = JsonUtility.ToJson(SOLst);
            Debug.Log(json);
            
        }

        public override void GenerateToEnumFile()
        {
            //FileUtility.GenerateConstantsFile(ProjectConfig.Instance.projectParentPath,"EventItems",SOLst.ToDictionary(x=>$"{x.eve}_{x.desc}_{x.uuid}",x=> x.uuid));
        }
    }
}
