// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Common;
using UnityEngine;

namespace EFramework.EditorHelpers {
    /// <summary>
    /// Fps counter service.
    /// </summary>
    sealed class FpsCounter : MonoBehaviourService<FpsCounter> {
        const int UpdateFrequency = 2;

        const float InvUpdatesPerSecond = 1 / (float) UpdateFrequency;

        const float BaseFontSize = 16 / 768f;

        int _frameCount;

        float _lastTime;

        [SerializeField]
        bool _drawShadow = true;

        [SerializeField]
        float _xOffset;

        [SerializeField]
        float _yOffset;

        [SerializeField]
        TextAnchor _anchor = TextAnchor.LowerLeft;

        GUIStyle _style;

        Rect _rect;

        Rect _rectShadow;

        Color _color = Color.white;

        readonly StringCache<int> _stringCache = new StringCache<int> (null, null, 256);

        protected override void OnCreateService () {
            DontDestroyOnLoad (gameObject);
            useGUILayout = false;
            _style = new GUIStyle ();
            _style.normal.textColor = Color.white;
            _style.alignment = _anchor;
            CalculateRect ();
        }

        protected override void OnDestroyService () { }

        void CalculateRect () {
            _rect = new Rect (_xOffset, _yOffset, Screen.width, Screen.height);
            _rectShadow = new Rect (_xOffset + 1, _yOffset + 1, Screen.width, Screen.height);
            var fontSize = (int) (BaseFontSize * Screen.height);
            _style.fontSize = fontSize;
        }

        void OnGUI () {
            if (Event.current.type != EventType.Repaint) {
                return;
            }
            var fpsString = _stringCache.Get (CurrentFps);
#if UNITY_EDITOR
            CalculateRect ();
#endif
            if (_drawShadow) {
                GUI.color = Color.black;
                GUI.Label (_rectShadow, fpsString, _style);
            }
            GUI.color = _color;
            GUI.Label (_rect, fpsString, _style);
        }

        void Update () {
            var currTime = Time.realtimeSinceStartup;
            if (currTime - _lastTime > InvUpdatesPerSecond) {
                CurrentFps = _frameCount * UpdateFrequency;
                _frameCount = 1;
                _lastTime = currTime;
            } else {
                _frameCount++;
            }
        }

        /// <summary>
        /// Set draw shadow state.
        /// </summary>
        /// <param name="state">Draw shadow or not.</param>
        public void SetDrawShadowState (bool state) {
            _drawShadow = state;
        }

        /// <summary>
        /// Set fps label color.
        /// </summary>
        /// <param name="color">Target color.</param>
        public void SetColor (Color color) {
            _color = color;
        }

        /// <summary>
        /// Set position of fps label.
        /// </summary>
        /// <param name="xOffset">OffsetX from anchor position.</param>
        /// <param name="yOffset">OffsetY from anchor position.</param>
        /// <param name="anchor">Base anchor position.</param>
        public void SetPosition (float xOffset, float yOffset, TextAnchor anchor) {
            _xOffset = xOffset;
            _yOffset = yOffset;
            _style.alignment = anchor;
            CalculateRect ();
        }

        /// <summary>
        /// Get current fps.
        /// </summary>
        public int CurrentFps { get; private set; }
    }
}