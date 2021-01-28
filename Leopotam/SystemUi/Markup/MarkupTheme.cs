// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.EditorHelpers;
using UnityEngine;

namespace EFramework.SystemUi.Markup {
    public sealed class MarkupTheme : ScriptableObject {
        [SerializeField]
        string _name = "NewMarkupTheme";

        [SerializeField]
        Sprite _buttonNormalSprite;

        [SerializeField]
        Sprite _buttonPressedSprite;

        [SerializeField]
        Sprite _buttonHighlightedSprite;

        [SerializeField]
        Sprite _buttonDisabledSprite;

        [HtmlColor]
        [SerializeField]
        Color _buttonNormalColor = Color.white;

        [HtmlColor]
        [SerializeField]
        Color _buttonPressedColor = Color.white;

        [HtmlColor]
        [SerializeField]
        Color _buttonHighlightedColor = Color.white;

        [HtmlColor]
        [SerializeField]
        Color _buttonDisabledColor = Color.gray;

        [SerializeField]
        Sprite _sliderBackgroundSprite;

        [SerializeField]
        Sprite _sliderForegroundSprite;

        [SerializeField]
        Sprite _sliderHandleSprite;

        [HtmlColor]
        [SerializeField]
        Color _sliderBackgroundColor = Color.gray;

        [HtmlColor]
        [SerializeField]
        Color _sliderForegroundColor = Color.white;

        [HtmlColor]
        [SerializeField]
        Color _sliderHandleColor = Color.white;

        [SerializeField]
        Sprite _toggleBackgroundSprite;

        [SerializeField]
        Sprite _toggleForegroundSprite;

        [HtmlColor]
        [SerializeField]
        Color _toggleBackgroundColor = Color.white;

        [HtmlColor]
        [SerializeField]
        Color _toggleForegroundColor = Color.white;

        [SerializeField]
        Vector2 _toggleBackgroundSize = Vector2.one * 24f;

        [SerializeField]
        Vector2 _toggleForegroundSize = Vector2.one * 24f;

        [SerializeField]
        Sprite _scrollbarBackgroundSprite;

        [SerializeField]
        Sprite _scrollbarHandleSprite;

        [HtmlColor]
        [SerializeField]
        Color _scrollbarBackgroundColor = Color.gray;

        [HtmlColor]
        [SerializeField]
        Color _scrollbarHandleColor = Color.white;

        [SerializeField]
        float _scrollbarWidth = 16f;

        [SerializeField]
        Sprite _inputBackgroundSprite;

        [HtmlColor]
        [SerializeField]
        Color _inputBackgroundColor = Color.white;

        [HtmlColor]
        [SerializeField]
        Color _inputPlaceholderColor = Color.gray;

        [HtmlColor]
        [SerializeField]
        Color _inputSelectionColor = new Color32 (168, 206, 255, 192);

        [SerializeField]
        FontStyle _inputPlaceholderStyle = FontStyle.Italic;

        [SerializeField]
        float _inputMargin = 10f;

        public enum ButtonState {
            Normal,
            Pressed,
            Highlighted,
            Disabled
        }

        public enum SliderState {
            Background,
            Foreground,
            Handle
        }

        public enum ToggleState {
            Background,
            Foreground
        }

        public enum ScrollbarState {
            Background,
            Handle
        }

        public enum InputState {
            Background,
            Placeholder,
            Selection
        }

        public string GetName () {
            return _name;
        }

        public Sprite GetButtonSprite (ButtonState state) {
            switch (state) {
                case ButtonState.Normal:
                    return _buttonNormalSprite;
                case ButtonState.Pressed:
                    return _buttonPressedSprite;
                case ButtonState.Highlighted:
                    return _buttonHighlightedSprite;
                case ButtonState.Disabled:
                    return _buttonDisabledSprite;
                default:
                    return null;
            }
        }

        public Color GetButtonColor (ButtonState state) {
            switch (state) {
                case ButtonState.Normal:
                    return _buttonNormalColor;
                case ButtonState.Pressed:
                    return _buttonPressedColor;
                case ButtonState.Highlighted:
                    return _buttonHighlightedColor;
                case ButtonState.Disabled:
                    return _buttonDisabledColor;
                default:
                    return Color.black;
            }
        }

        public Sprite GetSliderSprite (SliderState state) {
            switch (state) {
                case SliderState.Background:
                    return _sliderBackgroundSprite;
                case SliderState.Foreground:
                    return _sliderForegroundSprite;
                case SliderState.Handle:
                    return _sliderHandleSprite;
                default:
                    return null;
            }
        }

        public Color GetSliderColor (SliderState state) {
            switch (state) {
                case SliderState.Background:
                    return _sliderBackgroundColor;
                case SliderState.Foreground:
                    return _sliderForegroundColor;
                case SliderState.Handle:
                    return _sliderHandleColor;
                default:
                    return Color.black;
            }
        }

        public Sprite GetToggleSprite (ToggleState state) {
            switch (state) {
                case ToggleState.Background:
                    return _toggleBackgroundSprite;
                case ToggleState.Foreground:
                    return _toggleForegroundSprite;
                default:
                    return null;
            }
        }

        public Color GetToggleColor (ToggleState state) {
            switch (state) {
                case ToggleState.Background:
                    return _toggleBackgroundColor;
                case ToggleState.Foreground:
                    return _toggleForegroundColor;
                default:
                    return Color.black;
            }
        }

        public Vector2 GetToggleSize (ToggleState state) {
            switch (state) {
                case ToggleState.Background:
                    return _toggleBackgroundSize;
                case ToggleState.Foreground:
                    return _toggleForegroundSize;
                default:
                    return Vector2.zero;
            }
        }

        public Sprite GetScrollbarSprite (ScrollbarState state) {
            switch (state) {
                case ScrollbarState.Background:
                    return _scrollbarBackgroundSprite;
                case ScrollbarState.Handle:
                    return _scrollbarHandleSprite;
                default:
                    return null;
            }
        }

        public Color GetScrollbarColor (ScrollbarState state) {
            switch (state) {
                case ScrollbarState.Background:
                    return _scrollbarBackgroundColor;
                case ScrollbarState.Handle:
                    return _scrollbarHandleColor;
                default:
                    return Color.black;
            }
        }

        public float GetScrollbarWidth () {
            return _scrollbarWidth;
        }

        public Sprite GetInputSprite () {
            return _inputBackgroundSprite;
        }

        public Color GetInputColor (InputState state) {
            switch (state) {
                case InputState.Background:
                    return _inputBackgroundColor;
                case InputState.Placeholder:
                    return _inputPlaceholderColor;
                case InputState.Selection:
                    return _inputSelectionColor;
                default:
                    return Color.black;
            }
        }

        public float GetInputMargin () {
            return _inputMargin;
        }

        public FontStyle GetInputPlaceholderStyle () {
            return _inputPlaceholderStyle;
        }
    }
}