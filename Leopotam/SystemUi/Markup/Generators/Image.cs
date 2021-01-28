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
    static class ImageNode {
        static readonly int HashedPath = "path".GetStableHashCode ();

        static readonly int HashedRaw = "raw".GetStableHashCode ();

        static readonly int HashedNativeSize = "nativeSize".GetStableHashCode ();

        static readonly int HashedMask = "mask".GetStableHashCode ();

        static readonly int HashedFillCenter = "fillCenter".GetStableHashCode ();

        /// <summary>
        /// Create "image" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "image";
#endif
            Image img = null;
            RawImage tex = null;
            string attrValue;
            var useImg = true;
            var ignoreSize = false;
            var fillCenter = true;

            attrValue = node.GetAttribute (HashedRaw);
            if (string.CompareOrdinal (attrValue, "true") == 0) {
                useImg = false;
                tex = widget.gameObject.AddComponent<RawImage> ();
            } else {
                img = widget.gameObject.AddComponent<Image> ();
                img.type = Image.Type.Sliced;
            }

            attrValue = node.GetAttribute (HashedPath);
            if (!string.IsNullOrEmpty (attrValue)) {
                if (useImg) {
                    // Image.
                    var parts = MarkupUtils.SplitAttrValue (attrValue);
                    if (parts.Length == 2) {
                        var atlas = container.GetAtlas (parts[0]);
                        if ((object) atlas != null) {
                            img.sprite = atlas.Get (parts[1]);
                        }
                    }
                } else {
                    // RawImage.
                    tex.texture = Resources.Load<Texture2D> (attrValue);
                }
            }

            if (useImg) {
                attrValue = node.GetAttribute (HashedNativeSize);
                ignoreSize = (object) img.sprite != null && string.CompareOrdinal (attrValue, "true") == 0;
            }

            if (useImg && ignoreSize) {
                img.SetNativeSize ();
            } else {
                MarkupUtils.SetSize (widget, node);
            }

            attrValue = node.GetAttribute (HashedMask);
            if (string.CompareOrdinal (attrValue, "true") == 0) {
                widget.gameObject.AddComponent<Mask> ();
            }

            if (useImg) {
                attrValue = node.GetAttribute (HashedFillCenter);
                if (string.CompareOrdinal (attrValue, "false") == 0) {
                    fillCenter = false;
                }
                img.fillCenter = fillCenter;
            }

            if (useImg && !MarkupUtils.SetColor (img, node)) {
                img.color = Color.white;
            }
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);
            var isInteractive = MarkupUtils.ValidateInteractive (widget, node, container.DragTreshold);
            if (useImg) {
                img.raycastTarget = isInteractive;
            } else {
                tex.raycastTarget = isInteractive;
            }

            return widget;
        }
    }
}