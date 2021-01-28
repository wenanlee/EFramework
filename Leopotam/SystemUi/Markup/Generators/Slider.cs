// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Globalization;
using EFramework.Math;
using EFramework.Serialization;
using EFramework.SystemUi.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Markup.Generators {
    static class SliderNode {
        static readonly int HashedHandle = "handle".GetStableHashCode ();

        static readonly int HashedRtl = "rtl".GetStableHashCode ();

        static readonly int HashedRange = "range".GetStableHashCode ();

        static readonly int HashedValue = "value".GetStableHashCode ();

        static readonly int HashedOnChange = "onChange".GetStableHashCode ();

        public const string BackgroundImageName = "background";

        public const string ForegroundImageName = "foreground";

        public const string HandleImageName = "handle";

        /// <summary>
        /// Create "slider" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "slider";
#endif
            var slider = widget.gameObject.AddComponent<Slider> ();
            var theme = MarkupUtils.GetTheme (node, container);
            Image img;
            RectTransform rt;

            var direction = Slider.Direction.LeftToRight;
            var minValue = 0f;
            var maxValue = 1f;
            var useInts = false;
            var dataValue = 0f;
            var isInteractive = false;

            // background.
            rt = MarkupUtils.CreateUiObject (BackgroundImageName, widget);
            img = rt.gameObject.AddComponent<Image> ();
            img.sprite = theme.GetSliderSprite (MarkupTheme.SliderState.Background);
            img.color = theme.GetSliderColor (MarkupTheme.SliderState.Background);
            img.type = Image.Type.Sliced;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;

            // foreground.
            rt = MarkupUtils.CreateUiObject (ForegroundImageName, widget);
            img = rt.gameObject.AddComponent<Image> ();
            img.sprite = theme.GetSliderSprite (MarkupTheme.SliderState.Foreground);
            img.color = theme.GetSliderColor (MarkupTheme.SliderState.Foreground);
            img.type = Image.Type.Sliced;
            img.raycastTarget = false;
            rt.sizeDelta = Vector2.zero;
            slider.fillRect = rt;

            string attrValue;
            attrValue = node.GetAttribute (HashedHandle);
            var useHandle = string.CompareOrdinal (attrValue, "true") == 0;
            if (useHandle) {
                var handle = MarkupUtils.CreateUiObject (null, widget);
                slider.handleRect = handle;

                rt = MarkupUtils.CreateUiObject (HandleImageName, handle);
                img = rt.gameObject.AddComponent<Image> ();
                img.raycastTarget = false;
                img.sprite = theme.GetSliderSprite (MarkupTheme.SliderState.Handle);
                img.color = theme.GetSliderColor (MarkupTheme.SliderState.Handle);
                img.type = Image.Type.Sliced;
                img.SetNativeSize ();
                handle.sizeDelta = img.rectTransform.sizeDelta;
            }

            attrValue = node.GetAttribute (HashedRtl);
            if (string.CompareOrdinal (attrValue, "true") == 0) {
                direction = Slider.Direction.RightToLeft;
            }

            float amount;
            attrValue = node.GetAttribute (HashedRange);
            if (!string.IsNullOrEmpty (attrValue)) {
                var parts = MarkupUtils.SplitAttrValue (attrValue);
                if (parts.Length > 0 && !string.IsNullOrEmpty (parts[0])) {
                    if (float.TryParse (parts[0], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out amount)) {
                        minValue = amount;
                    }
                }
                if (parts.Length > 1 && !string.IsNullOrEmpty (parts[1])) {
                    if (float.TryParse (parts[1], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out amount)) {
                        maxValue = amount;
                    }
                }
                if (parts.Length > 2 && string.CompareOrdinal (parts[2], "true") == 0) {
                    useInts = true;
                }
            }

            attrValue = node.GetAttribute (HashedValue);
            if (!string.IsNullOrEmpty (attrValue)) {
                if (float.TryParse (attrValue, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out amount)) {
                    dataValue = amount;
                }
            }

            attrValue = node.GetAttribute (HashedOnChange);
            if (!string.IsNullOrEmpty (attrValue)) {
                widget.gameObject.AddComponent<UiSliderAction> ().SetGroup (attrValue);
                isInteractive = true;
            }

            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = useInts;
            slider.value = dataValue;
            slider.direction = direction;

            slider.transition = Selectable.Transition.None;

            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);
            MarkupUtils.SetNav (slider, node, container.UseNavigation);

            slider.interactable = useHandle && isInteractive;

            return widget;
        }
    }
}