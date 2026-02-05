#if UNITY_EDITOR
using EFramework.Unity.Entity;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class HierarchyRename
{
    static HierarchyRename()
    {
        EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyItem;
    }

    static void DrawHierarchyItem(int instanceID, Rect rect)
    {
        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go == null) return;
        if (go.TryGetComponent(out GameEntity entityObject))
        {
            if (entityObject == null) return;
            if (entityObject.ComponentsVolume == null) return;
            if (string.IsNullOrEmpty(entityObject.ComponentsVolume.Desc)) return;

            string displayName = $"{entityObject.ComponentsVolume.Desc} <{entityObject.ComponentsVolume.Uuid}> ({entityObject.name})";
            bool isSelected = Selection.Contains(go);
            Color textColor = isSelected ? Color.white : EditorStyles.label.normal.textColor;
            if (entityObject.gameObject.activeSelf == false)
            {
                textColor = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            Color bgColor = isSelected ? new Color(0.243f, 0.490f, 0.905f, 1f) : new Color(0.22f, 0.22f, 0.22f, 1f);
            EditorGUI.DrawRect(rect, bgColor);
            EditorGUI.LabelField(rect, displayName,
                new GUIStyle()
                {
                    normal = { textColor = textColor },
                    fontStyle = FontStyle.Bold
                }
            );

        }
    }
}
#endif