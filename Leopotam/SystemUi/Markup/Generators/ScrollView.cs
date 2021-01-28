// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Globalization;
using EFramework.Math;
using EFramework.Serialization;
using EFramework.SystemUi.Actions;
using EFramework.SystemUi.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Markup.Generators {
    static class ScrollViewNode {
        public const string ViewportName = "viewport";

        public const string ContentName = "content";

        static readonly int HashedContentSize = "contentSize".GetStableHashCode ();

        static readonly int HashedBar = "bar".GetStableHashCode ();

        static readonly int HashedDrag = "drag".GetStableHashCode ();

        static readonly int HashedClamp = "clamp".GetStableHashCode ();

        static readonly int HashedOnChange = "onChange".GetStableHashCode ();

        /// <summary>
        /// Create "scrollView" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "scrollView";
#endif
            float amount;
            string attrValue;
            Scrollbar horScroll = null;
            Scrollbar verScroll = null;

            var anchorMin = Vector2.zero;
            var anchorMax = Vector2.one;
            var offsetMin = Vector3.zero;
            var offsetMax = Vector3.zero;
            var needHorScroll = true;
            var needVerScroll = true;

            var theme = MarkupUtils.GetTheme (node, container);

            var scrollView = widget.gameObject.AddComponent<ScrollRect> ();

            // viewport.
            var viewport = MarkupUtils.CreateUiObject (ViewportName, widget);
            viewport.gameObject.AddComponent<RectMask2D> ();
            viewport.gameObject.AddComponent<NonVisualWidget> ().raycastTarget = false;
            viewport.anchorMin = Vector2.zero;
            viewport.anchorMax = Vector2.one;
            viewport.sizeDelta = Vector2.zero;
            viewport.pivot = Vector2.up;

            // content.
            var content = MarkupUtils.CreateUiObject (ContentName, viewport);
            attrValue = node.GetAttribute (HashedContentSize);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = MarkupUtils.SplitAttrValue (attrValue);
                if (parts.Length > 0 && !string.IsNullOrEmpty (parts[0])) {
                    if (float.TryParse (parts[0], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out amount)) {
                        amount *= 0.5f;
                        offsetMin.x = -amount;
                        offsetMax.x = amount;
                        anchorMin.x = 0.5f;
                        anchorMax.x = 0.5f;
                    }
                }
                if (parts.Length > 1 && !string.IsNullOrEmpty (parts[1])) {
                    if (float.TryParse (parts[1], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out amount)) {
                        amount *= 0.5f;
                        offsetMin.y = -amount;
                        offsetMax.y = amount;
                        anchorMin.y = 0.5f;
                        anchorMax.y = 0.5f;
                    }
                }
            }
            content.anchorMin = anchorMin;
            content.anchorMax = anchorMax;
            content.offsetMin = offsetMin;
            content.offsetMax = offsetMax;

            attrValue = node.GetAttribute (HashedClamp);
            if (string.CompareOrdinal (attrValue, "true") == 0) {
                scrollView.movementType = ScrollRect.MovementType.Clamped;
            }

            attrValue = node.GetAttribute (HashedDrag);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = MarkupUtils.SplitAttrValue (attrValue);
                if (parts.Length > 0 && string.CompareOrdinal (parts[0], "false") == 0) {
                    scrollView.horizontal = false;
                }
                if (parts.Length > 1 && string.CompareOrdinal (parts[1], "false") == 0) {
                    scrollView.vertical = false;
                }
            }

            attrValue = node.GetAttribute (HashedBar);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = MarkupUtils.SplitAttrValue (attrValue);
                if (parts.Length > 0 && string.CompareOrdinal (parts[0], "false") == 0) {
                    needHorScroll = false;
                }
                if (parts.Length > 1 && string.CompareOrdinal (parts[1], "false") == 0) {
                    needVerScroll = false;
                }
            }

            if (needHorScroll) {
                horScroll = CreateScrollbar (widget, theme, true);
                MarkupUtils.SetNav (horScroll, node, container.UseNavigation);
            }
            if (needVerScroll) {
                verScroll = CreateScrollbar (widget, theme, false);
                MarkupUtils.SetNav (verScroll, node, container.UseNavigation);
            }

            attrValue = node.GetAttribute (HashedOnChange);
            if (!string.IsNullOrEmpty (attrValue)) {
                widget.gameObject.AddComponent<NonVisualWidget> ();
                widget.gameObject.AddComponent<UiScrollViewAction> ().SetGroup (attrValue);
            }

            scrollView.content = content;
            scrollView.viewport = viewport;
            scrollView.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollView.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollView.horizontalScrollbar = horScroll;
            scrollView.verticalScrollbar = verScroll;
            scrollView.decelerationRate = 0.01f;

            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);

            scrollView.normalizedPosition = Vector2.up;

            return content;
        }

        static Scrollbar CreateScrollbar (RectTransform widget, MarkupTheme theme, bool isHorizontal) {
            Image img;
            var width = theme.GetScrollbarWidth ();

            // background.
            var rt = MarkupUtils.CreateUiObject (null, widget);
            var sb = rt.gameObject.AddComponent<Scrollbar> ();
            img = rt.gameObject.AddComponent<Image> ();
            img.sprite = theme.GetScrollbarSprite (MarkupTheme.ScrollbarState.Background);
            img.color = theme.GetScrollbarColor (MarkupTheme.ScrollbarState.Background);
            img.type = Image.Type.Sliced;
            if (isHorizontal) {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.right;
                rt.pivot = Vector2.zero;
                rt.sizeDelta = new Vector2 (0f, width);
            } else {
                rt.anchorMin = Vector2.right;
                rt.anchorMax = Vector2.one;
                rt.pivot = Vector2.one;
                rt.sizeDelta = new Vector2 (width, 0f);
            }

            // handle.
            rt = MarkupUtils.CreateUiObject (null, rt);
            img = rt.gameObject.AddComponent<Image> ();
            img.sprite = theme.GetScrollbarSprite (MarkupTheme.ScrollbarState.Handle);
            img.color = theme.GetScrollbarColor (MarkupTheme.ScrollbarState.Handle);
            img.type = Image.Type.Sliced;
            img.raycastTarget = false;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            sb.direction = isHorizontal ? Scrollbar.Direction.LeftToRight : Scrollbar.Direction.BottomToTop;

            sb.handleRect = rt;
            sb.transition = Selectable.Transition.None;

            return sb;
        }
    }
}