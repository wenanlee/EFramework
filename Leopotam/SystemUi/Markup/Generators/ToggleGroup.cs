// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Math;
using EFramework.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Markup.Generators {
    static class ToggleGroupNode {
        static readonly int HashedEmptyCheck = "emptyCheck".GetStableHashCode ();

        /// <summary>
        /// Create "toggleGroup" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "toggleGroup";
#endif
            var checkGroup = widget.gameObject.AddComponent<ToggleGroup> ();

            var attrValue = node.GetAttribute (HashedEmptyCheck);
            if (string.CompareOrdinal (attrValue, "true") == 0) {
                checkGroup.allowSwitchOff = true;
            }

            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);

            return widget;
        }
    }
}