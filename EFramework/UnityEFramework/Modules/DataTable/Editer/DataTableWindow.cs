#if UNITY_EDITOR
#if ODIN_INSPECTOR
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;


#endif
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    // 示例数据类
    public class MyConfigData : ScriptableObject
    {
#if ODIN_INSPECTOR
        [BoxGroup("Settings")]
#endif
        public string Name;
#if ODIN_INSPECTOR
        [BoxGroup("Settings"), Range(0, 100)]
#endif
        public int Health;
#if ODIN_INSPECTOR
        [BoxGroup("Settings"), ColorUsage(true)]
#endif
        public Color Color;
    }
#if ODIN_INSPECTOR
    public class DataTableWindow : OdinMenuEditorWindow
    {
        ProjectConfig projectConfig;
        [UnityEditor.MenuItem("Tools/项目配置")]
        private static void OpenWindow()
        {
            var window = GetWindow<DataTableWindow>();
            window.titleContent = new GUIContent("项目配置");
            window.Show();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            projectConfig ??= ScriptableObjectUtility.FindScriptableObject<ProjectConfig>();
            if (projectConfig == null)
                CreateProjectConfig();

            var tables = ScriptableObjectUtility.FindAllScriptableObjects<TableSOBase>();
            projectConfig.tables = new List<TableInfo>();
            foreach (TableSOBase table in tables) 
            {
                projectConfig.tables.Add(new TableInfo(table.name,table));
            }
        }
        private void Refresh()
        {
            foreach (var item in projectConfig.tables)
            {
                item.tableObj.Refresh();
                Debug.Log("刷新！");
            }
        }
        protected override void OnBeginDrawEditors()
        {
            if (MenuTree == null)
                return;

            var toolbarHeight = MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("刷新所有列表")))
                {
                    Refresh();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }



        private void CreateProjectConfig()
        {
            var config = ScriptableObject.CreateInstance<ProjectConfig>();
            string assetPath = EditorUtility.SaveFilePanelInProject(
                "保存 ProjectConfig",
                "ProjectConfig.asset",
                "asset",
                "请选择保存位置"
            );

            if (!string.IsNullOrEmpty(assetPath))
            {
                AssetDatabase.CreateAsset(config, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = config;
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Config.DrawSearchToolbar = true; // 添加搜索栏

            tree.Add("项目配置", projectConfig);
            foreach (var item in projectConfig.tables)
            {
                Debug.Log($"Table: {item.tableName}, Object: {item.tableObj}");
                tree.Add(item.tableName, item.tableObj);
            }

            return tree;
        }
    }
    [Serializable]
    public class TableInfo
    {
        public string tableName;
        [ShowInInspector]
        public TableSOBase tableObj;
        public TableInfo(string tableName, TableSOBase tableObj)
        {
            this.tableName = tableName;
            this.tableObj = tableObj;
        }
    }
#endif
}
#endif