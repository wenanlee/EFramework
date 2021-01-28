// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using EFramework.Common;
using UnityEngine;

namespace EFramework.Fx {
    /// <summary>
    /// Fade manager service.
    /// </summary>
    sealed class FadeManager : MonoBehaviourService<FadeManager> {
        Color _fadeFrom;

        Color _fadeTo;

        float _invFadeTime;

        float _time;

        Action _callback;

        protected override void OnCreateService () {
            useGUILayout = false;
        }

        protected override void OnDestroyService () { }

        void OnGUI () {
            if (_invFadeTime <= 0f) {
                enabled = false;
                return;
            }

            if (Event.current.type != EventType.Repaint) {
                return;
            }

            _time = Mathf.Clamp01 (_time + Time.deltaTime * _invFadeTime);
            var color = Color.Lerp (_fadeFrom, _fadeTo, _time);

            var savedColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture (
                new Rect (0, 0, Screen.width, Screen.height),
                Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.color = savedColor;

            if (_time >= 1f) {
                if (color.a <= 0f) {
                    enabled = false;
                }
                if (_callback != null) {
                    var cb = _callback;
                    _callback = null;
                    cb ();
                }
            }
        }

        /// <summary>
        /// Process fading from one color to another as fullscreen overlayed quad.
        /// </summary>
        /// <param name="start">Source opaque status.</param>
        /// <param name="end">Target opaque status.</param>
        /// <param name="time">Time of fading.</param>
        /// <param name="onSuccess">Optional callback on success ending of fading.</param>
        public void Process (Color start, Color end, float time, Action onSuccess = null) {
            _fadeFrom = start;
            _fadeTo = end;
            _callback = onSuccess;
            _time = 0f;
            _invFadeTime = time > 0f ? 1f / time : 0f;
            enabled = _invFadeTime > 0f;
        }
    }
}