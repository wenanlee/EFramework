using UnityEngine;
using UnityEditor;
using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using EFramework.Unity.Utility;

namespace EFramework.Unity.DataTable
{
    public class CreateScriptableObjectWindow : OdinEditorWindow
    {
        [ReadOnly,LabelText("文件存储路径")]
        public string defaultFolderPath;
        [LabelText("文件名")]
        public string fileName;
        private string folderPath;

        [SerializeField, HideLabel]
        [InlineEditor(InlineEditorModes.GUIOnly, InlineEditorObjectFieldModes.Hidden)]
        private ScriptableObject newSO;

        private Action<string> onCreateCallback;

        public static void OpenWindow<T>(string defaultPath, Action<string> onCreateCallback) where T : ScriptableObject
        {
            var window = GetWindow<CreateScriptableObjectWindow>();
            window.defaultFolderPath = defaultPath;
            window.folderPath = defaultPath;
            window.onCreateCallback = onCreateCallback;
            window.newSO = ScriptableObject.CreateInstance<T>();
            window.titleContent = new GUIContent($"创建 {typeof(T).Name}");
            window.minSize = new Vector2(400, 500);
            window.ShowUtility();
        }

        [BoxGroup("文件设置")]
        [LabelText("文件名")]
        [Required]
        public string FileName
        {
            get => fileName;
            set => fileName = value;
        }

        [BoxGroup("文件设置")]
        [LabelText("保存路径")]
        [FolderPath]
        public string FolderPath
        {
            get => folderPath;
            set => folderPath = value;
        }

        [BoxGroup("配置")]
        [LabelText("配置")]
        [ShowInInspector]
        public ScriptableObject Configuration => newSO;

        [HorizontalGroup("Buttons")]
        [Button("创建", ButtonSizes.Large)]
        [EnableIf("@!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(folderPath)")]
        [GUIColor(0, 1, 0)]
        private void Create()
        {
            bool success = ScriptableObjectUtility.CreateScriptableObject(newSO, folderPath, fileName);

            if (success)
            {
                onCreateCallback?.Invoke(fileName);
                EditorUtility.DisplayDialog("成功", $"{fileName} 已创建", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "创建失败", "确定");
            }
            Close();
        }

        [HorizontalGroup("Buttons")]
        [Button("取消", ButtonSizes.Large)]
        [GUIColor(1, 0.5f, 0.5f)]
        private void Cancel()
        {
            fileName = string.Empty;
        }

        protected override void OnDestroy()
        {
            // 如果窗口关闭时没有创建，销毁临时创建的 ScriptableObject
            if (newSO != null && !AssetDatabase.Contains(newSO))
            {
                DestroyImmediate(newSO);
            }
            base.OnDestroy();
        }
    }
}