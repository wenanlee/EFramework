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
    static class FixedTableNode {
        static readonly int HashedItemsInRow = "itemsInRow".GetStableHashCode ();

        static readonly int HashedCellSize = "cellSize".GetStableHashCode ();

        /// <summary>
        /// Create "fixedTable" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "fixedTable";
#endif
            var table = widget.gameObject.AddComponent<FixedTableLayout> ();
            var itemsInRow = 1;
            var cellSize = Vector2.zero;

            var attrValue = node.GetAttribute (HashedItemsInRow);
            if (!string.IsNullOrEmpty (attrValue)) {
                int.TryParse (attrValue, out itemsInRow);
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

            table.ItemsInRow = itemsInRow;
            table.CellSize = cellSize;

            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);

            return widget;
        }
    }
}