
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
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
#if UNITY_EDITOR
            // 参数有效性检查
            if (string.IsNullOrEmpty(targetPath))
            {
                Debug.LogError("目标路径不能为空");
                return null;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("文件名不能为空");
                return null;
            }

            // 检查目标类型是否有效
            if (typeof(T).IsAbstract || typeof(T).IsInterface)
            {
                Debug.LogError($"无法创建抽象类或接口类型的实例: {typeof(T)}");
                return null;
            }
            if (targetPath.StartsWith("Assets/"))
            {
                targetPath = targetPath.Substring("Assets/".Length);
            }
            try
            {
                // 确保目标路径存在
                string fullDirectoryPath = Path.Combine(Application.dataPath, targetPath);
                if (!Directory.Exists(fullDirectoryPath))
                {
                    Directory.CreateDirectory(fullDirectoryPath);
                    Debug.Log($"已创建目录: {fullDirectoryPath}");
                }

                // 创建 ScriptableObject 实例
                T asset = ScriptableObject.CreateInstance<T>();


                // 构建完整路径并确保唯一性
                string assetPath = Path.Combine("Assets", targetPath, fileName + ".asset");
                string uniquePath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

                // 保存资源
                AssetDatabase.CreateAsset(asset, uniquePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log($"成功创建 {typeof(T)} 位于: {uniquePath}");
                return asset;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"创建 ScriptableObject 失败: {ex.Message}");
                return null;
            }
#else
        Debug.LogError("CreateScriptableObject 仅在 Unity 编辑器中可用");
        return null;
#endif
        }

#if ODIN_INSPECTOR
        /// <summary>
        /// 获取所有可用的 ScriptableObject 类型（用于 Odin Inspector 下拉菜单）
        /// </summary>
        /// <returns>ScriptableObject 类型列表</returns>
        public static IEnumerable<ValueDropdownItem<Type>> GetAllScriptableObjectTypes()
        {
            var items = new List<ValueDropdownItem<Type>>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (System.Reflection.ReflectionTypeLoadException)
                {
                    // 忽略无法加载类型的程序集
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
            return items;
        }
#endif

        /// <summary>
        /// 查找指定类型的所有 ScriptableObject 资源
        /// </summary>
        /// <param name="type">要查找的 ScriptableObject 类型</param>
        /// <returns>找到的资源列表</returns>
        public static List<UnityEngine.Object> FindScriptableObjects(Type type)
        {
            if (type == null || !typeof(ScriptableObject).IsAssignableFrom(type))
            {
                Debug.LogError("提供的类型无效。类型必须是 ScriptableObject 的派生类。");
                return new List<UnityEngine.Object>();
            }

#if UNITY_EDITOR
            try
            {
                return AssetDatabase.FindAssets($"t:{type.Name}")
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .Select(path => AssetDatabase.LoadAssetAtPath(path, type))
                    .Where(asset => asset != null)
                    .ToList();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"查找类型 {type.Name} 的 ScriptableObjects 时出错: {ex.Message}");
                return new List<UnityEngine.Object>();
            }
#else
        try
        {
            return Resources.LoadAll("", type).ToList();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"加载类型 {type.Name} 的 ScriptableObjects 时出错: {ex.Message}");
            return new List<UnityEngine.Object>();
        }
#endif
        }

        /// <summary>
        /// 查找指定类型的所有 ScriptableObject 资源
        /// 示例：List<MyScriptableObject> allSOs = FindAllScriptableObjects<MyScriptableObject>();
        /// </summary>
        /// <typeparam name="T">ScriptableObject 类型</typeparam>
        /// <returns>找到的资源列表</returns>
        public static List<T> FindAllScriptableObjects<T>() where T : ScriptableObject
        {
#if UNITY_EDITOR
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
                Debug.LogError($"查找类型 {typeof(T).Name} 的 ScriptableObjects 时出错: {ex.Message}");
                return new List<T>();
            }
#else
        try
        {
            return Resources.LoadAll<T>("").ToList();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"加载类型 {typeof(T).Name} 的 ScriptableObjects 时出错: {ex.Message}");
            return new List<T>();
        }
#endif
        }

        /// <summary>
        /// 根据名称查找指定的 ScriptableObject 资源
        /// </summary>
        /// <typeparam name="T">ScriptableObject 类型</typeparam>
        /// <param name="name">资源名称</param>
        /// <returns>找到的资源，如果未找到则返回 null</returns>
        public static T FindScriptableObject<T>(string name) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("资源名称不能为空");
                return null;
            }

            return FindAllScriptableObjects<T>()
                .Where(asset => asset != null && asset.name == name)
                .FirstOrDefault();
        }

        /// <summary>
        /// 查找指定类型的第一个 ScriptableObject 资源
        /// </summary>
        /// <typeparam name="T">ScriptableObject 类型</typeparam>
        /// <returns>找到的资源，如果未找到则返回 null</returns>
        public static T FindScriptableObject<T>() where T : ScriptableObject
        {
            return FindAllScriptableObjects<T>().FirstOrDefault();
        }
    }
}