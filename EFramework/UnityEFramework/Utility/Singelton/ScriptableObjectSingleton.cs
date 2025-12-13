using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace EFramework.Unity.Utility
{

    /// <summary>
    /// 编辑器专用单例模式基类
    /// 简洁实现，只保留核心功能
    /// </summary>
    public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                {

                    // 查找现有实例
                    string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

                    if (guids.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        _instance = AssetDatabase.LoadAssetAtPath<T>(path);
                    }

                    // 创建新实例
                    if (_instance == null)
                    {
                        _instance = CreateInstance<T>();

                        string path = $"Assets/Editor/{typeof(T).Name}.asset";
                        AssetDatabase.CreateAsset(_instance, path);
                        AssetDatabase.SaveAssets();
                    }
                }
                return _instance;
#else
                return null;
#endif
            }
        }
#if UNITY_EDITOR

        /// <summary>
        /// 保存数据
        /// </summary>
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
