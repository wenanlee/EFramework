using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ToggleGroupExamplesComponent))]
[CanEditMultipleObjects]
public class ToggleGroupExamplesEditor : Editor
{
    private ToggleGroupExamplesComponent _target;
    private SerializedProperty _toggleListProp;

    private void OnEnable()
    {
        _target = (ToggleGroupExamplesComponent)target;
        _toggleListProp = serializedObject.FindProperty("toggleList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawHeader();
        DrawModuleArray();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawHeader()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("模块配置", EditorStyles.boldLabel);
        EditorGUILayout.Space();
    }

    private void DrawModuleArray()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_toggleListProp, true);
        if (EditorGUI.EndChangeCheck())
        {
            // 自动刷新数组内容（避免 UI 崩溃）
            serializedObject.Update();
        }
    }

    [CustomPropertyDrawer(typeof(ModuleBase), true)]
    public class ModuleBaseDrawer : PropertyDrawer
    {
        private  float TitleHeight = EditorGUIUtility.singleLineHeight * 1.5f;
        private const float PropertyIndent = 16f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return TitleHeight + EditorGUI.GetPropertyHeight(property, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // 获取模块对象
            var moduleRef = property.objectReferenceValue as ModuleBase;
            if (moduleRef == null)
            {
                EditorGUI.LabelField(position, "模块未初始化");
                EditorGUI.EndProperty();
                return;
            }

            // 计算标题区域
            var titleRect = new Rect(position.x, position.y, position.width, TitleHeight);
            var contentRect = new Rect(
                position.x,
                position.y + TitleHeight,
                position.width,
                EditorGUI.GetPropertyHeight(property, true)
            );

            // 绘制折叠标题（类似 Volume 风格）
            var foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };

            EditorGUI.BeginChangeCheck();
            bool isExpanded = EditorGUI.Foldout(titleRect, true, "模块配置", foldoutStyle);
            if (EditorGUI.EndChangeCheck())
            {
                // 可选：保存折叠状态（需添加字段）
            }

            // 绘制模块类型选择器
            EditorGUI.indentLevel = 0;
            var moduleType = moduleRef.GetType();
            Type newType = DrawTypeSelector(titleRect, moduleRef.GetType(), property);
            if (newType != null && newType != moduleRef.GetType())
            {
                // 创建新实例并更新属性
                var newInstance = CreateModuleInstance(newType);
                property.objectReferenceValue = newInstance;
            }
            // 如果展开，绘制模块属性
            if (isExpanded)
            {
                EditorGUI.indentLevel = 1;
                EditorGUI.PropertyField(contentRect, property, true);
                EditorGUI.indentLevel = 0;
            }

            EditorGUI.EndProperty();
        }

        private Type DrawTypeSelector(Rect titleRect, Type currentType, SerializedProperty property)
        {
            var types = ToggleGroupExamplesComponent.cachedModuleTypes.ToArray();
            var currentIndex = Array.IndexOf(types, currentType);
            var popupRect = new Rect(titleRect.x + titleRect.width - 120, titleRect.y, 120, titleRect.height);
            currentIndex = EditorGUI.Popup(popupRect, currentIndex, types.Select(t => t.Name).ToArray());

            return currentIndex >= 0 ? types[currentIndex] : null;
        }

        private ModuleBase CreateModuleInstance(Type type)
        {
            try
            {
                return ScriptableObject.CreateInstance(type) as ModuleBase;
            }
            catch (Exception e)
            {
                Debug.LogError($"无法创建模块实例 {type.Name}: {e.Message}");
                return null;
            }
        }
    }
}