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
    public class ProjectConfig : ScriptableObjectSingleton<ProjectConfig>
    {
#if ODIN_INSPECTOR
        [LabelText("项目名称")]
#endif
        public string projectName;
#if ODIN_INSPECTOR
        [LabelText("项目路径")]
        [FolderPath(ParentFolder = ""), BoxGroup("路径")]
#endif
        public string projectParentPath;

        [ShowInInspector, FolderPath, BoxGroup("路径")]
        public string prefabPath => projectParentPath + "/Prefabs/";
        [ShowInInspector, FolderPath, BoxGroup("路径")]
        public string resourcesPath => projectParentPath + "/Resources/";
        [ShowInInspector, FolderPath, BoxGroup("路径")]
        public string soDataPath => projectParentPath + "/Resources/Data/";

        public DataTableVolume volume;

        private void CreateDataTable()
        {
            // 创建数据表组件
        }
    }
}