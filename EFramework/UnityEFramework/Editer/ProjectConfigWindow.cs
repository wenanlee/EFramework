using EFramework.Unity.UIFramework;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace EFramework.Unity
{

    public class ProjectConfigWindow : OdinEditorWindow
    {

        [UnityEditor.MenuItem("Tools/项目配置")]
        private static void OpenWindow()
        {
            GetWindow<ProjectConfigWindow>().Show();
        }

        [LabelText("项目名称")]
        public string projectNmae;
        [LabelText("项目路径")]
        [FolderPath(ParentFolder = "Assets")]
        public string projectParentPath;
        [InlineEditor, PropertyOrder(100)]
        public ProjectConfig config;
        [Button("创建项目配置"), ButtonGroup, EnableIf("@this.config == null")]
        public void CreateProject()
        {
            config = CreateInstance<ProjectConfig>();
            config.projectName = projectNmae;
            config.projectParentPath = projectParentPath;
            config.jsonPath = $"{projectParentPath}/Config";
            config.jsonName = "ProjectConfig";
            string folderPath = projectParentPath + "/Config";
            FileIOHelper.WriteScriptableObject(folderPath, "ProjectConfig.asset", config);
        }
        [Button("更新项目配置"), ButtonGroup, EnableIf("@this.config != null")]
        public void UpdateProject()
        {

        }
    }
   
    public class ProjectConfig : SOBase
    {
        [LabelText("项目名称"),ReadOnly]
        public string projectName;

        [LabelText("项目路径")]
        [FolderPath(ParentFolder = "Assets"),ReadOnly]
        public string projectParentPath;

        public override void LoadFromJson()
        {
            Debug.Log(Application.dataPath + "/" + jsonPath + "/"+ jsonName + ".json");
            var json = FileIOHelper.ReadStringFromFile(Application.dataPath +"/"+ jsonPath + "/", jsonName + ".json");
            JsonUtility.FromJsonOverwrite(json, this);
        }
        public override void SaveJson()
        {
            var json = JsonUtility.ToJson(this);
            FileIOHelper.WriteStringToFile(json, Application.dataPath + "/" + jsonPath + "/", jsonName + ".json");
            AssetDatabase.Refresh();
        }
    }
}
