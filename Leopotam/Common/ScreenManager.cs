// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EFramework.Common {
    /// <summary>
    /// Screen / scene manager, provides api for navigation with history rollback support.
    /// </summary>
    sealed class ScreenManager : MonoBehaviourService<ScreenManager> {
        /// <summary>
        /// Get previous screen name or null.
        /// </summary>
        public string Previous { get { return _history.Count > 0 ? _history.Peek () : null; } }

        /// <summary>
        /// Get current screen name.
        /// </summary>
        public string Current { get { return SceneManager.GetActiveScene ().name; } }

        readonly Stack<string> _history = new Stack<string> (8);

        protected override void OnCreateService () {
            DontDestroyOnLoad (gameObject);
        }

        protected override void OnDestroyService () { }

        /// <summary>
        /// Navigate to new screen.
        /// </summary>
        /// <param name="screenName">Target screen name.</param>
        /// <param name="saveToHistory">Save current screen to history for using NavigateBack later.</param>
        public void NavigateTo (string screenName, bool saveToHistory = false) {
            if (saveToHistory) {
                _history.Push (Current);
            }

            SceneManager.LoadScene (screenName);
        }

        /// <summary>
        /// Navigate back through saved in history screens.
        /// </summary>
        public void NavigateBack () {
#if UNITY_ANDROID
            if (_history.Count == 0) {
                Application.Quit ();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }
#endif
#if UNITY_EDITOR
            if (_history.Count == 0) {
                Debug.LogWarning ("Cant navigate back");
                return;
            }
#endif
            if (_history.Count > 0) {
                SceneManager.LoadScene (_history.Pop ());
            }
        }

        /// <summary>
        /// Force history clearup.
        /// </summary>
        public void ClearHistory () {
            _history.Clear ();
        }
    }
}