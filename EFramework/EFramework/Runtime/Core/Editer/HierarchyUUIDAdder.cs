
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;
    using System;
namespace EFramework.Unity
{
    public static class HierarchyUUIDAdder
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/Add UUID Suffix", false, 0)]
        static void AddUUIDSuffix()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                AddUUIDSuffixToObject(obj);
            }

            Debug.Log($"已为 {Selection.gameObjects.Length} 个物体添加UUID后缀");
        }

        [MenuItem("GameObject/Add UUID Suffix", true)]
        static bool ValidateAddUUIDSuffix()
        {
            return Selection.gameObjects.Length > 0;
        }

        [MenuItem("GameObject/Remove UUID Suffix", false, 1)]
        static void RemoveUUIDSuffix()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                RemoveUUIDSuffixFromObject(obj);
            }

            Debug.Log($"已从 {Selection.gameObjects.Length} 个物体移除UUID后缀");
        }

        [MenuItem("GameObject/Remove UUID Suffix", true)]
        static bool ValidateRemoveUUIDSuffix()
        {
            return Selection.gameObjects.Length > 0;
        }

        static void AddUUIDSuffixToObject(GameObject obj)
        {
            string originalName = obj.name;

            // 如果已经有UUID后缀，先移除
            if (originalName.Contains("_UUID"))
            {
                return;
            }

            // 生成短UUID（8位）
            string shortUUID = UUID.New();

            // 添加后缀
            obj.name = $"{originalName}_UUID{shortUUID}";

            // 标记为脏，确保保存修改
            EditorUtility.SetDirty(obj);
        }

        static void RemoveUUIDSuffixFromObject(GameObject obj)
        {
            string currentName = obj.name;

            if (currentName.Contains("_UUID"))
            {
                int uuidIndex = currentName.IndexOf("_UUID");
                string originalName = currentName.Substring(0, uuidIndex);
                obj.name = originalName;

                // 标记为脏，确保保存修改
                EditorUtility.SetDirty(obj);
            }
        }
        #endif
    }

}
