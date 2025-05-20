using EFramework.Unity.Command;
using EFramework.Unity.UIFramework;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace EFramework.Unity
{

    public class ProjectConfigWindow : OdinEditorWindow
    {

        [UnityEditor.MenuItem("Tools/项目配置")]
        private static void OpenWindow()
        {
            var window = GetWindow<ProjectConfigWindow>();
            window.titleContent = new GUIContent("项目配置");
            window.Show();
        }
        [InlineEditor]
        [InlineButton("CreateNewConfigIfNull","+",ShowIf = "@this.projectConfig == null")]
        public ProjectConfig projectConfig;
        [InlineEditor]
        [InlineButton("CreateNewConfigIfNull", "+", ShowIf = "@this.commandEvents == null")]
        public CommandEventSO commandEvents;
        [Button("重新加载SO")]
        public void ReLoadSO()
        {
            projectConfig = Resources.Load<ProjectConfig>("ProjectConfig");
            commandEvents = Resources.Load<CommandEventSO>("CommandEvents");
        }
        protected override void Initialize()
        {
            // 加载或创建配置
            ReLoadSO();
            if (projectConfig == null)
            {
                //config = CreateInstance<ProjectConfig>();
                //AssetDatabase.CreateAsset(config, $"{projectParentPath}/ProjectConfig.asset");
                //AssetDatabase.SaveAssets();
                //AssetDatabase.Refresh();
            }
        }
        private void CreateNewConfigIfNull()
        {
            if (projectConfig == null)
            {
                CreateNewConfig();
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "配置已存在，无需创建", "确定");
            }
        }

        private void CreateNewConfig()
        {
            projectConfig = CreateInstance<ProjectConfig>();
            if (Directory.Exists(Application.dataPath + "/")) { }
            AssetDatabase.CreateAsset(projectConfig, "Assets/Config/ProjectConfig.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("成功", "已创建新的项目配置文件", "确定");
        }
        [Button(ButtonSizes.Large)]
        private void SaveConfig()
        {
            EditorUtility.SetDirty(projectConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }
    [CreateAssetMenu(fileName = "ProjectConfig",menuName = "EFramework/ProjectConfig")]
    public class ProjectConfig : ScriptableObject
    {
        [LabelText("项目名称")]
        public string projectName;

        [LabelText("项目路径")]
        [FolderPath(ParentFolder = "Assets")]
        public string projectParentPath;
        
    }
}
