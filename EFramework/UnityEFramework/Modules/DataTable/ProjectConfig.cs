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
#if ODIN_INSPECTOR
        
        [ShowInInspector, TableList,LabelText("表单")]
        public List<TableInfo> tables =new List<TableInfo>();
        [Button("项目初始化"),ShowIf("@tables.Count == 0"),EnableIf("@string.IsNullOrEmpty(projectParentPath) == false")]
        public void ProjectInit()
        {
            ScriptableObjectUtility.CreateScriptableObject<EventSOList>(projectParentPath + "/Resources/Data/Event", "事件列表");
        }
#endif
    }
}
#endif