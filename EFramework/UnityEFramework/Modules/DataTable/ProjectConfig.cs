using EFramework.Unity.Command;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    [CreateAssetMenu(fileName = "ProjectConfig", menuName = "EFramework/ProjectConfig")]
    public class ProjectConfig : ScriptableObject
    {
        [LabelText("项目名称")]
        public string projectName;

        [LabelText("项目路径")]
        //[FolderPath(ParentFolder = "Assets")]
        public string projectParentPath;

        [ValueDropdown("GetAllScriptableObjectTypes", IsUniqueList = true), ShowInInspector]
        public Dictionary<string, Type> tableDict = new Dictionary<string, Type>();
        private IEnumerable<ValueDropdownItem<Type>> GetAllScriptableObjectTypes()
        {
            var items = new List<ValueDropdownItem<Type>>();
            // 获取所有程序集
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    continue;
                }
                foreach (var type in types)
                {
                    if (type == null) continue;
                    if (type.IsAbstract || type.IsGenericType) continue;
                    if (typeof(ScriptableObject).IsAssignableFrom(type))
                    {
                        items.Add(new ValueDropdownItem<Type>(type.FullName, type));
                    }
                }
            }
            return items.Distinct();
        }
        //[PropertyDropdown]
        //[InlineButton("@CreateNewConfigIfNull(this.commandEvents)", "+")]
        public CommandEventSO commandEvents;

        public void LoadAllSOFiles()
        {
            commandEvents = Resources.Load<CommandEventSO>("CommandEventSO");
        }
        private void CreateNewConfigIfNull<T>(T t) where T : ScriptableObject
        {
            if (t == null)
            {
                CreateNewConfig<T>(t, projectParentPath, typeof(T).Name);
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "配置已存在，无需创建", "确定");
            }
        }

        private void CreateNewConfig<T>(T t, string path, string fileName) where T : ScriptableObject
        {
            t = CreateInstance<T>();
            string fullPath = Path.Combine(Application.dataPath, path);
            Debug.Log(fullPath);
            Debug.Log(path + "/" + fileName + ".asset");
            if (Directory.Exists(fullPath) == false)
                Directory.CreateDirectory(fullPath);

            AssetDatabase.CreateAsset(t, "Assets/" + path + "/" + fileName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            LoadAllSOFiles();
            EditorUtility.DisplayDialog("成功", "已创建新的项目配置文件", "确定");
        }
        [NaButton]
        private void SaveConfig()
        {
            //EditorUtility.SetDirty(projectConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
