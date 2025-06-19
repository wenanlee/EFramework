using EFramework.Unity.Command;
using EFramework.Unity.UIFramework;
using EFramework.Unity.Utility;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    // 示例数据类
    public class MyConfigData : ScriptableObject
    {
        [BoxGroup("Settings")] public string Name;
        [BoxGroup("Settings"), Range(0, 100)] public int Health;
        [BoxGroup("Settings"), ColorUsage(true)] public Color Color;
    }

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
        protected override void OnBeginDrawEditors()
        {
            if (MenuTree == null)
                return;

            var toolbarHeight = MenuTree.Config.SearchToolbarHeight;
            projectConfig ??= ScriptableObjectUtility.FindScriptableObject<ProjectConfig>();
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                // 1. 使用局部变量避免重复调用
                bool projectConfigExists =  projectConfig!= null;

                // 2. 创建 Project 按钮（延迟执行文件操作）
                if (!projectConfigExists)
                {
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("创建Project")))
                    {
                        // 延迟执行文件操作避免中断 GUI 布局
                        EditorApplication.delayCall += CreateProjectConfig;
                    }
                }

                // 3. 其他按钮保持不变
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("刷新菜单树")))
                {
                    //Refresh();
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("刷新编辑器缓存")))
                {
                    //GameSupportEditorUtility.Refresh();
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
            projectConfig ??= ScriptableObjectUtility.FindScriptableObject<ProjectConfig>();
            Debug.Log(projectConfig);
            //if()
            // 添加菜单项（直接显示对象）
            tree.Add("ProjectConfig", projectConfig);
            if (projectConfig != null) {
                foreach (var config in projectConfig.tableDict)
                {
                    tree.Add(config.Key, config.Value);
                }
            }
            // 添加ScriptableObject实例
            var data = AssetDatabase.LoadAssetAtPath<MyConfigData>("Assets/MyConfigData.asset");
            if (data == null)
            {
                data = CreateInstance<MyConfigData>();
                AssetDatabase.CreateAsset(data, "Assets/MyConfigData.asset");
                AssetDatabase.SaveAssets();
            }
            tree.Add("Data/Configuration Data", data);

            // 添加自定义方法绘制
            //tree.Add("Actions/Generate Objects", new GenerateObjectAction());
            //tree.Add("Actions/Delete All", new DeleteObjectsAction());

            // 添加Unity对象
            tree.Add("Scene Objects", FindObjectOfType<Light>());

            // 添加图标
            tree.Add("配置表", this, EditorIcons.House);

            return tree;
        }
    }
}
