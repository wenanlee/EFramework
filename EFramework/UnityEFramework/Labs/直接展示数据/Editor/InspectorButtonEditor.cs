using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

[CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
public class InspectorButtonEditor : Editor
{
    private Dictionary<string, object[]> methodParams = new Dictionary<string, object[]>();
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var targetType = target.GetType();
        var methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var m in methods)
        {
            var attr = m.GetCustomAttribute<InspectorButtonAttribute>();
            if (attr == null) continue;

            var parameters = m.GetParameters();
            var key = $"{m.Name}_{target.GetInstanceID()}";

            if (!methodParams.ContainsKey(key))
            {
                object[] paramValues = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    paramValues[i] = GetDefault(parameters[i].ParameterType);
                methodParams[key] = paramValues;
            }
            var values = methodParams[key];

            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                values[i] = DrawField(p.Name, p.ParameterType, values[i]);
            }

            EditorGUILayout.Space(3);
            if (GUILayout.Button(attr.DisplayName ?? m.Name))
            {
                m.Invoke(target, values);
            }
            EditorGUILayout.Space(10);
        }
    }

    private object DrawField(string label, Type type, object value)
    {
        // 支持Unity对象
        if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            return EditorGUILayout.ObjectField(label, value as UnityEngine.Object, type, true);

        // 支持常用结构体
        if (type == typeof(Vector2))
            return EditorGUILayout.Vector2Field(label, value is Vector2 v2 ? v2 : Vector2.zero);
        if (type == typeof(Vector3))
            return EditorGUILayout.Vector3Field(label, value is Vector3 v3 ? v3 : Vector3.zero);
        if (type == typeof(Vector4))
            return EditorGUILayout.Vector4Field(label, value is Vector4 v4 ? v4 : Vector4.zero);
        if (type == typeof(Color))
            return EditorGUILayout.ColorField(label, value is Color c ? c : Color.white);

        // 基础类型
        if (type == typeof(int))
            return EditorGUILayout.IntField(label, value is int v ? v : 0);
        if (type == typeof(float))
            return EditorGUILayout.FloatField(label, value is float v ? v : 0f);
        if (type == typeof(string))
            return EditorGUILayout.TextField(label, value as string ?? "");
        if (type == typeof(bool))
            return EditorGUILayout.Toggle(label, value is bool v ? v : false);
        if (type.IsEnum)
        {
            if (value == null || value.GetType() != type)
                value = Enum.GetValues(type).GetValue(0);
            return EditorGUILayout.EnumPopup(label, (Enum)value);
        }

        // 支持自定义struct/class
        if ((type.IsClass || type.IsValueType) && type != typeof(string))
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            if (value == null) value = Activator.CreateInstance(type);
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in fields)
            {
                var oldVal = field.GetValue(value);
                var newVal = DrawField("  " + field.Name, field.FieldType, oldVal);
                field.SetValue(value, newVal);
            }
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
            {
                if (!prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Length > 0) continue;
                var oldVal = prop.GetValue(value);
                var newVal = DrawField("  " + prop.Name, prop.PropertyType, oldVal);
                prop.SetValue(value, newVal);
            }
            return value;
        }

        EditorGUILayout.LabelField($"{label} (不支持的类型: {type.Name})");
        return value;
    }

    private object GetDefault(Type t)
    {
        if (t.IsValueType) return Activator.CreateInstance(t);
        return null;
    }
}