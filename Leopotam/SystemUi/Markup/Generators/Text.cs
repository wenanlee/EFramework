// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Math;
using EFramework.Serialization;
using EFramework.SystemUi.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Markup.Generators {
    static class TextNode {
        static readonly int HashedFontName = "fontName".GetStableHashCode ();

        static readonly int HashedFontSize = "fontSize".GetStableHashCode ();

        static readonly int HashedFontStyle = "fontStyle".GetStableHashCode ();

        static readonly int HashedAlign = "align".GetStableHashCode ();

        static readonly int HashedLocalize = "localize".GetStableHashCode ();

        /// <summary>
        /// Create "text" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "text";
#endif
            var txt = widget.gameObject.AddComponent<Text> ();
            string attrValue;
            string font = null;
            var align = TextAnchor.MiddleCenter;
            var style = FontStyle.Normal;
            var size = 24;

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
                widget.gameObject.AddComponent<TextLocalization> ().SetToken (attrValue);
            } else {
                txt.text = node.Value;
            }

            txt.alignment = align;
            txt.font = container.GetFont (font);
            txt.fontStyle = style;
            txt.fontSize = size;

            if (!MarkupUtils.SetColor (txt, node)) {
                txt.color = Color.black;
            }
            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);
            txt.raycastTarget = MarkupUtils.ValidateInteractive (widget, node, container.DragTreshold);

            return widget;
        }
    }
}