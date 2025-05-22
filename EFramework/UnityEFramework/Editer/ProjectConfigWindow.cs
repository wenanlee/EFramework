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
        //[InlineButton("CreateNewConfigIfNull","+",ShowIf = "@this.projectConfig == null")]
        public ProjectConfig projectConfig;

        protected override void Initialize()
        {
            projectConfig = Resources.Load<ProjectConfig>("ProjectConfig");
            if (projectConfig == null)
            {
               
                //config = CreateInstance<ProjectConfig>();
                //AssetDatabase.CreateAsset(config, $"{projectParentPath}/ProjectConfig.asset");
                //AssetDatabase.SaveAssets();
                //AssetDatabase.Refresh();
            }
            projectConfig.LoadAllSOFiles();
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

        [InlineEditor]
        [InlineButton("@CreateNewConfigIfNull(this.commandEvents)", "+", ShowIf = "@this.commandEvents == null")]
        public CommandEventSO commandEvents;

        public void LoadAllSOFiles()
        {
            commandEvents = Resources.Load<CommandEventSO>("CommandEventSO");
        }
        private void CreateNewConfigIfNull<T>(T t) where T:ScriptableObject
        {
            if (t == null)
            {
                CreateNewConfig<T>(t,projectParentPath,typeof(T).Name);
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "配置已存在，无需创建", "确定");
            }
        }

        private void CreateNewConfig<T>(T t,string path,string fileName) where T : ScriptableObject
        {
            t = CreateInstance<T>();
            string fullPath = Path.Combine(Application.dataPath, path);
            Debug.Log(fullPath);
            Debug.Log(path + "/" + fileName + ".asset");
            if (Directory.Exists(fullPath) == false)
                Directory.CreateDirectory(fullPath);

            AssetDatabase.CreateAsset(t, "Assets/"+path +"/"+fileName+".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            LoadAllSOFiles();
            EditorUtility.DisplayDialog("成功", "已创建新的项目配置文件", "确定");
        }
        [Button(ButtonSizes.Large)]
        private void SaveConfig()
        {
            //EditorUtility.SetDirty(projectConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
