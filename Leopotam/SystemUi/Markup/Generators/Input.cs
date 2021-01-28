// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Math;
using EFramework.Serialization;
using EFramework.SystemUi.Actions;
using EFramework.SystemUi.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Markup.Generators {
    static class InputNode {
        static readonly int HashedFontName = "fontName".GetStableHashCode ();

        static readonly int HashedFontSize = "fontSize".GetStableHashCode ();

        static readonly int HashedFontStyle = "fontStyle".GetStableHashCode ();

        static readonly int HashedAlign = "align".GetStableHashCode ();

        static readonly int HashedLocalize = "localize".GetStableHashCode ();

        static readonly int HashedOnChange = "onChange".GetStableHashCode ();

        /// <summary>
        /// Create "input" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "input";
#endif
            var input = widget.gameObject.AddComponent<InputField> ();
            string attrValue;
            string font = null;
            var align = TextAnchor.MiddleLeft;
            var style = FontStyle.Normal;
            var size = 24;
            var isInteractive = false;

            var theme = MarkupUtils.GetTheme (node, container);

            var img = widget.gameObject.AddComponent<Image> ();
            img.type = Image.Type.Sliced;
            img.sprite = theme.GetInputSprite ();
            img.color = theme.GetInputColor (MarkupTheme.InputState.Background);

            var ph = MarkupUtils.CreateUiObject (null, widget);
            var phText = ph.gameObject.AddComponent<Text> ();
            phText.fontStyle = FontStyle.Italic;

            var txt = MarkupUtils.CreateUiObject (null, widget);
            var inText = txt.gameObject.AddComponent<Text> ();
            inText.supportRichText = false;

            attrValue = node.GetAttribute (HashedFontName);
            if (!string.IsNullOrEmpty (attrValue)) {
                font = attrValue;
            }

            attrValue = node.GetAttribute (HashedFontSize);
            if (!string.IsNullOrEmpty (attrValue)) {
                int.TryParse (attrValue, out size);
            }

            attrValue = node.GetAttribute (HashedFontStyle);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = MarkupUtils.SplitAttrValue (attrValue);
                for (var i = 0; i < parts.Length; i++) {
                    switch (parts[i]) {
                        case "bold":
                            style |= FontStyle.Bold;
                            break;
                        case "italic":
                            style |= FontStyle.Italic;
                            break;
                    }
                }
            }

            attrValue = node.GetAttribute (HashedAlign);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = MarkupUtils.SplitAttrValue (attrValue);
                var alignHor = 1;
                var alignVer = 3;
                for (var i = 0; i < parts.Length; i++) {
                    switch (parts[i]) {
                        case "left":
                            alignHor = 0;
                            break;
                        case "right":
                            alignHor = 2;
                            break;
                        case "top":
                            alignVer = 0;
                            break;
                        case "bottom":
                            alignVer = 6;
                            break;
                    }
                }
                align = (TextAnchor) (alignHor + alignVer);
            }

            attrValue = node.GetAttribute (HashedLocalize);
            if (!string.IsNullOrEmpty (attrValue)) {
                phText.gameObject.AddComponent<TextLocalization> ().SetToken (attrValue);
            } else {
                phText.text = node.Value;
            }

            var margin = theme.GetInputMargin ();
            var marginMin = new Vector2 (margin, margin);
            var marginMax = -marginMin;

            ph.anchorMin = Vector2.zero;
            ph.anchorMax = Vector2.one;
            ph.offsetMin = marginMin;
            ph.offsetMax = marginMax;

            txt.anchorMin = Vector2.zero;
            txt.anchorMax = Vector2.one;
            txt.offsetMin = marginMin;
            txt.offsetMax = marginMax;

            phText.raycastTarget = false;
            inText.raycastTarget = false;

            phText.alignment = align;
            inText.alignment = align;

            phText.font = container.GetFont (font);
            inText.font = phText.font;

            phText.color = theme.GetInputColor (MarkupTheme.InputState.Placeholder);
            if (!MarkupUtils.SetColor (inText, node)) {
                inText.color = Color.black;
            }

            phText.fontStyle = theme.GetInputPlaceholderStyle ();
            inText.fontStyle = style;

            phText.fontSize = size;
            inText.fontSize = size;

            input.targetGraphic = img;
            input.transition = Selectable.Transition.None;
            input.placeholder = phText;
            input.textComponent = inText;
            input.selectionColor = theme.GetInputColor (MarkupTheme.InputState.Selection);

            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);
            MarkupUtils.SetNav (input, node, container.UseNavigation);

            attrValue = node.GetAttribute (HashedOnChange);
            if (!string.IsNullOrEmpty (attrValue)) {
                widget.gameObject.AddComponent<UiInputAction> ().SetGroup (attrValue);
                isInteractive = true;
            }
            input.interactable = isInteractive;

            return widget;
        }
    }
}