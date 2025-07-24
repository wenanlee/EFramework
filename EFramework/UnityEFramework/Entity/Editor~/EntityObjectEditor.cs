using UnityEditor;
using UnityEngine;
namespace EFramework.Unity.Entity
{

    [CustomEditor(typeof(EntityObject), true)]
    public class EntityObjectEditor : Editor
    {
        private SerializedObject volumeSerializedObject;
        private SerializedProperty volumeProperty;
        private Editor volumeEditor;

        void OnEnable()
        {
            volumeProperty = serializedObject.FindProperty("volume");
            CreateVolumeEditor();
        }

        void OnDisable()
        {
            DestroyVolumeEditor();
        }

        private void CreateVolumeEditor()
        {
            DestroyVolumeEditor();
            if (volumeProperty != null && volumeProperty.objectReferenceValue != null)
            {
                volumeSerializedObject = new SerializedObject(volumeProperty.objectReferenceValue);
                volumeEditor = Editor.CreateEditor(volumeProperty.objectReferenceValue);
            }
        }

        private void DestroyVolumeEditor()
        {
            if (volumeEditor != null)
            {
                DestroyImmediate(volumeEditor);
                volumeEditor = null;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // 1. 绘制默认属性（排除volume字段）
            DrawPropertiesExcluding(serializedObject, "volume");

            // 2. 单独绘制volume字段
            EditorGUILayout.PropertyField(volumeProperty);

            // 3. 检查volume引用变化
            if (volumeProperty.objectReferenceValue == null)
            {
                DestroyVolumeEditor();
            }
            else if (volumeEditor == null || volumeEditor.target != volumeProperty.objectReferenceValue)
            {
                CreateVolumeEditor();
            }

            // 4. 绘制volume的内联视图
            if (volumeProperty.objectReferenceValue != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Volume Settings", EditorStyles.boldLabel);

                EditorGUI.BeginChangeCheck();
                volumeSerializedObject.Update();
                volumeEditor.OnInspectorGUI();
                if (EditorGUI.EndChangeCheck())
                {
                    volumeSerializedObject.ApplyModifiedProperties();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}