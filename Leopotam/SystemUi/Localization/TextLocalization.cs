// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Localization;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace EFramework.SystemUi.Localization {
    /// <summary>
    /// Localization helper for System UI Text.
    /// </summary>
    [RequireComponent (typeof (Text))]
    public sealed class TextLocalization : MonoBehaviour {
        [SerializeField]
        string _token = null;

        Text _text;

        void OnEnable () {
            OnLocalize ();
        }

        public void SetToken (string token) {
            if (string.CompareOrdinal (_token, token) != 0) {
                _token = token;
                OnLocalize ();
            }
        }

        [Preserve]
        void OnLocalize () {
            if (!string.IsNullOrEmpty (_token)) {
                if ((object) _text == null) {
                    _text = GetComponent<Text> ();
                }
                _text.text = Localizer.Get (_token);
            }
        }
    }
}