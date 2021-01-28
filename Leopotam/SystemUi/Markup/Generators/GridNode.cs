// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Globalization;
using EFramework.Math;
using EFramework.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Markup.Generators {
    static class GridNode {
        static readonly int HashedFlip = "flip".GetStableHashCode ();

        static readonly int HashedCellSize = "cellSize".GetStableHashCode ();

        /// <summary>
        /// Create "grid" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "grid";
#endif
            var grid = widget.gameObject.AddComponent<GridLayoutGroup> ();

            grid.childAlignment = TextAnchor.MiddleCenter;
            var flipX = false;
            var flipY = false;
            var cellSize = Vector2.zero;

            var attrValue = node.GetAttribute (HashedFlip);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = MarkupUtils.SplitAttrValue (attrValue);
                flipX = parts.Length > 0 && string.CompareOrdinal (parts[0], "true") == 0;
                flipY = parts.Length > 1 && string.CompareOrdinal (parts[1], "true") == 0;
            }

            float amount;
            attrValue = node.GetAttribute (HashedCellSize);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = MarkupUtils.SplitAttrValue (attrValue);
                if (parts.Length > 0 && !string.IsNullOrEmpty (parts[0])) {
                    if (float.TryParse (parts[0], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out amount)) {
                        cellSize.x = amount;
                    }
                }
                if (parts.Length > 1 && !string.IsNullOrEmpty (parts[1])) {
                    if (float.TryParse (parts[1], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out amount)) {
                        cellSize.y = amount;
                    }
                }
            }

            grid.cellSize = cellSize;
            grid.startCorner = (GridLayoutGroup.Corner) ((flipX ? 1 : 0) | (flipY ? 2 : 0));

            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);

            return widget;
        }
    }
}