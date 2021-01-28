// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Math;
using EFramework.Serialization;
using UnityEngine;

namespace EFramework.SystemUi.Markup.Generators {
    static class AlignNode {
        static readonly int HashedSide = "side".GetStableHashCode ();

        /// <summary>
        /// Create "align" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "align";
#endif
            var rt = widget.GetComponent<RectTransform> ();
            var deltaSize = Vector2.zero;
            var anchorMin = Vector2.zero;
            var anchorMax = Vector2.one;
            var attrValue = node.GetAttribute (HashedSide);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = MarkupUtils.SplitAttrValue (attrValue);
                for (var i = 0; i < parts.Length; i++) {
                    switch (parts[i]) {
                        case "left":
                            anchorMax.x = 0f;
                            deltaSize.x = 1f;
                            break;
                        case "right":
                            anchorMin.x = 1f;
                            deltaSize.x = 1f;
                            break;
                        case "top":
                            anchorMin.y = 1f;
                            deltaSize.y = 1f;
                            break;
                        case "bottom":
                            anchorMax.y = 0f;
                            deltaSize.y = 1f;
                            break;
                    }
                }
            }

            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.sizeDelta = deltaSize;

            MarkupUtils.SetHidden (widget, node);

            return widget;
        }
    }
}