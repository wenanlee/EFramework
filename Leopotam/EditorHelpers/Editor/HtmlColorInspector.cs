// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace EFramework.EditorHelpers.UnityEditors {
    [CustomPropertyDrawer (typeof (HtmlColorAttribute))]
    sealed class HtmlColorAttributeInspector : PropertyDrawer {
        const int HtmlLineWidth = 280;
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            var htmlRect = position;
            htmlRect.width = HtmlLineWidth;
            var colorRect = position;
            colorRect.x += HtmlLineWidth;
            colorRect.width = position.width - HtmlLineWidth;
            var htmlValue = EditorGUI.TextField (htmlRect, label, "#" + ColorUtility.ToHtmlStringRGBA (property.colorValue));
            Color color;
            if (ColorUtility.TryParseHtmlString (htmlValue, out color)) {
                property.colorValue = color;
            }
            property.colorValue = EditorGUI.ColorField (colorRect, property.colorValue);
        }
    }
}