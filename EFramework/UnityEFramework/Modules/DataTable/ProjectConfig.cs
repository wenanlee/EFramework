#if UNITY_EDITOR
#if ODIN_INSPECTOR
using EFramework.Unity.Event;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    public class ProjectConfig : ScriptableObject
    {
#if ODIN_INSPECTOR
        [LabelText("项目名称")]
#endif
        public string projectName;
#if ODIN_INSPECTOR
        [LabelText("项目路径")]
        [FolderPath(ParentFolder = "")]
#endif
        public string projectParentPath;
        public string prefabPath => projectParentPath + "/Prefab/";
        public string resourcesPath => projectParentPath + "/Resources/";
        public string soDataPath => projectParentPath + "/Resources/Data/";
#if ODIN_INSPECTOR
        [TableList(IsReadOnly = true)]
        public List<TableInfo> tableSOLst;

        public EntityTableSO entityTableSO;
        public EventTableSO eventTableSO;

        [Button("项目初始化"), EnableIf("@string.IsNullOrEmpty(projectParentPath) == false")]
        public void ProjectInit()
        {
            tableSOLst = new();
            FindSOAsset<EntityTableSO>(out entityTableSO, "Entity", "实体表");
            FindSOAsset<EventTableSO>(out eventTableSO, "Event", "事件列表");

        }
        public void FindSOAsset<T>(out T t, string tablePath, string tableName) where T : TableSOBase
        {
            t = ScriptableObjectUtility.FindScriptableObject<T>();
            if (t == null)
                t = ScriptableObjectUtility.CreateScriptableObject<T>(soDataPath + tablePath, tableName);
            tableSOLst.Add(new TableInfo(tableName, t));
        }
#endif
        public void LoadSO()
        {

        }
    }
}
#endif