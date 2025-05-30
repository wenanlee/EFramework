using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(UniversalDictionarySO))]
public class UniversalDictionarySOEditor : Editor
{
    private string lastHash = "";
    public override void OnInspectorGUI()
    {
        var so = (UniversalDictionarySO)target;
        var items = so.items;
        // 生成当前 items 的 JSON 字符串
        string currentJson = GenerateJson(items);

        if (currentJson != lastHash)
        {
            SaveJsonToFile(currentJson);
            lastHash = currentJson;
        }

        if (GUILayout.Button("Add Entry"))
        {
            items.Add(new UniversalKeyValuePair() { key = "名字", type = typeof(string), value = "李佳琦" });
        }
        for (int i = 0; i < items.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            //EditorGUILayout.BeginHorizontal();
            //if (GUILayout.Button("Remove")) { items.RemoveAt(i); break; }
            //EditorGUILayout.EndHorizontal();
            Debug.Log(items.Count);
            var val = items[i].value;
            DrawField(items[i].key, items[i].type, items[i].value);
            EditorGUILayout.EndVertical();
        }
        if (GUI.changed)
            EditorUtility.SetDirty(so);
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

    private string GenerateJson(List<UniversalKeyValuePair> items)
    {
        return EFramework.Utility.JSONWriter.ToJson(items);
        //return JsonConvert.SerializeObject(items);
    }

    private void SaveJsonToFile(string json)
    {
        string path = "Assets/GeneratedDictionary.json";
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();
        Debug.Log($"JSON saved to: {path}");
    }
}