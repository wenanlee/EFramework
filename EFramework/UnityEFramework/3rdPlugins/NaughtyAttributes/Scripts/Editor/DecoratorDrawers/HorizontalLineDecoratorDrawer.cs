using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(NaHorizontalLineAttribute))]
    public class HorizontalLineDecoratorDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            NaHorizontalLineAttribute lineAttr = (NaHorizontalLineAttribute)attribute;
            return EditorGUIUtility.singleLineHeight + lineAttr.Height;
        }

        public override void OnGUI(Rect position)
        {
            Rect rect = EditorGUI.IndentedRect(position);
            rect.y += EditorGUIUtility.singleLineHeight / 3.0f;
            NaHorizontalLineAttribute lineAttr = (NaHorizontalLineAttribute)attribute;
            NaughtyEditorGUI.HorizontalLine(rect, lineAttr.Height, lineAttr.Color.GetColor());
        }
    }
}
