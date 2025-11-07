using EFramework.Unity.Entity;
using EFramework.Unity.Event;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
#if UNITY_EDITOR
    public class ProjectMgr
    {
        [InlineEditor(Expanded = true)]
        private static ProjectConfig projectConfig;

        public static ProjectConfig ProjectConfig
        {
            get
            {
                if (projectConfig == null)
                {
                    projectConfig = ScriptableObjectUtility.FindScriptableObject<ProjectConfig>();
                    if (projectConfig == null)
                    {
                        Debug.LogError("找不到ProjectConfig，请确保已创建并配置ProjectConfig");
                    }
                }
                return projectConfig; 
            }
        }
    }
#endif
}
