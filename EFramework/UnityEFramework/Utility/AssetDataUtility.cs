using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EFramework.Unity.Utility
{
#if UNITY_EDITOR
    /// <summary>
    /// 资源数据工具类，提供在编辑器中对各种资源类型的操作支持
    /// </summary>
    public static class AssetDataUtility
    {
        /// <summary>
        /// 获取所有指定类型的预制体
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="folders">搜索的文件夹路径</param>
        /// <returns>找到的预制体组件列表</returns>
        public static List<T> GetAllPrefabs<T>(params string[] folders) where T : Component
        {
            try
            {
                var assets = folders.Length == 0
                    ? AssetDatabase.FindAssets($"t:Prefab")
                    : AssetDatabase.FindAssets($"t:Prefab", folders);

                return assets
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path))
                    .Where(prefab => prefab != null && prefab.GetComponent<T>() != null)
                    .Select(prefab => prefab.GetComponent<T>())
                    .Where(component => component != null)
                    .ToList();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"获取 {typeof(T).Name} 类型预制体时出错: {ex.Message}");
                return new List<T>();
            }
        }

        /// <summary>
        /// 获取所有包含指定组件类型的预制体
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="folders">搜索的文件夹路径</param>
        /// <returns>找到的预制体列表</returns>
        public static List<GameObject> GetAllPrefabs(Type type, params string[] folders)
        {
            if (type == null || !typeof(Component).IsAssignableFrom(type))
            {
                Debug.LogError("提供的类型无效。类型必须是 Component 的派生类。");
                return new List<GameObject>();
            }

            try
            {
                var assets = folders.Length == 0
                    ? AssetDatabase.FindAssets($"t:Prefab")
                    : AssetDatabase.FindAssets($"t:Prefab", folders);

                return assets
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path))
                    .Where(prefab => prefab != null && prefab.GetComponent(type) != null)
                    .ToList();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"获取 {type.Name} 类型预制体时出错: {ex.Message}");
                return new List<GameObject>();
            }
        }

        /// <summary>
        /// 获取指定文件夹中的所有指定类型资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="folderPath">文件夹路径</param>
        /// <returns>找到的资源数组</returns>
        public static T[] FindAllAssets<T>(string folderPath) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogError("文件夹路径不能为空");
                return new T[0];
            }

            try
            {
                string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new string[] { folderPath });
                T[] assets = new T[guids.Length];

                for (int i = 0; i < guids.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
                }

                return assets.Where(asset => asset != null).ToArray();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"在路径 {folderPath} 中查找 {typeof(T).Name} 类型资源时出错: {ex.Message}");
                return new T[0];
            }
        }

        /// <summary>
        /// 在指定路径查找指定类型的资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="assetPath">资源完整路径</param>
        /// <returns>找到的资源，未找到则返回 null</returns>
        public static T FindAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogError("资源路径不能为空");
                return null;
            }

            try
            {
                return AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"在路径 {assetPath} 加载 {typeof(T).Name} 类型资源时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 根据名称查找指定类型的资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="assetName">资源名称</param>
        /// <param name="folders">搜索的文件夹路径</param>
        /// <returns>找到的资源，未找到则返回 null</returns>
        public static T FindAssetByName<T>(string assetName, params string[] folders) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("资源名称不能为空");
                return null;
            }

            try
            {
                var assets = folders.Length == 0
                    ? AssetDatabase.FindAssets($"{assetName} t:{typeof(T).Name}")
                    : AssetDatabase.FindAssets($"{assetName} t:{typeof(T).Name}", folders);

                return assets
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                    .Where(asset => asset != null && asset.name == assetName)
                    .FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"根据名称 {assetName} 查找 {typeof(T).Name} 类型资源时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取项目中所有指定类型的资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>找到的资源列表</returns>
        public static List<T> FindAllAssetsOfType<T>() where T : UnityEngine.Object
        {
            try
            {
                return AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                    .Where(asset => asset != null)
                    .ToList();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"查找所有 {typeof(T).Name} 类型资源时出错: {ex.Message}");
                return new List<T>();
            }
        }

        /// <summary>
        /// 创建指定类型的预制体
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="folderPath">保存路径</param>
        /// <param name="prefabName">预制体名称</param>
        /// <returns>创建的预制体组件引用</returns>
        public static T CreatePrefab<T>(string folderPath, string prefabName) where T : Component
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogError("文件夹路径不能为空");
                return null;
            }

            if (string.IsNullOrEmpty(prefabName))
            {
                Debug.LogError("预制体名称不能为空");
                return null;
            }

            try
            {
                // 确保文件夹存在
                if (!AssetDatabase.IsValidFolder(folderPath))
                {
                    Debug.LogError($"文件夹路径不存在: {folderPath}");
                    return null;
                }

                // 创建 GameObject 并添加组件
                GameObject gameObject = new GameObject(prefabName);
                T component = gameObject.AddComponent<T>();

                // 构建完整路径
                string assetPath = $"{folderPath}/{prefabName}.prefab";
                string uniquePath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

                // 创建预制体
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, uniquePath);

                // 销毁临时对象
                UnityEngine.Object.DestroyImmediate(gameObject);

                if (prefab != null)
                {
                    Debug.Log($"成功创建预制体 {prefabName} 位于: {uniquePath}");
                    return prefab.GetComponent<T>();
                }
                else
                {
                    Debug.LogError($"创建预制体失败: {prefabName}");
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"创建预制体 {prefabName} 时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取资源的 GUID
        /// </summary>
        /// <param name="asset">资源对象</param>
        /// <returns>资源的 GUID</returns>
        public static string GetAssetGUID(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                Debug.LogError("资源对象不能为 null");
                return string.Empty;
            }

            try
            {
                string path = AssetDatabase.GetAssetPath(asset);
                return AssetDatabase.AssetPathToGUID(path);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"获取资源 GUID 时出错: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <returns>是否存在</returns>
        public static bool AssetExists(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return false;
            }

            try
            {
                return AssetDatabase.LoadMainAssetAtPath(assetPath) != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除指定路径的资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <returns>是否删除成功</returns>
        public static bool DeleteAsset(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogError("资源路径不能为空");
                return false;
            }

            if (!AssetExists(assetPath))
            {
                Debug.LogWarning($"要删除的资源不存在: {assetPath}");
                return false;
            }

            try
            {
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.Refresh();
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"删除资源 {assetPath} 时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 重命名资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="newName">新名称（不带扩展名）</param>
        /// <returns>是否重命名成功</returns>
        public static bool RenameAsset(string assetPath, string newName)
        {
            if (string.IsNullOrEmpty(assetPath) || string.IsNullOrEmpty(newName))
            {
                Debug.LogError("资源路径和新名称不能为空");
                return false;
            }

            if (!AssetExists(assetPath))
            {
                Debug.LogError($"资源不存在: {assetPath}");
                return false;
            }

            try
            {
                string directory = System.IO.Path.GetDirectoryName(assetPath);
                string extension = System.IO.Path.GetExtension(assetPath);
                string newPath = $"{directory}/{newName}{extension}";

                string result = AssetDatabase.RenameAsset(assetPath, newName);
                AssetDatabase.Refresh();

                return string.IsNullOrEmpty(result);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"重命名资源 {assetPath} 为 {newName} 时出错: {ex.Message}");
                return false;
            }
        }
    }
#endif
}
