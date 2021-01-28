// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Globalization;
using EFramework.Math;
using EFramework.Serialization;
using EFramework.SystemUi.Layouts;
using UnityEngine;

namespace EFramework.SystemUi.Markup.Generators {
    static class StackNode {
        static readonly int HashedPadding = "padding".GetStableHashCode ();

        static readonly int HashedAlign = "align".GetStableHashCode ();

        static readonly int HashedVertical = "vertical".GetStableHashCode ();

        static readonly int HashedReverse = "reverse".GetStableHashCode ();

        /// <summary>
        /// Create "stack" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "stack";
#endif
            string attrValue;
            float amount;
            var align = OneAxisAlignment.Center;
            var padding = 0f;

            var stack = widget.gameObject.AddComponent<StackLayout> ();

            attrValue = node.GetAttribute (HashedAlign);
            if (!string.IsNullOrEmpty (attrValue)) {
                switch (attrValue) {
                    case "start":
                        align = OneAxisAlignment.Start;
                        break;
                    case "end":
                        align = OneAxisAlignment.End;
                        break;
                }
            }

            attrValue = node.GetAttribute (HashedPadding);
            if (!string.IsNullOrEmpty (attrValue)) {
                if (float.TryParse (attrValue, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out amount)) {
                    padding = amount;
                }
            }

            attrValue = node.GetAttribute (HashedVertical);
            var isVertical = string.CompareOrdinal (attrValue, "true") == 0;

            attrValue = node.GetAttribute (HashedReverse);
            var isReverse = string.CompareOrdinal (attrValue, "true") == 0;

            stack.Padding = padding;
            stack.ChildAlignment = align;
            stack.IsVertical = isVertical;
            stack.IsReverse = isReverse;

            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);

            return widget;
        }
    }
}