// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Math;
using EFramework.Serialization;
using EFramework.SystemUi.Actions;
using EFramework.SystemUi.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Markup.Generators {
    static class ToggleNode {
        public const string BackgroundImageName = "background";

        public const string ForegroundImageName = "foreground";

        public const string ContentName = "content";

        static readonly int HashedGroup = "group".GetStableHashCode ();

        static readonly int HashedCheck = "check".GetStableHashCode ();

        static readonly int HashedOnChange = "onChange".GetStableHashCode ();

        /// <summary>
        /// Create "toggle" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "toggle";
#endif
            var tr = widget.transform;
            Image img;
            RectTransform rt;
            Vector2 size;
            string attrValue;

            var toggle = widget.gameObject.AddComponent<Toggle> ();
            var theme = MarkupUtils.GetTheme (node, container);

            var isInteractive = false;

            // background.
            rt = MarkupUtils.CreateUiObject (BackgroundImageName, tr);
            img = rt.gameObject.AddComponent<Image> ();
            img.sprite = theme.GetToggleSprite (MarkupTheme.ToggleState.Background);
            img.type = Image.Type.Sliced;
            img.raycastTarget = false;
            size = theme.GetToggleSize (MarkupTheme.ToggleState.Background);
            rt.anchorMin = new Vector2 (0f, 0.5f);
            rt.anchorMax = new Vector2 (0f, 0.5f);
            rt.offsetMin = new Vector2 (0f, -size.y * 0.5f);
            rt.offsetMax = new Vector2 (size.x, size.y * 0.5f);
            toggle.targetGraphic = img;

            // foreground.
            rt = MarkupUtils.CreateUiObject (ForegroundImageName, rt);
            img = rt.gameObject.AddComponent<Image> ();
            rt.anchorMin = new Vector2 (0.5f, 0.5f);
            rt.anchorMax = new Vector2 (0.5f, 0.5f);
            rt.sizeDelta = theme.GetToggleSize (MarkupTheme.ToggleState.Foreground);
            img.sprite = theme.GetToggleSprite (MarkupTheme.ToggleState.Foreground);
            img.type = Image.Type.Sliced;
            img.raycastTarget = false;
            toggle.graphic = img;

            // content.
            rt = MarkupUtils.CreateUiObject (ContentName, tr);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.right * size.x;
            rt.offsetMax = Vector2.zero;

            attrValue = node.GetAttribute (HashedGroup);
            if (!string.IsNullOrEmpty (attrValue)) {
                var groupGo = container.GetNamedNode (attrValue);
                if ((object) groupGo != null) {
                    toggle.group = groupGo.GetComponent<ToggleGroup> ();
                }
            }

            attrValue = node.GetAttribute (HashedCheck);
            if (string.CompareOrdinal (attrValue, "true") == 0) {
                toggle.isOn = true;
            }

            attrValue = node.GetAttribute (HashedOnChange);
            if (!string.IsNullOrEmpty (attrValue)) {
                widget.gameObject.AddComponent<NonVisualWidget> ();
                widget.gameObject.AddComponent<UiToggleAction> ().SetGroup (attrValue);
                isInteractive = true;
            }

            toggle.transition = Selectable.Transition.None;
            toggle.interactable = isInteractive;

            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);
            MarkupUtils.SetNav (toggle, node, container.UseNavigation);

            return rt;
        }
    }
}