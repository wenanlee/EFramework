// 文件: DataTableWindow.cs

#if UNITY_EDITOR
// 注意：使用 Odin Inspector 前，请确保已在 Player Settings -> Scripting Define Symbols 中添加 'ODIN_INSPECTOR'
#if ODIN_INSPECTOR 
using EFramework.Unity.Utility; // 确保这个命名空间下的 FindScriptableObject 存在且有效
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
#else
// 如果没有 Odin Inspector，此文件其余部分将不会被编译
#endif // ODIN_INSPECTOR

using UnityEngine;

namespace EFramework.Unity.DataTable
{
#if ODIN_INSPECTOR
    public class DataTableWindow : OdinMenuEditorWindow
    {
        // --- 常量定义 ---
        private const string MENU_ITEM_PATH = "Tools/项目配置";
        private const string WINDOW_TITLE = "项目配置";
        private const string REFRESH_ALL_BUTTON_TEXT = "刷新所有列表";
        private const string REFRESH_CURRENT_BUTTON_TEXT = "刷新当前表";
        private const string ADD_ENTRY_BUTTON_TEXT = "添加";
        private const string GENERATE_ENUM_BUTTON_TEXT = "生成枚举";
        private const string EXPORT_JSON_BUTTON_TEXT = "导出到json";
        private const string CURRENT_TABLE_LABEL_PREFIX = "当前表: ";
        private const string CREATE_CONFIG_MENU_ITEM = "项目配置/点击此处创建新的项目配置";

        private DataTableBase currentSelectedTable;
        private bool hasInitializedMenuTree = false; // 标记是否已初始化过菜单树

        [UnityEditor.MenuItem(MENU_ITEM_PATH)]
        private static void OpenWindow()
        {
            var window = GetWindow<DataTableWindow>();
            window.titleContent = new GUIContent(WINDOW_TITLE);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            // OnEnable 不再负责创建 ProjectConfig，而是留给 BuildMenuTree 处理
        }

        // 刷新所有表的逻辑
        private void RefreshAll()
        {
            var projectConfig = ProjectConfig.Instance;
            if (projectConfig?.volume?.components == null)
            {
                Debug.LogWarning("无法刷新：ProjectConfig 或其 components 为空。");
                return;
            }

            foreach (var component in projectConfig.volume.components)
            {
                component.Refresh();
            }

            ForceMenuTreeRebuild();
        }

        // 刷新当前选中的表
        private void RefreshCurrentTable()
        {
            if (currentSelectedTable != null)
            {
                Debug.Log($"刷新当前表: {currentSelectedTable.TableName}");
                currentSelectedTable.Refresh(); // 实现具体刷新逻辑
                ForceMenuTreeRebuild();
            }
        }

        // 向当前表添加新条目
        private void AddToCurrentTable()
        {
            if (currentSelectedTable != null)
            {
                Debug.Log($"向表 {currentSelectedTable.TableName} 添加新条目");
                currentSelectedTable.Add(); // 实现具体添加逻辑
                ForceMenuTreeRebuild();
            }
        }

        // 生成枚举
        private void GenerateEnum()
        {
            if (currentSelectedTable != null)
            {
                Debug.Log($"为表 {currentSelectedTable.TableName} 生成枚举");
                currentSelectedTable.GenerateToEnumFile(); // 实现具体生成逻辑
            }
        }

        // 导出到JSON
        private void ExportToJson()
        {
            if (currentSelectedTable != null)
            {
                Debug.Log($"导出表 {currentSelectedTable.TableName} 到JSON"); ;
                currentSelectedTable.ExportToJson();
            }
        }

        protected override void OnBeginDrawEditors()
        {
            if (MenuTree == null)
                return;

            var toolbarHeight = MenuTree.Config.SearchToolbarHeight;

            // 更新当前选中的表
            UpdateSelectedTable();

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent(REFRESH_ALL_BUTTON_TEXT)))
                {
                    RefreshAll();
                }
                // 只有在选中TableSOBase时才显示额外的按钮
                if (currentSelectedTable != null)
                {
                    GUILayout.Space(10); // 分隔符

                    if (SirenixEditorGUI.ToolbarButton(new GUIContent(REFRESH_CURRENT_BUTTON_TEXT)))
                    {
                        RefreshCurrentTable();
                    }

                    if (SirenixEditorGUI.ToolbarButton(new GUIContent(ADD_ENTRY_BUTTON_TEXT)))
                    {
                        AddToCurrentTable();
                    }

                    if (SirenixEditorGUI.ToolbarButton(new GUIContent(GENERATE_ENUM_BUTTON_TEXT)))
                    {
                        GenerateEnum();
                    }

                    if (SirenixEditorGUI.ToolbarButton(new GUIContent(EXPORT_JSON_BUTTON_TEXT)))
                    {
                        ExportToJson();
                    }

                    // 显示当前选中的表名
                    GUILayout.FlexibleSpace();
                    GUILayout.Label($"{CURRENT_TABLE_LABEL_PREFIX}{currentSelectedTable.TableName}", EditorStyles.boldLabel);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        // 更新当前选中的TableSOBase对象
        private void UpdateSelectedTable()
        {
            if (MenuTree.Selection != null && MenuTree.Selection.SelectedValue is DataTableBase newSelection)
            {
                if (currentSelectedTable != newSelection)
                {
                    currentSelectedTable = newSelection;
                    Debug.Log($"选中表格: {currentSelectedTable.TableName}");
                }
            }
            else
            {
                currentSelectedTable = null;
            }
        }

        private void CreateProjectConfigAndRefresh()
        {
            var config = ScriptableObject.CreateInstance<ProjectConfig>();
            config.volume = new(); // 假设 Volume 是一个类，需要实例化
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

                // 重新加载 Instance 并强制刷新菜单树
                // ProjectConfig.ReloadInstance(); // 如果有这个方法
                ForceMenuTreeRebuild();
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Config.DrawSearchToolbar = true;

            // 尝试获取 ProjectConfig 实例
            var projectConfig = ProjectConfig.Instance;

            if (projectConfig == null)
            {
                // 如果 ProjectConfig 不存在，提供一个创建它的选项
                tree.Add(CREATE_CONFIG_MENU_ITEM, new CreateConfigAction(this));
                return tree;
            }

            // 添加 ProjectConfig 本身
            tree.Add("项目配置", projectConfig);

            // 添加所有 TableSOBase 组件
            if (projectConfig.volume?.components != null)
            {
                foreach (var component in projectConfig.volume.components)
                {
                    if (component != null) // 防止空引用
                    {
                        tree.Add(component.TableName, component);
                    }
                }
            }

            // 标记菜单树已根据有效配置初始化
            if (!hasInitializedMenuTree)
            {
                hasInitializedMenuTree = true;
                // 可以在这里做一次性的初始化工作，如果需要的话
            }

            return tree;
        }

        // 用于在菜单中触发创建操作的辅助类
        private class CreateConfigAction
        {
            private readonly DataTableWindow windowRef;

            public CreateConfigAction(DataTableWindow window)
            {
                this.windowRef = window;
            }

            [Button(ButtonSizes.Large)]
            public void ClickToCreate()
            {
                windowRef.CreateProjectConfigAndRefresh();
            }
        }
    }
#endif // ODIN_INSPECTOR
}
#endif // UNITY_EDITOR