using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace EFramework.Unity.Utility
{
    public static class ScriptableObjectUtility
    {
        /// <summary>
        /// 创建并保存一个 ScriptableObject 实例到指定路径
        /// </summary>
        /// <typeparam name="T">ScriptableObject 类型</typeparam>
        /// <param name="targetPath">目标路径（相对于 Assets 文件夹）</param>
        /// <param name="fileName">文件名（不带扩展名）</param>
        /// <returns>创建的 ScriptableObject 实例</returns>
        public static T CreateScriptableObject<T>(string targetPath, string fileName) where T : ScriptableObject
        {
            // 确保在编辑器环境下执行
#if UNITY_EDITOR
            // 检查目标类型是否有效
            if (typeof(T).IsAbstract || typeof(T).IsInterface)
            {
                Debug.LogError($"Cannot create instance of abstract or interface type: {typeof(T)}");
                return null;
            }

            // 确保目标路径存在
            string fullDirectoryPath = Path.Combine(Application.dataPath, targetPath);
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
                Debug.Log($"Created directory: {fullDirectoryPath}");
            }

            // 创建 ScriptableObject 实例
            T asset = ScriptableObject.CreateInstance<T>();

            // 构建完整路径
            string fullPath = Path.Combine(targetPath, fileName + ".asset");
            fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

            // 保存资源
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // 重要：强制重新导入资源以正确链接脚本
            AssetDatabase.ImportAsset(fullPath, ImportAssetOptions.ForceUpdate);

            Debug.Log($"Successfully created {typeof(T)} at: {fullPath}");
            return asset;
#else
        Debug.LogError("CreateScriptableObject is only available in the Unity Editor");
        return null;
#endif
        }
        /// <summary>
        /// 按类型查找所有ScriptableObject资源
        /// List<MyScriptableObject> allSOs = FindAllScriptableObjects<MyScriptableObject>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindAllScriptableObjects<T>() where T : ScriptableObject
        {
            return AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                .Where(asset => asset != null)
                .ToList();
        }
        public static T FindScriptableObject<T>(string name) where T : ScriptableObject
        {
            var assets = AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                .Where(asset => asset != null && asset.name == name)
                .ToList();
            return assets.FirstOrDefault();
        }
        public static T FindScriptableObject<T>() where T : ScriptableObject
        {
            var assets = AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                .Where(asset => asset != null)
                .ToList();
            return assets.FirstOrDefault();
        }
    }
    public static class AssetDataUnility
    {
        public static T[] FindAllAssets<T>(string folderPath) where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets("", new string[] { folderPath });
            T[] assets = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return assets;
        }
        public static T FindAsset<T>(string folderPath) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(folderPath);
        }
    }
}

