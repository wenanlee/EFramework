using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(NaInfoBoxAttribute))]
    public class InfoBoxDecoratorDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return GetHelpBoxHeight();
        }

        public override void OnGUI(Rect rect)
        {
            NaInfoBoxAttribute infoBoxAttribute = (NaInfoBoxAttribute)attribute;

            float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
            Rect infoBoxRect = new Rect(
                rect.x + indentLength,
                rect.y,
                rect.width - indentLength,
                GetHelpBoxHeight());

            DrawInfoBox(infoBoxRect, infoBoxAttribute.Text, infoBoxAttribute.Type);
        }

        private float GetHelpBoxHeight()
        {
            NaInfoBoxAttribute infoBoxAttribute = (NaInfoBoxAttribute)attribute;
            float minHeight = EditorGUIUtility.singleLineHeight * 2.0f;
            float desiredHeight = GUI.skin.box.CalcHeight(new GUIContent(infoBoxAttribute.Text), EditorGUIUtility.currentViewWidth);
            float height = Mathf.Max(minHeight, desiredHeight);

            return height;
        }

        private void DrawInfoBox(Rect rect, string infoText, ENaInfoBoxType infoBoxType)
        {
            MessageType messageType = MessageType.None;
            switch (infoBoxType)
            {
                case ENaInfoBoxType.Normal:
                    messageType = MessageType.Info;
                    break;

                case ENaInfoBoxType.Warning:
                    messageType = MessageType.Warning;
                    break;

                case ENaInfoBoxType.Error:
                    messageType = MessageType.Error;
                    break;
            }

            NaughtyEditorGUI.HelpBox(rect, infoText, messageType);
        }
    }
}
